using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
   [SerializeField] private Transform weaponContainer;

   public void EquipWeapon(Gun weaponPrefab)
   {
      var weaponInstance = Instantiate(weaponPrefab, weaponContainer.position, transform.rotation);
      weaponInstance.transform.parent = weaponContainer;
   }
}
