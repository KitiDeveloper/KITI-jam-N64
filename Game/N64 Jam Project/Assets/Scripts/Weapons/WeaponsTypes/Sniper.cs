using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    private void Start()
    {
        _currentBullets = _magazineSize;
    }
}
