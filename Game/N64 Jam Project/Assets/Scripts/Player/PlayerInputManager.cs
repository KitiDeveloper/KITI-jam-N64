using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode CrouchKey = KeyCode.C;
    public KeyCode LowCrouchKey = KeyCode.Z;
    public KeyCode thrownWeaponKey = KeyCode.G;
    public KeyCode SlideKey = KeyCode.LeftControl;

    void Update()
    {
        XZ_DirInput();
    }

    private void XZ_DirInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}
