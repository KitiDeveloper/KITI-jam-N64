using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : Weapon
{
    private void Start()
    {
        _currentBullets = _magazineSize;
    }
}
