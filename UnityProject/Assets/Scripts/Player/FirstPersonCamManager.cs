using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonCamManager : MonoBehaviour
{
    public float sensX;
    public float sensY;
    //Controller values
    public float sensitivityValue;

    public Transform orientation;
    public GameMenu gameMenu;

    float xRotation;
    float yRotation;

    public float negXClamp;
    public float posXClamp;

    public Slider sensitivitySlider;

    private void Start()
    {
        sensitivityValue = PlayerPrefs.GetFloat("sensitivityValue");
        sensitivitySlider.value = sensitivityValue;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sensitivitySlider.value = sensY;
        sensitivitySlider.value = sensX;
    }

    public void FixedUpdate()
    {
        UpdateCameraSensitivity(sensitivitySlider.value);
    }

    public void UpdateCameraSensitivity(float value)
    {
        sensX = value;
        sensY = value;
    }

    private void Update()
    {
        sensitivityValue = sensitivitySlider.value;
        PlayerPrefs.SetFloat("sensitivityValue", sensitivityValue);

        if (gameMenu.gameIsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        Debug.Log("mouseY" + mouseY + ", xRotation : " + xRotation);
        xRotation = Mathf.Clamp(xRotation, negXClamp, posXClamp);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
