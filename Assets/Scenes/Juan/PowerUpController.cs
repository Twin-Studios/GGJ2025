using Unity.VisualScripting;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private PowerAOEPulse power_AOEPulse;
    [SerializeField] private WeaponShooter weaponShooter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CollectablePowerUp>(out var collectable))
            {
            switch (collectable.Type)
            {
                case PowerUpType.BulletPower:
                    weaponShooter.BulletPower += weaponShooter.BulletPower / 2;
                    break;
                case PowerUpType.BulletNumber:
                    weaponShooter.BulletNumber += 1;
                    break;
                case PowerUpType.BulletSize:
                    weaponShooter.BulletSize += weaponShooter.BulletSize/2;
                    break;
                case PowerUpType.BulletFireRate:
                    weaponShooter.BulletFireRate -= weaponShooter.BulletFireRate / 2;
                    break;
            }
            Destroy(collectable.gameObject);
        }
    }
}
