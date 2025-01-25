using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShooter : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private float fireRate = 0.2f;
    InputAction fireAction;

    private float _nextFireTime = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireAction = InputSystem.actions.FindActionMap("Player")["Attack"];
    }

    private void Update()
    {
        _nextFireTime -= Time.deltaTime;

        if (_nextFireTime<0 && fireAction.IsPressed())
        {
            Shoot();
            _nextFireTime = fireRate;
        }
    }

    private void Shoot()
    {
      var bullet =  Instantiate(bulletPrefab, spawnPos.position, transform.rotation);
        bullet.AddForce(spawnPos.forward*bulletForce);
    }

}
