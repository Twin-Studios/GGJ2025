using TMPro;
using UnityEngine;

public class CollectablePowerUp : MonoBehaviour
{
    public PowerUpType Type;
    [SerializeField] private TextMeshProUGUI textName;

    private void Start()
    {
        textName.text = "+"+ Type.ToString();
    }
}



public enum PowerUpType
{
    BulletPower,
    BulletNumber,
    BulletSize,
    BulletFireRate
}


