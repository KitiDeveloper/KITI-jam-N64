using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Vector3 direction;
    [SerializeField] public float _speed = 10;
    [SerializeField] public WeaponBrain.Owner owner;

    // Update is called once per frame
    void Update()
    {
        transform.parent.GetComponent<Rigidbody>().velocity = direction * _speed;
    }


}
