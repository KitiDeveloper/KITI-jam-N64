using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallRunManager : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public PlayerMovementManager movementManager;
    public Rigidbody rb;
    public PlayerInputManager inputManager;
    public PlayerJumpManager jumpManager;

    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    public float wallRunTimer;

    [Header("Detection")]
    public float wallCheckDistence;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    public bool isWallRunning = false;

    private void Start()
    {
        wallRunTimer = maxWallRunTime;
    }

    void Update()
    {
        CheckForWall();
        WallRunEvent();

        if (Input.GetKeyDown(inputManager.jumpKey) && jumpManager.readyToJump && isWallRunning)
        {
            jumpManager.readyToJump = false;

            jumpManager.JumpEvent();

            Invoke(nameof(ResetJumpEvent), jumpManager.jumpCooldown);
        }
    }

    private void FixedUpdate()
    {
        if (isWallRunning) { WallRunningMovement(); }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistence, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out rightWallHit, wallCheckDistence, whatIsWall);
    }

    private bool AboveGroundCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void WallRunEvent()
    {
        if((wallLeft || wallRight) && inputManager.verticalInput > 0 && AboveGroundCheck() && jumpManager.readyToJump)
        {
            wallRunTimer -= Time.deltaTime;
            if (!isWallRunning) { StartWallRunEvent(); }
        }

        else
        {
            if (isWallRunning) { StopWallRunEvent(); }
        }

        if (wallRunTimer <= 0)
        {
            StopWallRunEvent();
        }
    }

    private void StartWallRunEvent()
    {
        isWallRunning = true;

        wallRunTimer = maxWallRunTime;
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, orientation.transform.up);

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
    }

    private void StopWallRunEvent()
    {
        wallRunTimer = maxWallRunTime;

        isWallRunning = false;
    }

    public void ResetJumpEvent()
    {
        jumpManager.readyToJump = true;

        movementManager.exitingSlope = false;
    }
}
