using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Vector3 Target;
    [SerializeField] private float _speed = 10;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (Target - transform.position).normalized;
        // Déplacer la balle vers la cible
        transform.position += direction * _speed * Time.deltaTime;
    }
}
