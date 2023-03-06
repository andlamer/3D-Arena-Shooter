using System;

namespace ShooterBase.Core
{
    public class CharacterStat
    {
        public event Action MaxStatReached;
        public event Action MinStatReached;
        public event Action<int> StatUpdated;

        private readonly int _maxStatAmount;
        private readonly int _minStatAmount;
        private int _currentStatAmount;
        private readonly int _startingStatAmount;
        private readonly bool _isStatInitialized;

        public void Reset()
        {
            if (!_isStatInitialized) return;

            _currentStatAmount = _startingStatAmount;
        }

        public void DecreaseStat(int amount)
        {
            _currentStatAmount -= amount;
            _currentStatAmount = Math.Clamp(_currentStatAmount, _minStatAmount, _maxStatAmount);
            StatUpdated?.Invoke(_currentStatAmount);

            if (_currentStatAmount == _minStatAmount)
                MinStatReached?.Invoke();
        }

        public void IncreaseStat(int amount)
        {
            _currentStatAmount += amount;
            _currentStatAmount = Math.Clamp(_currentStatAmount, _minStatAmount, _maxStatAmount);
            StatUpdated?.Invoke(_currentStatAmount);
            
            if (_currentStatAmount == _maxStatAmount)
                MaxStatReached?.Invoke();
        }

        public void SetToPercent(int percent)
        {
            _currentStatAmount = (int)(percent / 100f * _maxStatAmount);
            StatUpdated?.Invoke(_currentStatAmount);

            if (_currentStatAmount == _maxStatAmount)
            {
                MaxStatReached?.Invoke();
            }
            else if (_currentStatAmount == _minStatAmount)
            {
                MinStatReached?.Invoke();
            }
        }
        
        public int GetStatPercent() => _currentStatAmount * 100 / _maxStatAmount;

        public CharacterStat(int maxAmount, int startingAmount, int minStatAmount)
        {
            {
                _maxStatAmount = maxAmount;
                _minStatAmount = minStatAmount;
                _currentStatAmount = startingAmount;
                _startingStatAmount = startingAmount;
                _isStatInitialized = true;
            }
        }
    }
}