using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private LookAtConstraint lookAtconstraint;
    [SerializeField] private float maxLinearVelocity = 3f;
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private PlayerController3D player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxLinearVelocity;
        StartCoroutine(TryGetTarget());
    }

    IEnumerator TryGetTarget()
    {
        while (lookAtconstraint.sourceCount == 0)
        {
            if (player == null)
            {
                player = FindFirstObjectByType<PlayerController3D>();
                
            }
            else
            {
                ConstraintSource source = new ConstraintSource();
                source.sourceTransform = player.transform;
                source.weight = 1;
                lookAtconstraint.AddSource(source);
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player == null)
        {
            return;
        }
        rb.maxLinearVelocity = maxLinearVelocity;
        rb.AddForce((player.transform.position - transform.position).normalized* pushForce);
    }

    public void AddForce(Vector3 force)
    {
        force.y = transform.position.y;
        rb.AddForce(force, ForceMode.Impulse);
    }
}
