using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCrouchManager : MonoBehaviour
{
    [Header("references")]
    public PlayerInputManager playerInput;
    public FirstPersonEngine fPEngine;
    public PlayerMovementManager movementManager;
    public Rigidbody rb;
    public Transform camHolder;
    public Transform feetTransform;
    public CapsuleCollider playerObj;

    public bool crouching = false;
    public bool lowCrouching = false;

    public float cameraStandHeight;
    public float cameraCrouchHeight;
    public float cameraLowCrouchHeight;

    public float capsuleStandHeight;
    public float capsuleCrouchHeight;
    public float capsuleLowCrouchHeight;

    public Vector3 standStanceCenter;
    public Vector3 crouchStanceCenter;
    public Vector3 lowCrouchStanceCenter;

    private float cameraHeight;

    public float playerStanceSmoothing;
    private float cameraHeightVolocity;

    private Vector3 stanceCapsuleCenterVelocity;
    private float stanceCapsuleHeightVelocity;

    private float stanceCheckErrorMargin = 0.05f;
    public LayerMask playerMask;

    [Header("Stance")]
    public PlayerStance playerStance;

    public enum PlayerStance
    {
        Stand,
        Crouch,
        LowCrouch
    }

    private void Awake()
    {
        cameraHeight = camHolder.transform.localPosition.y;
    }

    void Update()
    {
        CalculateCameraHeight();
        InputHandler();
    }

    public void InputHandler()
    {
        if (Input.GetKeyDown(playerInput.CrouchKey) && fPEngine.canCrouch)
        {
            CrouchEvent();
        }
        else if (Input.GetKeyDown(playerInput.LowCrouchKey) && fPEngine.canCrouch && movementManager.grounded)
        {
            LowCrouchEvent();
        }
        else if (Input.GetKey(playerInput.SprintKey) && !lowCrouching)
        {
            playerStance = PlayerStance.Stand;
            crouching = false;
        }
    }

    //Does what it says on the tin but also sets the collider height and center
    private void CalculateCameraHeight()
    {
        float cameraStanceHeight = cameraStandHeight;
        float capsuleStanceHeight = capsuleStandHeight;
        Vector3 stanceCenter = standStanceCenter;

        if (playerStance == PlayerStance.Crouch)
        {
            cameraStanceHeight = cameraCrouchHeight;
            stanceCenter = crouchStanceCenter;
            capsuleStanceHeight = capsuleCrouchHeight;
        }
        else if (playerStance == PlayerStance.LowCrouch)
        {
            cameraStanceHeight = cameraLowCrouchHeight;
            stanceCenter = lowCrouchStanceCenter;
            capsuleStanceHeight = capsuleLowCrouchHeight;
        }

        //Alters the position of the camera using the camera holder
        cameraHeight = Mathf.SmoothDamp(camHolder.localPosition.y, cameraStanceHeight, ref cameraHeightVolocity, playerStanceSmoothing);
        camHolder.localPosition = new Vector3(camHolder.localPosition.x, cameraHeight, camHolder.localPosition.z);

        //Alters the height and center of the player collider
        playerObj.height = Mathf.SmoothDamp(playerObj.height, capsuleStanceHeight, ref stanceCapsuleHeightVelocity, playerStanceSmoothing);
        playerObj.center = Vector3.SmoothDamp(playerObj.center, stanceCenter, ref stanceCapsuleCenterVelocity, playerStanceSmoothing);
    }

    public void CrouchEvent()
    {
        crouching = true;

        if (playerStance == PlayerStance.Crouch)
        {
            if (StanceCheck(capsuleStandHeight)) { return; }

            playerStance = PlayerStance.Stand;
            crouching = false;
            return;
        }
        else if (playerStance == PlayerStance.LowCrouch)
        {
            if (StanceCheck(capsuleCrouchHeight)) { return; }

            lowCrouching = false;
        }

        playerStance = PlayerStance.Crouch;
    }

    private void LowCrouchEvent()
    {
        lowCrouching = true;

        if (playerStance == PlayerStance.LowCrouch)
        {
            if (StanceCheck(capsuleCrouchHeight)) { return; }

            playerStance = PlayerStance.Crouch;
            lowCrouching = false;
            return;
        }
        else if (playerStance == PlayerStance.Crouch)
        {
            crouching = false;
        }

        playerStance = PlayerStance.LowCrouch;
    }

    private bool StanceCheck(float stanceCheckHeight)
    {
        stanceCheckHeight = stanceCheckHeight * 0.6f;

        Vector3 start = new Vector3(feetTransform.position.x, feetTransform.position.y + playerObj.radius + stanceCheckErrorMargin + stanceCheckHeight, feetTransform.position.z);
        Vector3 end = new Vector3(feetTransform.position.x, feetTransform.position.y - playerObj.radius - stanceCheckErrorMargin + stanceCheckHeight, feetTransform.position.z);

        return Physics.CheckCapsule(start, end, playerObj.radius * 0.985f, playerMask);
    }
}
