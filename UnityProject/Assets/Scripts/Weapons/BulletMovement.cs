using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Vector3 direction;
    [SerializeField] private float _speed = 10;

    // Update is called once per frame
    void Update()
    {
        transform.parent.GetComponent<Rigidbody>().velocity = direction * _speed;
    }


}
