using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float pushForce = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<EnemyController>(out var enemy))
        {
            enemy.AddForce(rb.linearVelocity* pushForce);
        }
    }
}
