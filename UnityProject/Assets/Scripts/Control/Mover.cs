using UnityEngine;

namespace N64Jam.Control
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 5f;
        private Rigidbody rb;
        private Vector3 moveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void MoveTo(Vector3 newDirection)
        {
            moveDirection = newDirection;
            //rb.AddForce(moveDirection.normalized * movementSpeed, ForceMode.Acceleration);
            rb.velocity = moveDirection.normalized * movementSpeed;
        }

        public void RotateCharacter()
        {
            if (moveDirection != Vector3.zero)
            {
                gameObject.transform.forward = moveDirection;
            }
        }
    }
}