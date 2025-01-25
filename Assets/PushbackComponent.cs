using UnityEngine;
public class PushbackComponent : MonoBehaviour
{
  
    [SerializeField] private float pushPower = 100f;
    private float lastPushbackTime = 0f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyController>(out var enemy))
        {
            //enemy.AddForce(enemy.transform.position-transform.position * pushPower);
        }
    }
}

