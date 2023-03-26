using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BulletMovement : MonoBehaviour
{
    public Vector3 Target;
    private Vector3 direction;
    [SerializeField] private float _speed = 10;

    private void Start()
    {
        direction = (Target - this.transform.parent.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.GetComponent<Rigidbody>().velocity = direction * _speed;
    }
}
