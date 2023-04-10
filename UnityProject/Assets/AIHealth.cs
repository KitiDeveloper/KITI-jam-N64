using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    private float _health;
    [SerializeField] private float _maxHealth;

    // Start is called before the first frame update
    public void Damage(float dmg)
    {
        _maxHealth -= dmg;
    }
}
