using System.Collections;
using UnityEngine;

public class Autodestroy : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }

    
}
