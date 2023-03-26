using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMG : Weapon
{
    private void Start()
    {
        _magazineSize = 50;
        _currentBullets = _magazineSize;
        _reloadDuration = 5;
        _attackSpeed = 6;
    }
}
