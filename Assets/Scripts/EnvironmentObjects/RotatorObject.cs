using UnityEngine;

namespace EnvironmentObjects
{
    public class RotatorObject : MonoBehaviour
    {
        public float rotationSpeed = 1f;


        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
