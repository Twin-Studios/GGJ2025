using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject vfxExplosion;
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
        var enemy = other.GetComponentInParent<EnemyController>();

        if (enemy != null)
        {
            enemy.AddForce(rb.linearVelocity* pushForce);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(vfxExplosion, transform.position, Quaternion.identity);
      
    }
}
