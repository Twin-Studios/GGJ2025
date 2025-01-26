using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Animations;
using UnityEngine;
using Assets.Scenes.Juan;
using UnityEngine.Events;

namespace Assets
{
    public class BallController : MonoBehaviour
    {
        private Rigidbody rb;

        [SerializeField] private bool limitVelocity = true;
        [SerializeField] private float maxLinearVelocity = 3f;
 
        [SerializeField] private float gravity = 9.8f;

        [field: SerializeField]
        public List<GameObject> Contents { get; private set; }

        public bool CanMerge { get; set; } = true;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rb = GetComponent<Rigidbody>();

            FindFirstObjectByType<PlayerManager>().GameStarted.AddListener(OnGameStarted);

            rb.isKinematic = true;
        }

        private void OnGameStarted()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (limitVelocity == false) return;

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

        private void OnTriggerEnter(Collider other)
        {
            

        }
    }
}
