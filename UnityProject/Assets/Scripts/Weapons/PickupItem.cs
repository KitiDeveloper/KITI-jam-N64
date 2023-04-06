using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Gun weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("I am in contact");
            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateWeaponForPlayer(other);
            }
        }
    }

    private void CreateWeaponForPlayer(Collider other)
    {
        WeaponHolder weaponHolder = other.GetComponentInParent<WeaponHolder>();
        //weaponHolder.EquipWeapon(weaponPrefab);
        Destroy(gameObject, 1f);
    }
}