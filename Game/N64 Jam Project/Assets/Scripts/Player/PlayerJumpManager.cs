using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpManager : MonoBehaviour
{
    public PlayerInputManager playerInput;
    public PlayerMovementManager movementManager;
    public Rigidbody rb;
    public PlayerCrouchManager crouchManager;

    [NonSerialized] public float jumpForce;
    [NonSerialized] public float jumpCooldown;
    private bool readyToJump = true;

    public void JumpUpdate()
    {
        if(Input.GetKeyDown(playerInput.jumpKey) && readyToJump && movementManager.grounded && !crouchManager.crouching)
        {
            readyToJump = false;

            JumpEvent();

            Invoke(nameof(ResetJumpEvent), jumpCooldown);
        }
    }

    private void JumpEvent()
    {
        movementManager.exitingSlope = true;


        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJumpEvent()
    {
        readyToJump = true;

        movementManager.exitingSlope = false;
    }
}
