using System.Collections;
using System.Collections.Generic;
using N64Jam.Control;
using UnityEngine;

namespace N64Jam.States
{

    public class PlayerStateMachine : StateMachine
    {
        [SerializeField] private float rotationSpeed = 10f;
        [HideInInspector] public InputHandler inputHandler { get; private set; }
        [HideInInspector] public Mover mover { get; private set; }
        
        
        //States 
        private PlayerStandingState playerStandingState;

        private Transform cameraTransform;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            mover = GetComponent<Mover>();
            playerStandingState = new PlayerStandingState(this);
            cameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            SwitchState(playerStandingState);
        }

        public Vector3 CalculateMovementDirection()
        {
            Vector3 inputDirection = cameraTransform.right * inputHandler.MoveInput.x;
            inputDirection += cameraTransform.forward * inputHandler.MoveInput.y;
            inputDirection.y = 0;
            inputDirection.Normalize();
            return inputDirection;
        }
        
        public void FaceMovementDirection(Vector3 moveDirection, float deltaTime)
        {
            if (moveDirection == Vector3.zero) moveDirection = transform.forward;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection),
                deltaTime * rotationSpeed);
        }
        
        
    }
}
