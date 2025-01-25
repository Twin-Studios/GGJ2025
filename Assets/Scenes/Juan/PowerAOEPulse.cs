using UnityEngine;

public class PowerAOEPulse : MonoBehaviour
{
    [SerializeField] private GameObject pulsePrefab;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float fireRate = 1f;

    private float _nextFireTime = 0;
    private float _currentDuration = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        _nextFireTime = 0;
        _currentDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        _nextFireTime -= Time.deltaTime;

        if (_nextFireTime < 0)
        {
            CreatePulse();
            _nextFireTime = fireRate;
        }

        _currentDuration -= Time.deltaTime;
        if (_currentDuration < 0)
            gameObject.SetActive(false);
    }

    public void CreatePulse()
    {
        Instantiate(pulsePrefab, transform.position,Quaternion.identity);
    }

    public void StartOrRefresh()
    {
        if (!isActiveAndEnabled)
        { 
            gameObject.SetActive(true);
        }
        else
        {
            _currentDuration = duration;
        }
    }
}
