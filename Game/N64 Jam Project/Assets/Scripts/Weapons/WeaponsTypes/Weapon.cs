using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject _bullet;
    public int _magazineSize;
    public int _currentBullets;
    public float _reloadDuration;
    public float _reloadTimeLeft;
    public float _attackSpeed;  //Number of attacks per second
    public float _timeBeforeNextShoot;
    public bool _reloading;
    public float _attackDistance;
    public float _attackCancelDistance;
    public  void Shoot(GameObject target)
    {
        if (CanShoot())
        {
            _timeBeforeNextShoot = 1 / (_attackSpeed);
            GameObject tempBullet = Instantiate(_bullet);
            tempBullet.transform.position = this.transform.position;
            tempBullet.transform.Find("BulletMovement").GetComponent<BulletMovement>().Target = target.transform.position;
            Destroy(tempBullet, 5f);
            _currentBullets--;
        }
        else
        {
            if(_currentBullets <= 0 && !_reloading)
            {
                _reloading = true;
                _reloadTimeLeft = _reloadDuration;
            }else if(_reloading){
                _reloadTimeLeft -= Time.deltaTime;
                if(_reloadTimeLeft < 0)
                {
                    _currentBullets = _magazineSize;
                    _reloading = false;
                }
            }
            else
            {
                _timeBeforeNextShoot -= Time.deltaTime;
            }
        }
    }
    public bool CanShoot()
    {
        if (_timeBeforeNextShoot <= 0 && _currentBullets > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasRange(GameObject target, VisionState visionState)
    {
        if(visionState == VisionState.Attack)
        {
            if (Vector3.Distance(target.transform.position, this.transform.position) < _attackCancelDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (Vector3.Distance(target.transform.position, this.transform.position) < _attackDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
