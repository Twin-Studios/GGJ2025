using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float maxLinearVelocity = 3f;
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private PlayerController3D player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxLinearVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        rb.maxLinearVelocity = maxLinearVelocity;
        rb.AddForce((player.transform.position - transform.position).normalized* pushForce);
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }
}
