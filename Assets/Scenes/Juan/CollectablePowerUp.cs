using TMPro;
using UnityEngine;

public class CollectablePowerUp : MonoBehaviour
{
    public PowerUpType Type;
    public string Name;
    [SerializeField] private TextMeshProUGUI textName;

    private void Start()
    {
        textName.text = "+"+Name;
    }
}



public enum PowerUpType
{
    BulletPower,
    BulletNumber,
    BulletSize,
    BulletFireRate,
    Ability_AOEPulse
}


