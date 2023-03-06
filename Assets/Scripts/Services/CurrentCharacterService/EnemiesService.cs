using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShooterBase.Core;
using ShooterBase.ScriptableObjects;
using UnityEngine;

namespace ShooterBase.Services
{
    public class EnemiesService : IEnemiesService, IDisposable
    {
        private readonly IEnemySpawnSettings _enemySpawnSettings;
        private readonly BossEnemy.Pool _bossPool;
        private readonly DivingEnemy.Pool _divingEnemyPool;
        private readonly IRespawnPointsService _respawnPointsService;

        private readonly List<IEnemy> _activeEnemies = new();

        private CancellationTokenSource _cancellationTokenSource;

        private bool _spawnEnabled;
        private int _currentEnemiesKilledAmount;
        private float _currentRespawnTime;
        private float _currentRespawnRate = 1f;

        public EnemiesService(IEnemySpawnSettings enemySpawnSettings, BossEnemy.Pool bossPool, DivingEnemy.Pool divingEnemyPool, IRespawnPointsService respawnPointsService)
        {
            _enemySpawnSettings = enemySpawnSettings;
            _bossPool = bossPool;
            _divingEnemyPool = divingEnemyPool;
            _respawnPointsService = respawnPointsService;
        }

        public void StartSpawning()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _currentRespawnRate = 1f;
            _currentRespawnTime = _enemySpawnSettings.MaxRespawnTime;
            _spawnEnabled = true;
            Spawn().Forget();
        }

        private async UniTask Spawn()
        {
            var divingWavesSpawned = 0;

            while (_spawnEnabled)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_currentRespawnTime), cancellationToken: _cancellationTokenSource.Token);

                if (Math.Abs(divingWavesSpawned - (_enemySpawnSettings.DivingToBossRatio - 1)) < float.Epsilon * 100)
                {
                    for (var i = 0; i < Math.Truncate(_currentRespawnRate); i++)
                    {
                        var bossEnemy = _bossPool.Spawn(_respawnPointsService.GetRandomEnemySpawnPoint());
                        bossEnemy.OnEnemyDead += OnEnemyDeath;
                        _activeEnemies.Add(bossEnemy);
                    }

                    divingWavesSpawned = 0;
                }
                else
                {
                    for (var i = 0; i < Math.Truncate(_currentRespawnRate); i++)
                    {
                        var divingEnemy = _divingEnemyPool.Spawn(_respawnPointsService.GetRandomEnemySpawnPoint());
                        divingEnemy.OnEnemyDead += OnEnemyDeath;
                        _activeEnemies.Add(divingEnemy);
                    }

                    divingWavesSpawned += 1;
                }

                _currentRespawnTime = Mathf.Clamp(_currentRespawnTime - _enemySpawnSettings.RespawnTimeDecreasePerEnemy, _enemySpawnSettings.MinRespawnTime,
                    _enemySpawnSettings.MaxRespawnTime);

                if (Math.Abs(_currentRespawnTime - _enemySpawnSettings.MinRespawnTime) < float.Epsilon * 100)
                    _currentRespawnRate += _enemySpawnSettings.RespawnAmountIncreasePerEnemy;
            }
        }

        private void OnEnemyDeath(IEnemy enemy)
        {
            enemy.OnEnemyDead -= OnEnemyDeath;

            if (_activeEnemies.Contains(enemy))
                _activeEnemies.Remove(enemy);
        }

        public void WipeAll()
        {
            foreach (var enemy in _activeEnemies)
            {
                enemy.OnEnemyDead -= OnEnemyDeath;
                enemy.Kill();
            }

            _activeEnemies.Clear();
        }

        public void Dispose()
        {
            _spawnEnabled = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}