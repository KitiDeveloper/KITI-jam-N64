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
    public float maxSlideTime = 1;
    public float slideForce = 200;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    private bool sliding;

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

        crouchManager.playerStance = PlayerCrouchManager.PlayerStance.LowCrouch;

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * inputManager.verticalInput + orientation.right * inputManager.horizontalInput;

        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        if(slideTimer <= 0)
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
