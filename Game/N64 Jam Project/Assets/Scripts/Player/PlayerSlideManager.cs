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

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
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
        if(Input.GetKeyDown(inputManager.SlideKey) && (inputManager.horizontalInput != 0 || inputManager.verticalInput != 0)) 
        { 
            StartSlide();
        }
        if(Input.GetKeyUp(inputManager.SlideKey) && sliding)
        {

        }
    }

    private void StartSlide()
    {
        sliding = true;
    }

    private void SlidingMovement()
    {

    }

    private void StopSlide()
    {
        
    }
}
