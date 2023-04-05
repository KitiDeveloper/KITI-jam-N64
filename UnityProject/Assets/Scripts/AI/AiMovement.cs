using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private AiVision _vision;
    [SerializeField] private AiBrain _brain;
    private GameObject player;
    private float _lastPOIOrPartolReach = 0;
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
            _brain.TargetReached(Time.time - _lastPOIOrPartolReach);
            _lastPOIOrPartolReach = Time.time;
        }
    }

    public void UpdateMovementTarget(Vector3 target)
    {
        _agent.isStopped = false;
        _agent.SetDestination(target);
    }

    public void UpdateMovementTarget()
    {
        _agent.isStopped = true;
    }
}
