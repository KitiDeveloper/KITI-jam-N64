using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSlideManager : MonoBehaviour
{
    [Header("references")]
    public PlayerInputManager inputManager;
    public Transform orientation;
    public Transform playerObj;
    public Rigidbody rb;
    public PlayerMovementManager pm;
    public PlayerCrouchManager crouchManager;

    [Header("Sliding")]
    public float maxSlideTime = 0.7f;
    public float slideForce = 200;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    public bool sliding;

    void Start()
    {
        startYScale = playerObj.localScale.y;
    }

    void Update()
    {
        if(Input.GetKeyDown(inputManager.SlideKey) && (inputManager.horizontalInput != 0 || inputManager.verticalInput != 0) && !crouchManager.crouching && !crouchManager.lowCrouching) 
        { 
            StartSlide();
        }
        if(Input.GetKeyUp(inputManager.SlideKey) && sliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if(sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        sliding = true;

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * inputManager.verticalInput + orientation.right * inputManager.horizontalInput;
        crouchManager.LowCrouchEvent();

        //Normal sliding
        if(!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        //Slope sliding
        else
        {
            rb.AddForce(pm.GetSlopeMoveDir(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0 && !crouchManager.StanceCheck(crouchManager.capsuleStandHeight))
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        sliding = false;

        crouchManager.playerStance = PlayerCrouchManager.PlayerStance.Stand;
    }
}
