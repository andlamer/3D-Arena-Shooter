using UnityEngine;
using Zenject;
using ScriptableObject = Zenject.ScriptableObject;

namespace ShooterBase.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "ScriptableObjects/CameraSettings", order = 1)]
    public class CameraSettings : ScriptableObject, ICameraSettings
    {
        [SerializeField] private float xRotationSpeed = 500f;
        [SerializeField] private float yRotationSpeed = 500f;

        public Vector2 GetCameraRotationSpeed() => new(xRotationSpeed, yRotationSpeed);

        public override void InstallBindings()
        {
            Container.Bind<ICameraSettings>().FromInstance(this).AsSingle();
        }
    }
}