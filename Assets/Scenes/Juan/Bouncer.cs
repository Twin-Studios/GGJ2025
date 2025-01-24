using UnityEngine;
using UnityEngine.EventSystems;

public class Bouncer : MonoBehaviour
{
    [SerializeField] private PlayerController3D player;
    [SerializeField] private float force = 200;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger entered");
    }

    Vector3 collisionNormal;
    Vector3 collisionPoint;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision entered");
        collisionPoint = collision.contacts[0].point;
        collisionNormal = collision.contacts[0].normal;
        player.rb.AddForce(Vector3.Reflect(player.rb.linearVelocity, -collisionNormal) *force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(collisionPoint, -collisionNormal*5);
    }
    
}
