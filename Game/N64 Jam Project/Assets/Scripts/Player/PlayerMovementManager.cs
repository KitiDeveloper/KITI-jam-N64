using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    [NonSerialized] public float moveSpeed;
    [NonSerialized] public float lowCrouchSpeed = 1.5f;
    [NonSerialized] public float crouchSpeed;
    [NonSerialized] public float walkSpeed;
    [NonSerialized] public float sprintSpeed;
    [NonSerialized] public float groundDrag = 5;
    [NonSerialized] public float inAirSpeed = 0.2f;
    [NonSerialized] public float playerHeight;
    [NonSerialized] public float maxSlopeAngle;

    [NonSerialized] public bool grounded;
    [NonSerialized] public bool exitingSlope;

    public LayerMask whatIsGround;

    public Transform orientation;
    public CapsuleCollider playerObj;

    public PlayerInputManager playerInput;
    public FirstPersonEngine fPEngine;
    public PlayerCrouchManager crouchManager;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private RaycastHit slopeHit;

    public MovementState State;

    public enum MovementState
    {
        crouching,
        walking,
        sprinting,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerHeight = playerObj.height;
    }

    public void StateHandler()
    {
        //When crouching
        if (crouchManager.crouching)
        {
            State = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        else if (crouchManager.lowCrouching)
        {
            State = MovementState.crouching;
            moveSpeed = lowCrouchSpeed;
        }

        //When sprinting
        else if(grounded && Input.GetKey(playerInput.SprintKey))
        {
            State = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        //When walking
        else if (grounded)
        {
            State = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        //When in the air
        else
        {
            State = MovementState.air;
        }
    }

    public void MovementUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded) {rb.drag = groundDrag;} else {rb.drag = 0;}

        SpeedControl();
    }

    public void MovePlayer()
    {
        moveDirection = orientation.forward * playerInput.verticalInput + orientation.right * playerInput.horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDir() * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if(grounded == true) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); }
        else if (grounded == false) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f * inAirSpeed, ForceMode.Force); }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        //Limits speed on slopes
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed) { rb.velocity = rb.velocity.normalized * moveSpeed; }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 LimitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(LimitedVel.x, rb.velocity.y, LimitedVel.z);
            }
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDir()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
