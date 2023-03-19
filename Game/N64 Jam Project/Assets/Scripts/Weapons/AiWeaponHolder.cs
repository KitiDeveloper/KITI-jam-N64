using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class AiWeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject startingWeapon;
    private Weapon currentWeapon;
    private GameObject _player;

    Vector3 handPosition;

    public bool IsShooting = true;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (startingWeapon)
        {
            GameObject currentWeaponObject = Instantiate(startingWeapon);
            currentWeapon = currentWeaponObject.GetComponent<Weapon>();
            currentWeaponObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            currentWeapon = transform.GetChild(0).Find("Weapon").GetComponent<Weapon>();
            Debug.Log(currentWeapon);
        }

    }
    z
    private void Update()
    {
        Shoot();
    }

    public void TakeWeapon(GameObject weapon)
    {
        currentWeapon = weapon.GetComponent<Weapon>();
    }

    private void Shoot()
    {
        if (IsShooting)
        {
            currentWeapon.Shoot(_player);
        }
        
    }
}
