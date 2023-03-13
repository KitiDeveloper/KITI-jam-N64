using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonEngine : MonoBehaviour
{
    [Header("Activater")]
    public bool canMove;
    public bool canJump;
    public bool canCrouch;

    [Header("Engine workers")]
    public PlayerMovementManager movementManager;
    public PlayerJumpManager jumpManager;
    //public PlayerInputManager playerInput;
    public PlayerCrouchManager crouchManager;

    [Header("Player stats")]
    public float inAirSpeed = 0.1f;
    public float crouchSpeed = 1.5f;
    public float walkSpeed = 4.0f;
    public float sprintSpeed = 8.0f;
    public float groundDrag = 5.0f;
    public float jumpForce = 6.0f;
    public float jumpCooldown = 1.5f;
    public float maxSlopeAngle = 40.0f;

    private Gun gun;

    void Awake()
    {
        movementManager.inAirSpeed = inAirSpeed;
        movementManager.crouchSpeed = crouchSpeed;
        movementManager.walkSpeed = walkSpeed;
        movementManager.sprintSpeed = sprintSpeed;

        movementManager.groundDrag = groundDrag;
        movementManager.maxSlopeAngle = maxSlopeAngle;

        jumpManager.jumpForce = jumpForce;
        jumpManager.jumpCooldown = jumpCooldown;
    }

    void Update()
    {
        if (canJump)
        {
            jumpManager.JumpUpdate();
        }
        if(canMove) 
        {
            movementManager.MovementUpdate(); 
            movementManager.StateHandler();
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!gun)
            {
                gun = GetComponentInChildren<Gun>();
                if (!gun) return;
            }
            gun.Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (canMove) { movementManager.MovePlayer(); }
    }
}
