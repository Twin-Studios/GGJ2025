using UnityEngine;

public class NetBalanceBone : MonoBehaviour
{
    [SerializeField] private float repositionForce = 5;

    private Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, startPos,Time.deltaTime * repositionForce);
    }
}
