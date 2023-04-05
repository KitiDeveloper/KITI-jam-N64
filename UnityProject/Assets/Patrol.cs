using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private List<GameObject> _patrolPoint = new List<GameObject>();


    int _currentPatrolPoint = 0;

    public GameObject NextInterestPoint()
    {
        return _patrolPoint[_currentPatrolPoint];
    }


    public void TargetReached()
    {
        _currentPatrolPoint = (_currentPatrolPoint+1) % _patrolPoint.Count;
    }
}
