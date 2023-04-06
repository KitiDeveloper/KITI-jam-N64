using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerCrouchManager;
using static PlayerMovementManager;
using Random = UnityEngine.Random;

public class FirstPersonEngine : MonoBehaviour
{
    [Header("Activater")]
    public bool canMove;
    public bool canJump;
    public bool canCrouch;

    [Header("References")]
    public Transform orientation;
    public CapsuleCollider playerObj;
    private Rigidbody rb;
    public Transform camHolder;
    public Transform feetTransform;

    [Header("Player stats")]
    public float inAirSpeed = 0.1f;
    public float lowCrouchSpeed = 1.5f;
    public float crouchSpeed = 3.5f;
    public float walkSpeed = 4.0f;
    public float sprintSpeed = 8.0f;
    public float wallSprintSpeed = 12;
    public float groundDrag = 5.0f;
    public float jumpForce = 6.0f;
    public float jumpCooldown = 1.5f;
    public float maxSlopeAngle = 40.0f;
    public float playerHeight;

    private Gun gun;

    //---------------------------------------------------------------------
    //InputManager Variables
    //---------------------------------------------------------------------

    private float horizontalInput;
    private float verticalInput;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode CrouchKey = KeyCode.C;
    public KeyCode LowCrouchKey = KeyCode.Z;
    public KeyCode thrownWeaponKey = KeyCode.G;
    public KeyCode SlideKey = KeyCode.LeftControl;

    //---------------------------------------------------------------------
    //MovementManager Variables
    //---------------------------------------------------------------------

    public bool grounded;
    public bool exitingSlope;

    public LayerMask whatIsGround;

    private Vector3 moveDirection;
    private RaycastHit slopeHit;

    public MovementState State;

    public float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    private float momentumTrackerVolocity;
    public float momentumTrackerSmoothing;

    //---------------------------------------------------------------------
    //CrouchManager Variables
    //---------------------------------------------------------------------

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

    //---------------------------------------------------------------------
    //SlideManager Variables
    //---------------------------------------------------------------------

    [Header("Sliding")]
    public float maxSlideTime = 0.7f;
    public float slideForce = 200;
    private float slideTimer;

    public bool sliding;

    //---------------------------------------------------------------------
    //WallRunManager Variables
    //---------------------------------------------------------------------

    [Header("Wallrunning")]
    public LayerMask whatIsWall;
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

    //---------------------------------------------------------------------
    //JumpManager Variables
    //---------------------------------------------------------------------

    public bool readyToJump = true;

    //---------------------------------------------------------------------
    //SoundManager Variables
    //---------------------------------------------------------------------

    [Range(0.1f, 0.7f)]
    public float VolumeMultiplier = 0.1f;
    [Range(0.1f, 0.7f)]
    public float PitchMultiplier = 0.1f;

    public AudioClip[] StartSlidingSound;
    public AudioClip[] WallrunSprintingSound;

    private AudioSource FootstepAudioSource = default;

    private List<AudioClip> WalkFootstepSounds = new List<AudioClip>();
    private List<AudioClip> RunFootstepSounds = new List<AudioClip>();
    private List<AudioClip> SneakFootstepSounds = new List<AudioClip>();
    private AudioClip Sliding;
    private List<AudioClip> JumpStartingSound = new List<AudioClip>();
    private List<AudioClip> JumpLandingSound = new List<AudioClip>();
    private FootstepSwapper Swapper;

    //---------------------------------------------------------------------
    //TimerForSounds Variables
    //---------------------------------------------------------------------
    [Header("FootstepTimer")]
    private float time;
    public float walkFStimer;
    public float runFStimer;
    public float crouchFStimer;
    public float slideFStimer;


    void Awake()
    {
        cameraHeight = camHolder.transform.localPosition.y;
        FootstepAudioSource = gameObject.AddComponent<AudioSource>();
        Swapper = GetComponent<FootstepSwapper>();
    }

    private void Start()
    {
        wallRunTimer = maxWallRunTime;

        time = walkFStimer;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerHeight = playerObj.height;
    }

    void Update()
    {
        XZ_DirInput();

        CalculateCameraHeight();
        InputHandler();

        if (Input.GetKeyDown(SlideKey) && (verticalInput > 0) && !crouching && !lowCrouching)
        {
            StartSlide();
        }
        if (Input.GetKeyUp(SlideKey) && sliding)
        {
            StopSlide();
        }

        if (canJump)
        {
            JumpUpdate();
        }
        if(canMove) 
        {
            MovementUpdate(); 
            StateHandler();
            
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

        CheckForWall();
        WallRunEvent();

        if (Input.GetKeyDown(jumpKey) && readyToJump && isWallRunning)
        {
            readyToJump = false;

            JumpEvent();

            Invoke(nameof(ResetJumpEvent), jumpCooldown);
        }

        JumpUpdate();


        //FSTimer
        time -= Time.deltaTime;
        if (time < 0)
        {
            HandleFootSteps();
            if (State == MovementState.walking)
            {
                time = walkFStimer;
            }
            else if (State == MovementState.sprinting || State == MovementState.wallSprinting)
            {
                time = runFStimer;
            }
            else if (State == MovementState.crouching)
            {
                time = crouchFStimer;
            }

        }

    }

    private void FixedUpdate()
    {
        if (canMove) { MovePlayer(); }

        if (sliding)
        {
            SlidingMovement();
        }

        if (isWallRunning) { WallRunningMovement(); }
    }


    //-----------------------------------------------------------------------
    //InputManager
    //-----------------------------------------------------------------------

    private void XZ_DirInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    //-----------------------------------------------------------------------
    //MovementManager
    //-----------------------------------------------------------------------

    public enum MovementState
    {
        crouching,
        walking,
        sprinting,
        wallSprinting,
        air
    }

    public void StateHandler()
    {
        //When wall running/sprinting
        if (isWallRunning)
        {
            State = MovementState.wallSprinting;
            desiredMoveSpeed = wallSprintSpeed;
        }

        //When crouching
        else if (crouching)
        {
            State = MovementState.crouching;
            if (grounded) { desiredMoveSpeed = crouchSpeed; }
        }

        else if (lowCrouching)
        {
            State = MovementState.crouching;
            if (grounded) { desiredMoveSpeed = lowCrouchSpeed; }
        }

        //When sprinting
        else if (grounded && Input.GetKey(SprintKey))
        {
            State = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        //When walking
        else if (grounded)
        {
            State = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //When in the air
        else
        {
            State = MovementState.air;
        }

        MomentumTracker();
    }

    //Makes it so you mantain your momentum
    private void MomentumTracker()
    {
        lastDesiredMoveSpeed = moveSpeed;

        float difference = lastDesiredMoveSpeed - desiredMoveSpeed;

        if (difference > 1)
        {
            moveSpeed = Mathf.SmoothDamp(lastDesiredMoveSpeed, desiredMoveSpeed, ref momentumTrackerVolocity, momentumTrackerSmoothing);
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
    }

    public void MovementUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded) { rb.drag = groundDrag; } else { rb.drag = 0; }

        SpeedControl();
    }

    public void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDir(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (grounded == true) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); }
        else if (grounded == false) { rb.AddForce(moveDirection.normalized * moveSpeed * 10f * inAirSpeed, ForceMode.Force); }
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

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDir(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    //-----------------------------------------------------------------------
    //StanceManager
    //-----------------------------------------------------------------------

    public enum PlayerStance
    {
        Stand,
        Crouch,
        LowCrouch
    }

    public void InputHandler()
    {
        if (Input.GetKeyDown(CrouchKey) && canCrouch)
        {
            CrouchEvent();
        }
        else if (Input.GetKeyDown(LowCrouchKey) && canCrouch)
        {
            LowCrouchEvent();
        }
        else if (Input.GetKey(SprintKey) && !sliding)
        {
            if (StanceCheck(capsuleStandHeight)) { return; }

            playerStance = PlayerStance.Stand;
            crouching = false;
            lowCrouching = false;
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

    public void LowCrouchEvent()
    {
        if (!sliding) { lowCrouching = true; }

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

    public bool StanceCheck(float stanceCheckHeight)
    {
        stanceCheckHeight = stanceCheckHeight * 0.6f;

        Vector3 start = new Vector3(feetTransform.position.x, feetTransform.position.y + playerObj.radius + stanceCheckErrorMargin + stanceCheckHeight, feetTransform.position.z);
        Vector3 end = new Vector3(feetTransform.position.x, feetTransform.position.y - playerObj.radius - stanceCheckErrorMargin + stanceCheckHeight, feetTransform.position.z);

        return Physics.CheckCapsule(start, end, playerObj.radius * 0.985f, playerMask);
    }

    //---------------------------------------------------------------------
    //SlideManager
    //---------------------------------------------------------------------

    private void StartSlide()
    {
        sliding = true;

        slideTimer = maxSlideTime;

        int n = Random.Range(1, StartSlidingSound.Length);

        FootstepAudioSource.clip = StartSlidingSound[n];
        FootstepAudioSource.volume = Random.Range(1.0f - VolumeMultiplier, 1.0f);
        FootstepAudioSource.pitch = Random.Range(1.0f - PitchMultiplier, 1.0f);

        FootstepAudioSource.PlayOneShot(FootstepAudioSource.clip);


        Swapper.CheckSurface();

        FootstepAudioSource.clip = Sliding;
        FootstepAudioSource.volume = Random.Range(1.0f - VolumeMultiplier, 1.0f);
        FootstepAudioSource.pitch = Random.Range(1.0f - PitchMultiplier, 1.0f);

        FootstepAudioSource.PlayOneShot(FootstepAudioSource.clip);

    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        LowCrouchEvent();

        if(!sliding) { return; }

        //Normal sliding
        if (!OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        //Slope sliding
        else
        {
            rb.AddForce(GetSlopeMoveDir(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
        {
            StopSlide();
        }

        
    }

    private void StopSlide()
    {
        if(StanceCheck(capsuleStandHeight)) { playerStance = PlayerStance.LowCrouch; sliding = false; lowCrouching = true; return; }

        sliding = false;

        playerStance = PlayerStance.Stand;

        FootstepAudioSource.Stop();
    }

    //---------------------------------------------------------------------
    //WallRunManager Variables
    //---------------------------------------------------------------------

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
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGroundCheck() && readyToJump)
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

        rb.useGravity = true;

        isWallRunning = false;
    }

    //---------------------------------------------------------------------
    //JumpManager Variables
    //---------------------------------------------------------------------

    public void JumpUpdate()
    {
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded && !crouching)
        {
            readyToJump = false;

            JumpEvent();

            Invoke(nameof(ResetJumpEvent), jumpCooldown);
        }
    }

    public void JumpEvent()
    {
        exitingSlope = true;


        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJumpEvent()
    {
        readyToJump = true;

        exitingSlope = false;
    }


    public void SwapFootsteps(FootstepCollection collection)
    {
        WalkFootstepSounds.Clear();
        RunFootstepSounds.Clear();
        SneakFootstepSounds.Clear();
        JumpStartingSound.Clear();
        JumpLandingSound.Clear();

        Sliding = collection.Sliding;

        for (int i = 0; i < collection.WalkFootsteps.Count; i++)
        {
            WalkFootstepSounds.Add(collection.WalkFootsteps[i]);
        }

        for (int i = 0; i < collection.RunFootsteps.Count; i++)
        {
            RunFootstepSounds.Add(collection.RunFootsteps[i]);
        }

        for (int i = 0; i < collection.SneakFootsteps.Count; i++)
        {
            SneakFootstepSounds.Add(collection.SneakFootsteps[i]);
        }

        for (int i = 0; i < collection.JumpStart.Count; i++)
        {
            JumpStartingSound.Add(collection.JumpStart[i]);
        }

        for (int i = 0; i < collection.JumpLanding.Count; i++)
        {
            JumpLandingSound.Add(collection.JumpLanding[i]);
        }
    }


    private void HandleFootSteps()
    {
        Swapper.CheckSurface();
        if (State == MovementState.air) return;
        if (Math.Abs(rb.velocity.x + rb.velocity.z) < 1) return;
        if (sliding == true) return;
        

        if (State == MovementState.walking)
        {
            int n = Random.Range(1, WalkFootstepSounds.Count);
            FootstepAudioSource.clip = WalkFootstepSounds[n];
            FootstepAudioSource.volume = Random.Range(1.0f - VolumeMultiplier, 1.0f);
            FootstepAudioSource.pitch = Random.Range(1.0f - PitchMultiplier, 1.0f);
            FootstepAudioSource.PlayOneShot(FootstepAudioSource.clip);
            //Reset used sound not to get again
            WalkFootstepSounds[n] = WalkFootstepSounds[0];
            WalkFootstepSounds[0] = FootstepAudioSource.clip;
            Debug.Log("WalkingSuccess");
        }
        else if(State == MovementState.sprinting)
        {
            int n = Random.Range(1, RunFootstepSounds.Count);
            FootstepAudioSource.clip = RunFootstepSounds[n];
            FootstepAudioSource.volume = Random.Range(1.0f - VolumeMultiplier, 1.0f);
            FootstepAudioSource.pitch = Random.Range(1.0f - PitchMultiplier, 1.0f);
            FootstepAudioSource.PlayOneShot(FootstepAudioSource.clip);
            //Reset used sound not to get again
            RunFootstepSounds[n] = RunFootstepSounds[0];
            RunFootstepSounds[0] = FootstepAudioSource.clip;
            Debug.Log("RunningSuccess");
        }
        else if (State == MovementState.crouching)
        {
            int n = Random.Range(1, SneakFootstepSounds.Count);
            FootstepAudioSource.clip = SneakFootstepSounds[n];
            FootstepAudioSource.volume = Random.Range(1.0f - VolumeMultiplier, 1.0f);
            FootstepAudioSource.pitch = Random.Range(1.0f - PitchMultiplier, 1.0f);
            FootstepAudioSource.PlayOneShot(FootstepAudioSource.clip);
            //Reset used sound not to get again
            SneakFootstepSounds[n] = SneakFootstepSounds[0];
            SneakFootstepSounds[0] = FootstepAudioSource.clip;
            Debug.Log("SneakingSuccess");
        }
        else if (State == MovementState.wallSprinting)
        {
            int n = Random.Range(1, WallrunSprintingSound.Length);
            FootstepAudioSource.clip = WallrunSprintingSound[n];
            FootstepAudioSource.volume = Random.Range(1.0f - VolumeMultiplier, 1.0f);
            FootstepAudioSource.pitch = Random.Range(1.0f - PitchMultiplier, 1.0f);
            FootstepAudioSource.PlayOneShot(FootstepAudioSource.clip);
            //Reset used sound not to get again
            WallrunSprintingSound[n] = WallrunSprintingSound[0];
            WallrunSprintingSound[0] = FootstepAudioSource.clip;
            Debug.Log("WallRunningSuccess");
        }

    }

}
