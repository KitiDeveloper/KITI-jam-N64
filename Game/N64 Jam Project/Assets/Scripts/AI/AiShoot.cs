using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AiShoot : MonoBehaviour
{

    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject AIMainGameObject;


    //Settings
    [SerializeField] private float _shootDistance;
    [SerializeField] private float _stopShootDistance;


    //Shoot variable
    private bool _isShooting = false;
    private float _currentCooldownShot = 0;
    private float _cooldownShot = 3f;
    [SerializeField] private float _bulletSpeed;


    private void Update()
    {
        if (_isShooting)
        {
            if(_currentCooldownShot <= 0)
            {
                Shoot();
                _currentCooldownShot = _cooldownShot;
            }

            _currentCooldownShot -= Time.deltaTime;
        }
    }
    public bool canShoot()
    {
        if (_isShooting)
        {
            return (Vector3.Distance(transform.position, _player.transform.position)) < _stopShootDistance;
        }
        else
        {
            return (Vector3.Distance(transform.position, _player.transform.position)) < _shootDistance;
        }
        
    }


    public void Attack()
    {
        _isShooting = true;
    }

    public void StopAttack()
    {
        _isShooting = false;
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(_bullet, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = (_player.transform.position - AIMainGameObject.transform.position).normalized * _bulletSpeed;
        Destroy(projectile, 5.0f);
    }
}
