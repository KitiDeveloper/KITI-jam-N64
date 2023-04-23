using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private AiBrain _brain;

    private void Start()
    {
        _health = _maxHealth;
    }

    // Start is called before the first frame update
    public void Damage(float dmg)
    {
        _health = Math.Min(_maxHealth, _health-dmg);
    }


    private void Update()
    {
        if(_health <= 0)
        {
            _brain.Die();
        }
    }
}
