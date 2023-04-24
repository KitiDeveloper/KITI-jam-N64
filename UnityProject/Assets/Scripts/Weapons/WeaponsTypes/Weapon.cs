using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

    //Audio Variables
    [Range(0.1f, 0.7f)]
    public float VolumeMultiplier = 0.1f;
    [Range(0.1f, 0.7f)]
    public float PitchMultiplier = 0.1f;

    public AudioSource WeaponAudioSource;
    public AudioClip[] UsualShotSounds;
    public AudioClip[] LastShotSounds;
    public AudioClip[] OutOfAmmoSounds;
    private int CurrentUsusalShotNumber;
    private int CurrentLastShotNumber;
    private int CurrentOutOfAmmoNumber;
    //private bool WasLastShot;


    [SerializeField] private WeaponBrain _brain;

    public Vector3 targetRotation;
    public Vector3 currentRotation;

<<<<<<< Updated upstream
    public AudioMixer audioMixer;

    //Sounds
    [SerializeField] private List<AudioSource> _audioSources;

=======
>>>>>>> Stashed changes
    private void Start()
    {
        _camera = GameObject.FindGameObjectsWithTag("Player")[0].transform.Find("CameraHolder");

        WeaponAudioSource = gameObject.AddComponent<AudioSource>();
        ShuffleLastShotSounds();
        ShuffleOutOfAmmoSounds();
        ShuffleUsualShotSounds();
        CurrentUsusalShotNumber = 0;
        CurrentLastShotNumber = 0;
        CurrentOutOfAmmoNumber = 0;
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

            if (_currentBullets > 1)
            {
                WeaponAudioSource.clip = UsualShotSounds[CurrentUsusalShotNumber];
                WeaponAudioSource.volume = Random.Range(1.0f - VolumeMultiplier, 1.0f);
                WeaponAudioSource.pitch = Random.Range(1.0f - PitchMultiplier, 1.0f);
                WeaponAudioSource.PlayOneShot(WeaponAudioSource.clip);
                CurrentUsusalShotNumber = CurrentUsusalShotNumber + 1;
                if (CurrentUsusalShotNumber == UsualShotSounds.Length)
                {
                    ShuffleUsualShotSounds();
                    CurrentUsusalShotNumber = 0;
                }
                Debug.Log("UsualShot");
            }

            if (_currentBullets == 1)
            {
                WeaponAudioSource.clip = LastShotSounds[CurrentLastShotNumber];
                WeaponAudioSource.volume = Random.Range(1.0f - VolumeMultiplier, 1.0f);
                WeaponAudioSource.pitch = Random.Range(1.0f - PitchMultiplier, 1.0f);
                WeaponAudioSource.PlayOneShot(WeaponAudioSource.clip);
                CurrentLastShotNumber = CurrentLastShotNumber + 1;
                if (CurrentLastShotNumber == LastShotSounds.Length)
                {
                    ShuffleLastShotSounds();
                    CurrentLastShotNumber = 0;
                }
                //WasLastShot = true;
                Debug.Log("LastShot");
            }



            _currentBullets--;

            if(_brain.GetOwner() == WeaponBrain.Owner.Player)
            {
                Recoil();
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
            if (_currentBullets == 0)
            {
                WeaponAudioSource.clip = OutOfAmmoSounds[CurrentOutOfAmmoNumber];
                WeaponAudioSource.volume = Random.Range(1.0f - VolumeMultiplier, 1.0f);
                WeaponAudioSource.pitch = Random.Range(1.0f - PitchMultiplier, 1.0f);
                WeaponAudioSource.PlayDelayed(0.1f);
                CurrentOutOfAmmoNumber = CurrentOutOfAmmoNumber + 1;
                if (CurrentOutOfAmmoNumber == OutOfAmmoSounds.Length)
                {
                    ShuffleOutOfAmmoSounds();
                    CurrentOutOfAmmoNumber = 0;
                }
                //WasLastShot = false;
                Debug.Log("Out of ammo");
            }
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

    private void ShuffleUsualShotSounds()
    {
        for (int i = 0; i < UsualShotSounds.Length; i++)
        {
            AudioClip tempSound = UsualShotSounds[i];
            int n = Random.Range(i, UsualShotSounds.Length);
            UsualShotSounds[i] = UsualShotSounds[n];
            UsualShotSounds[n] = tempSound;
        }
    }

    private void ShuffleLastShotSounds()
    {
        for (int i = 0; i < LastShotSounds.Length; i++)
        {
            AudioClip tempSound = LastShotSounds[i];
            int n = Random.Range(i, LastShotSounds.Length);
            LastShotSounds[i] = LastShotSounds[n];
            LastShotSounds[n] = tempSound;
        }
    }

    private void ShuffleOutOfAmmoSounds()
    {
        for (int i = 0; i < OutOfAmmoSounds.Length; i++)
        {
            AudioClip tempSound = OutOfAmmoSounds[i];
            int n = Random.Range(i, OutOfAmmoSounds.Length);
            OutOfAmmoSounds[i] = OutOfAmmoSounds[n];
            OutOfAmmoSounds[n] = tempSound;
        }
    }
}
