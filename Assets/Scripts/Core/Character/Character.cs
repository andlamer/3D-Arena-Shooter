using System.Collections.Generic;
using ShooterBase.ScriptableObjects;
using ShooterBase.Services;
using UnityEngine;
using Zenject;

namespace ShooterBase.Core
{
    public class Character : MonoBehaviour, IStatsDamageable
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform targetCharacterBodyTransform;
        [SerializeField] private Transform targetCharacterHeadTransform;

        [SerializeField] private Transform groundChecker;
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private BasicWeapon weapon;
        [SerializeField] private UltimateAbility ultimateAbility;

        private const float Gravity = -9.81f;
        private const float GroundDistance = 0.1f;
        private const float GravityResetVelocityY = -2f;

        private static readonly Vector2 CameraVerticalMinMaxAngles = new(-50f, 80f);

        private ICharacterStatsService _characterStatsService;
        private ICameraSettings _cameraSettings;
        private ICharacterSettings _characterSettings;

        private float _cameraRotation;
        private Vector3 _velocity;
        private Vector2 _cameraRotationSpeed;
        private Vector2 _playerMovementSpeed;
        private float _gravityMultiplier;

        [Inject]
        private void Construct(ICharacterStatsService characterStatsService, ICameraSettings cameraSettings, ICharacterSettings characterSettings)
        {
            _characterStatsService = characterStatsService;
            _cameraSettings = cameraSettings;
            _characterSettings = characterSettings;
        }

        private void Start()
        {
            _cameraRotationSpeed = _cameraSettings.GetCameraRotationSpeed();
            _playerMovementSpeed = _characterSettings.GetPlayerMovementSpeed();
            _gravityMultiplier = _characterSettings.GetPlayerGravityMultiplier();
        }

        public void MoveCharacter(Vector2 bodyMovement, Vector2 cameraMovement)
        {
            _cameraRotation -= cameraMovement.y * Time.deltaTime * _cameraRotationSpeed.y;
            _cameraRotation = Mathf.Clamp(_cameraRotation, CameraVerticalMinMaxAngles.x, CameraVerticalMinMaxAngles.y);
            targetCharacterHeadTransform.localRotation = Quaternion.Euler(_cameraRotation, 0f, 0f);

            targetCharacterBodyTransform.Rotate(Vector3.up * cameraMovement.x * Time.deltaTime * _cameraRotationSpeed.x);

            var bodyMove = targetCharacterBodyTransform.right * bodyMovement.x * _playerMovementSpeed.x
                           + targetCharacterBodyTransform.forward * bodyMovement.y * _playerMovementSpeed.y;
            characterController.Move(bodyMove * Time.deltaTime);

            if (Physics.CheckSphere(groundChecker.position, GroundDistance, groundLayer) && _velocity.y < 0)
                _velocity.y = GravityResetVelocityY;

            _velocity.y += Gravity * Time.deltaTime * _gravityMultiplier;
            characterController.Move(_velocity * Time.deltaTime);
        }

        public void Shoot() => weapon.Shoot();

        public void UseUltimate()
        {
            ultimateAbility.Use();
            _characterStatsService.DecreaseStat(CreatureStats.Strength, _characterSettings.GetUltimateAbilityStrengthCost());
        }

        public void TakeDamage(IEnumerable<DamageToAffectedStat> damageToStats)
        {
            foreach (var affectedStat in damageToStats)
            {
                _characterStatsService.DecreaseStat(affectedStat.AffectedCreatureStat, affectedStat.Damage);
            }
        }

        public bool IsDead() => _characterStatsService.GetStatPercent(CreatureStats.Health) == 0;
    }
}