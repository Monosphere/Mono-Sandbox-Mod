using UnityEngine;

namespace MonoSandbox.Behaviours.Tools
{
    public class LimitVelocity : MonoBehaviour
    {
        public float maxSpeed = 3f;
        Rigidbody rb = null;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }
}