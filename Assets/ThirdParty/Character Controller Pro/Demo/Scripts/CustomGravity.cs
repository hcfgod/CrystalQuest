using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightbug.CharacterControllerPro.Demo
{
    [RequireComponent(typeof(Rigidbody))]
    public class CustomGravity : MonoBehaviour
    {
        public Transform planet;
        public float gravity = 10f;

        new Rigidbody rigidbody;

        private void Awake()
        {
            if (planet == null)
            {
                enabled = false;
                return;
            }

            rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
        }

        void FixedUpdate()
        {
            Vector3 dir = (planet.position - transform.position).normalized;
            rigidbody.velocity += dir * gravity * Time.deltaTime;
        }

    }

}

