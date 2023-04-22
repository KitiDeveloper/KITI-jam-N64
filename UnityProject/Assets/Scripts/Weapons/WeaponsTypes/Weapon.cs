using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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
    public float _recoilX;
    public float _recoilY;
    public float _recoilZ;
    public float _snapiness;
    public float _returnSpeed;
    private Transform _camera;

    [SerializeField] private WeaponBrain _brain;

    public Vector3 targetRotation;
    public Vector3 currentRotation;

    //Sounds
    [SerializeField] private List<AudioSource> _audioSources;

    private void Start()
    {
        _camera = GameObject.FindGameObjectsWithTag("Player")[0].transform.Find("CameraHolder");
    }

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

        if (_brain.GetOwner() == WeaponBrain.Owner.Player)
        {
            targetRotation = Vector3.Slerp(targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, _snapiness * Time.deltaTime);
            if (!_camera)
            {
                _camera = GameObject.FindGameObjectsWithTag("Player")[0].transform.Find("CameraHolder");
            }

            this.transform.parent.Find("Visual").localRotation = Quaternion.Euler(currentRotation);
        }
    }
    public  void Shoot(Vector3 direction, Vector3 spawnPosition)
    {
        if (CanShoot())
        {
            _timeBeforeNextShoot = 1 / (_attackSpeed);
            GameObject tempBullet = Instantiate(_bullet, spawnPosition, Quaternion.identity);
            BulletMovement bulletMovement = tempBullet.transform.Find("BulletMovement").GetComponent<BulletMovement>();
            bulletMovement.direction = direction;
            if(_brain.GetOwner() == WeaponBrain.Owner.Player)
            {
                bulletMovement._speed *= 2;
            }
            Destroy(tempBullet, 5f);
            _currentBullets--;

            if(_brain.GetOwner() == WeaponBrain.Owner.Player)
            {
                Recoil();
            }
            if(_audioSources.Count > 0)
            {
                int random = Random.Range(0, _audioSources.Count);
                _audioSources[random].Play();   
            }

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

    public void Reload()
    {
        _currentBullets = 0;
    }

    public bool CanReload()
    {
        return _currentBullets > 0;
    }


    private void Recoil()
    {
        targetRotation += new Vector3(_recoilX, Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
    }
}
