using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AiBrain : MonoBehaviour
{
    //Object 
    private GameObject _player;
    [SerializeField] private AiVision _aiVision;
    [SerializeField] private AiWeaponHolder AIWeaponHolder;
    [SerializeField] private GameObject AIMainGameObject;
    [SerializeField] private GameObject AiHead;
    [SerializeField] private AiMovement AiMovement;
    [SerializeField] private AIInterestPoint AIInterestPoint;
    [SerializeField] private Renderer AIRenderer;
    [SerializeField] private Patrol AIPatrol;
    [SerializeField] private AIHealth AIHealth;
    [SerializeField] private GameObject _offset;
    public Vector3 _oldOffsetPosition = Vector3.zero;

    private Rigidbody _aiRigidbody;

    //State
    [SerializeField] public ActionState m_ActionState;
    [SerializeField] public AlertState m_AlertState;
    //Vision
    public bool m_DirectVisionOnPlayer;
    public bool m_SoftVisionOnPlayer;
    //AlertState
    private bool _playerSeen;
    //AlertAction
    private bool m_AlerteGiven = false;
    //LKP
    private Vector3 _lkpPosition = Vector3.zero;
    private bool _lkpSet = false;

    //Settings
    [SerializeField] private float TimeToWaitOnLkP;
    [SerializeField] private float TimeToWaitOnPOI;
    [SerializeField] private bool _isWaiting = false;
    [SerializeField] private float _currentWaitingTime;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _aiRigidbody = AIMainGameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        _aiVision.SetPlayerInLOS();
        UpdateAlertState();
        UpdateColor();
        UpdateActionState();
        UpdateBehavior();
    }

    private void UpdateAlertState()
    {
        if (m_DirectVisionOnPlayer)
        {
            m_AlertState = AlertState.Engaged;
            _playerSeen = true;
            return;
        }
        if (m_AlertState == AlertState.Engaged && !m_SoftVisionOnPlayer && !(m_ActionState == ActionState.LKP || m_ActionState == ActionState.Attack ||m_ActionState == ActionState.Chase))
        {
            m_AlertState = AlertState.Alerted;
            return;
        }
    }

    private void UpdateActionState()
    {
        if (m_AlertState == AlertState.Relaxed)
        {
            m_ActionState = ActionState.POI;
            return;
        }
        if (m_AlertState == AlertState.Concerned)
        {
            m_ActionState = ActionState.Investigate;
            return;
        }
        if (m_AlertState == AlertState.Alerted)
        {
            m_ActionState = ActionState.Patrol;
            return;
        }
        if(m_AlertState == AlertState.Engaged)
        {
            if (!m_AlerteGiven)
            {
                m_ActionState = ActionState.Alert;
                return;
            }
            if(((!(m_ActionState == ActionState.Attack) && AIWeaponHolder.HasRange(ActionState.Chase)) 
                ||(m_ActionState == ActionState.Attack && AIWeaponHolder.HasRange(ActionState.Attack))) && m_SoftVisionOnPlayer)
            {
                m_ActionState = ActionState.Attack;
                return;
            }
            else if (m_SoftVisionOnPlayer)
            {
                m_ActionState = ActionState.Chase;
                return;
            }
            else
            {
                if (!_lkpSet)
                {
                    _lkpPosition = _player.transform.position;
                }
                m_ActionState = ActionState.LKP;
            }
            
        }
    }

    private void UpdateBehavior()
    {
        if(m_ActionState == ActionState.POI)
        {
            AiMovement.UpdateMovementTarget(AIInterestPoint.NextInterestPoint().transform.position);
            return;
        }
        if(m_ActionState == ActionState.Chase)
        {
            AiMovement.UpdateMovementTarget(_player.transform.position);
            _aiVision.LookAtTarget(_player);
            _oldOffsetPosition = _offset.transform.position;

            return;
        }
        if(m_ActionState == ActionState.Attack)
        {
            AiMovement.UpdateMovementTarget();
            AIWeaponHolder.Attack();
            _aiVision.LookAtTarget(_player);
            _oldOffsetPosition = _offset.transform.position;
            return;
        }
        if(m_ActionState == ActionState.LKP)
        {
            AiMovement.UpdateMovementTarget(_lkpPosition);
            return;
        }
        if(m_ActionState == ActionState.Alert)
        {
            m_AlerteGiven = true;
        }
        if(m_ActionState == ActionState.Patrol)
        {
            AiMovement.UpdateMovementTarget(AIPatrol.NextInterestPoint().transform.position);
            return;
        }
    }

    public void TargetReached(float last)
    {
        if (m_ActionState == ActionState.POI)
        {
            if(last > 1)
            {
                AIInterestPoint.TargetReached();
            }
        }
        else if (m_ActionState == ActionState.LKP)
        {
            m_ActionState = ActionState.None;
        }else if(m_ActionState == ActionState.Patrol)
        {
            if(last > 1) {
                AIPatrol.TargetReached();
            }
        }
    }

    private void UpdateColor()
    {
        if (m_ActionState == ActionState.POI)
        {
            AIRenderer.material.color = Color.white;
        }
        else if (m_ActionState == ActionState.Chase)
        {
            AIRenderer.material.color = Color.yellow;
        }
        else if (m_ActionState == ActionState.Attack)
        {
            AIRenderer.material.color = Color.red;
        }
        else if (m_ActionState == ActionState.LKP)
        {
            AIRenderer.material.color = Color.blue;
        }else if(m_ActionState == ActionState.Patrol)
        {
            AIRenderer.material.color = Color.grey;
        }
    }

    public void TakeDamage(int dmg)
    {
        AIHealth.Damage(dmg);
    }

    public void Die()
    {
        AIWeaponHolder.Drop();
        Destroy(AIMainGameObject);
    }

}
