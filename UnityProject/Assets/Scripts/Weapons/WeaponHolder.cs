using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject startingWeapon;
    [SerializeField] private GameObject _offset;
    [SerializeField] private GameObject _weaponCenter;
    [SerializeField] private Weapon currentWeapon;

    private List<GameObject> weaponAvailable = new List<GameObject>{} ;
    
    Vector3 handPosition;

    private void Start()
    {
        if (startingWeapon)
        {
            GameObject currentWeaponObject = Instantiate(startingWeapon);
            currentWeaponObject.transform.localPosition = Vector3.zero;
            currentWeapon = currentWeaponObject.GetComponent<Weapon>();
        }
        else
        {
            currentWeapon = this.transform.GetChild(0).Find("Weapon").GetComponent<Weapon>();
        }

    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.E) && weaponAvailable.Count > 0)
        {
            GameObject lastWeapon = transform.GetChild(0).gameObject;
            lastWeapon.transform.parent = null;
            lastWeapon.transform.rotation = Quaternion.identity;
            WeaponBrain lastWeaponBrain = lastWeapon.GetComponent<WeaponBrain>();
            lastWeaponBrain.Drop();
            GameObject nextWeapon = weaponAvailable[0];
            weaponAvailable[0].transform.parent = this.transform;
            nextWeapon.GetComponent<WeaponBrain>().Pick(WeaponBrain.Owner.Player);
            nextWeapon.transform.localPosition = Vector3.zero;
            nextWeapon.transform.localRotation = Quaternion.identity;
            weaponAvailable.Remove(nextWeapon);
            currentWeapon = nextWeapon.transform.Find("Weapon").GetComponent<Weapon>();
            _offset = currentWeapon.transform.parent.Find("Visual").Find("Offset").gameObject;
            _weaponCenter = currentWeapon.transform.parent.Find("Visual").Find("WeaponCenter").gameObject;
        }
    }

    public void Attack()
    {
        currentWeapon.Shoot((_offset.transform.position - _weaponCenter.transform.position).normalized, _offset);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && (other.gameObject.GetComponent<WeaponBrain>().GetOwner() == WeaponBrain.Owner.None))
        {
            weaponAvailable.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon") && (other.gameObject.GetComponent<WeaponBrain>().GetOwner() == WeaponBrain.Owner.None))
        {
            if (weaponAvailable.Contains(other.gameObject))
            {
                weaponAvailable.Remove(other.gameObject);
            }
        }
    }

}
