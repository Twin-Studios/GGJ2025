using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private LookAtConstraint lookAtconstraint;
    [SerializeField] private float maxLinearVelocity = 3f;
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private PlayerController3D player;

    [SerializeField] private float gravity = 9.8f;

    [field:SerializeField]
    public List<GameObject> Contents { get; private set; }

    public bool CanMerge { get; set; } = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(TryGetTarget());
    }

    IEnumerator TryGetTarget()
    {
        while (player == null)
        {
            player = FindFirstObjectByType<PlayerController3D>();

            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null) return;
       
        rb.AddForce((player.transform.position - transform.position).normalized* pushForce);

        rb.linearVelocity = new Vector3(
            Mathf.Min(rb.linearVelocity.x, maxLinearVelocity),
            rb.linearVelocity.y + -1 * gravity * Time.fixedDeltaTime,
            Mathf.Min(rb.linearVelocity.z, maxLinearVelocity));
    }

    public void AddForce(Vector3 force)
    {
        force.y = transform.position.y;
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(CanMerge == false)
            return;

        Debug.Log($"Enemy {name} collided with {collision.gameObject.name}");

        var enemy = collision.gameObject.GetComponent<EnemyController>();

        if (enemy == null)
            return;

        enemy.CanMerge = false;


        foreach (var content in enemy.Contents)
        {
            content.transform.SetParent(transform);
            Contents.Add(content);
        }

        //pushForce += enemy.pushForce;

        Destroy(enemy);
    }
}
