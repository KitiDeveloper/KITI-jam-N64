using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private Rigidbody _aiRigidbody;

    //State
    [SerializeField] public VisionState m_VisionState;
    private Vector3 _lkpPosition = Vector3.zero;

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
        UpdateState();
        UpdateColor();
    }
    private void UpdateState()
    {
        if (_aiVision.PlayerInLOS())
        {
            if (AIWeaponHolder.HasRange(m_VisionState))
            {
                AIWeaponHolder.Shoot();
                _aiRigidbody.velocity= Vector3.zero;
                m_VisionState = VisionState.Attack;
                AiMovement.UpdateMovementTarget();
                _aiVision.LookAtTarget(_player);
                _isWaiting = false;
            }
            else
            {
                m_VisionState = VisionState.Chase;
                AiMovement.UpdateMovementTarget(_player.transform.position);
                _aiVision.LookAtTarget(_player);
                _isWaiting = false;
            }

        }
        else
        {
            if (m_VisionState == VisionState.Chase || m_VisionState == VisionState.Attack)
            {
                m_VisionState = VisionState.LKP;
                _lkpPosition = _player.transform.position;
                AiMovement.UpdateMovementTarget(_lkpPosition);
                _aiVision.LookAtTarget(_lkpPosition);

            }
            else if (m_VisionState == VisionState.PlayerUnseen)
            {
                GameObject nextTarget = AIInterestPoint.NextInterestPoint();
                AiMovement.UpdateMovementTarget(nextTarget.transform.position);
                _aiVision.LookAtTarget(nextTarget);

            }
        }
    }

    public void TargetReached()
    {
        if (m_VisionState == VisionState.PlayerUnseen)
        {
            if (!_isWaiting)
            {
                _isWaiting = true;
                _currentWaitingTime = TimeToWaitOnPOI;
            }
            else
            {
                _currentWaitingTime -= Time.deltaTime;
                if (_currentWaitingTime <= 0)
                {
                    _isWaiting = false;
                    AIInterestPoint.TargetReached();
                }
            }
            
        }
        else if (m_VisionState == VisionState.LKP)
        {
            if (!_isWaiting)
            {
                _isWaiting = true;
                _currentWaitingTime = TimeToWaitOnLkP;
            }
            else
            {
                _currentWaitingTime -= Time.deltaTime;
                if(_currentWaitingTime <= 0)
                {
                    _isWaiting = false;
                    m_VisionState = VisionState.PlayerUnseen;
                }
            }
        }
    }

    private void UpdateColor()
    {
        if (m_VisionState == VisionState.PlayerUnseen)
        {
            AIRenderer.material.color = Color.white;
        }
        else if (m_VisionState == VisionState.Chase)
        {
            AIRenderer.material.color = Color.yellow;
        }
        else if (m_VisionState == VisionState.Attack)
        {
            AIRenderer.material.color = Color.red;
        }
        else if (m_VisionState == VisionState.LKP)
        {
            AIRenderer.material.color = Color.blue;
        }
    }

}
