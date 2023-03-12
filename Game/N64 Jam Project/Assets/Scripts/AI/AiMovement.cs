using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private AiVision _vision;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(_agent.remainingDistance < 0.5)
        {
            _vision.TargetReached();
        }
    }




    public void UpdateMovementTarget(Vector3 target)
    {
        _agent.SetDestination(target);
    }
}
