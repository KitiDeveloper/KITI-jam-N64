using UnityEngine;
using UnityEngine.InputSystem;

namespace N64Jam.Control
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerControls playerControls;
        public Vector2 MoveInput { get; private set; }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        public void OnMovement(InputValue value)
        {
            MoveInput = value.Get<Vector2>();
        }
    }
}