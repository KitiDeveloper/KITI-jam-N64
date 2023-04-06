using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject startingWeapon;
    [SerializeField] private GameObject _offset;
    [SerializeField] private GameObject _weaponCenter;
    private Weapon currentWeapon;
    
    Vector3 handPosition;

    private void Start()
    {
        if (startingWeapon)
        {
            GameObject currentWeaponObject = Instantiate(startingWeapon);
            currentWeapon = currentWeaponObject.GetComponent<Weapon>();
            currentWeaponObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            currentWeapon = transform.GetChild(0).Find("Weapon").GetComponent<Weapon>();
        }

    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    public void TakeWeapon(GameObject weapon)
    {
        currentWeapon = weapon.GetComponent<Weapon>();
    }

    public void Attack()
    {
        currentWeapon.Shoot(_offset.transform.position - _weaponCenter.transform.position, _offset);

    }

}
