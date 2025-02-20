using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShooter : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform spawnPos;
    public float BulletPower = 10f;
    public int BulletNumber = 1;
    public float BulletSize = 0.1f;
    public float BulletFireRate = 0.2f;

    InputAction fireAction;

    private float _nextFireTime = 0;

    [SerializeField]
    private PlayerController3D playerController3D;

    [SerializeField]
    private AudioClip shootSound;
    [SerializeField]
    private float volume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireAction = playerController3D.PlayerInput.actions["Attack"];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(spawnPos.position, spawnPos.forward * 10);
    }

    private void Update()
    {
        spawnPos.rotation = playerController3D.PlayerCamera.transform.rotation;
        _nextFireTime -= Time.deltaTime;

        if (_nextFireTime<0 && fireAction.IsPressed())
        {
           StartCoroutine(ShotCoroutine());
        }

    }

    private IEnumerator ShotCoroutine()
    {
        _nextFireTime = BulletFireRate;
        playerController3D.LookAtCameraDirection();
        yield return new WaitForSeconds(0.01f);
        Shoot(BulletNumber);
      
    }

    //private void Shoot()
    //{
    //  var bullet =  Instantiate(bulletPrefab, spawnPos.position, transform.rotation);
    //    bullet.AddForce(spawnPos.forward*bulletForce);
    //}

    public void Shoot(int bulletNumber)
    {
        float angleStep = 60 / bulletNumber;
        float offset = (bulletNumber / 2) * angleStep;
        var angle = 0f;
        for (int i = 0; i < bulletNumber; i++)
        {
            var instantiatedObj = Instantiate(bulletPrefab).GetComponent<Bullet>();
            instantiatedObj.transform.position = spawnPos.position;
            instantiatedObj.transform.rotation = spawnPos.rotation;

            var rotationEuler = spawnPos.rotation.eulerAngles;
            rotationEuler.y += angle - offset;
            instantiatedObj.transform.rotation = Quaternion.Euler(rotationEuler);

            //instantiatedObj.transform.rotation = Quaternion.AngleAxis(angle - offset + spawnPos.rotation.eulerAngles.y, Vector3.up);
            angle += angleStep;

            instantiatedObj.AddForce(instantiatedObj.transform.forward * BulletPower);

            instantiatedObj.transform.localScale = Vector3.one * BulletSize;

        }


        SFXManager.Instance.PlayOneShot(shootSound, volume);
    }

}
