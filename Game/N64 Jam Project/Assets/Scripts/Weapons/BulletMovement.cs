using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BulletMovement : MonoBehaviour
{
    public Vector3 Target;
    [SerializeField] private float _speed = 10;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (Target - transform.position).normalized;
        transform.parent.GetComponent<Rigidbody>().velocity = (Target - this.transform.parent.position).normalized * _speed;
    }
}
