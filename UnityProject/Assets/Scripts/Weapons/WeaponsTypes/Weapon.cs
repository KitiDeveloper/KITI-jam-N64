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

    [SerializeField] private WeaponBrain _brain;

    public void Update()
    {
        if (_brain.GetOwner() == WeaponBrain.Owner.Player) {
            if (_currentBullets <= 0 && !_reloading)
            {
                _reloading = true;
                _reloadTimeLeft = _reloadDuration;
            }
            else if (_reloading)
            {
                _reloadTimeLeft -= Time.deltaTime;
                if (_reloadTimeLeft < 0)
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
    public  void Shoot(Vector3 direction, Vector3 spawnPosition)
    {
        if (CanShoot())
        {
            _timeBeforeNextShoot = 1 / (_attackSpeed);
            GameObject tempBullet = Instantiate(_bullet, spawnPosition, Quaternion.identity);
            tempBullet.transform.Find("BulletMovement").GetComponent<BulletMovement>().direction = direction;
            Destroy(tempBullet, 5f);
            _currentBullets--;
        }
        else if(_brain.GetOwner() == WeaponBrain.Owner.AI)
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

    public void Shoot(GameObject target, GameObject spawnPosition)
    {
        Shoot((target.transform.position - spawnPosition.transform.position).normalized, spawnPosition.transform.position);
    }

    public void Shoot(GameObject target, Vector3 spawnPosition)
    {
        Shoot((target.transform.position - spawnPosition).normalized, spawnPosition);
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

    public bool HasRange(GameObject target, ActionState visionState)
    {
        if(visionState == ActionState.Attack)
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
