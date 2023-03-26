using UnityEngine;

public class Gun : MonoBehaviour
{
   [SerializeField] private GameObject bulletPrefab;
   [SerializeField] private Transform bulletPoint;
   [SerializeField] private float bulletSpeed = 800;

   public void Shoot()
   {
      GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, transform.rotation);
      bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
   }

}
