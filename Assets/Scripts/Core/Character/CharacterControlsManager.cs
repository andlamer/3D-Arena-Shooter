using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShooterBase.Core
{
    public class CharacterControlsManager : MonoBehaviour
    {
        [SerializeField] private Joystick movementJoystick;
        [SerializeField] private Joystick cameraJoystick;
        [SerializeField] private Button shootButton;
        [SerializeField] private Button ultimateButton;

        private Character _cachedCharacter;

        [Inject]
        private void Construct(Character character)
        {
            _cachedCharacter = character;
        }
        
        private void Start()
        {
            shootButton.onClick.AddListener(OnShootButtonClick);
            ultimateButton.onClick.AddListener(OnUltimateButtonClick);
        }

        private void OnDestroy()
        {
            shootButton.onClick.RemoveAllListeners();
            ultimateButton.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            if (_cachedCharacter != null)
                _cachedCharacter.MoveCharacter(movementJoystick.Direction, cameraJoystick.Direction);
        }

        private void OnShootButtonClick()
        {
            if (_cachedCharacter != null)
                _cachedCharacter.Shoot();
        }

        private void OnUltimateButtonClick()
        {
            if (_cachedCharacter != null)
                _cachedCharacter.UseUltimate();
        }
    }
}

