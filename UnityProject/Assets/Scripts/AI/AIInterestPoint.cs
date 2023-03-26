using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInterestPoint : MonoBehaviour
{

    [SerializeField] private List<GameObject> _interestPoints = new List<GameObject>();


    int _currentInterestCount = 0;

    public GameObject NextInterestPoint()
    {
        return _interestPoints[_currentInterestCount];
    }


    public void TargetReached()
    {
        List<int> tempRand = new List<int>();
        for (int i = 0; i < _interestPoints.Count; i++)
        {
            tempRand.Add(i);
        }

        tempRand.Remove(_currentInterestCount);
                
        int rd = Random.Range(0, tempRand.Count);

        _currentInterestCount = tempRand[rd];
    }
}
