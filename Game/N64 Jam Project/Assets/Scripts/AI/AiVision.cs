using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;


enum VisionState
{
    PlayerUnseen,
    Chase,
    LKP,
    Attack,
}
public class AiVision : MonoBehaviour
{

    //Object 
    private GameObject _player;
    [SerializeField] private GameObject AIMainGameObject;
    [SerializeField] private AiMovement AiMovement;
    [SerializeField] private AIInterestPoint AIInterestPoint;
    [SerializeField] private AiWeaponHolder AIWeaponHolder;
    [SerializeField] private Renderer AIRenderer;

    //State
    private VisionState _visionState;
    private Vector3 _lkpPosition = Vector3.zero;
    
    //Settings
    [SerializeField] private float FOVDistance;
    [SerializeField] private float FOVAngle;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        UpdateColor();
    }


    private bool PlayerInLOS()
    {
        if(Vector3.Distance(_player.transform.position, AIMainGameObject.transform.position) < FOVDistance)
        {
            Vector3 directionToPlayer = _player.transform.position - AIMainGameObject.transform.position;

            if(Vector3.Angle(directionToPlayer, AIMainGameObject.transform.forward) < FOVAngle/2) {
                RaycastHit hit;
                if (Physics.Raycast(AIMainGameObject.transform.position, _player.transform.position - AIMainGameObject.transform.position, out hit, FOVDistance))
                {
                    if(hit.transform && hit.transform.gameObject == _player)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    private void UpdateState()
    {
        if (PlayerInLOS())
        {
            if (AIWeaponHolder.HasRange())
            {
                AIWeaponHolder.Shoot();
                _visionState = VisionState.Attack;
                AiMovement.UpdateMovementTarget(AIMainGameObject.transform.position);
            }
            else
            {
                _visionState = VisionState.Chase;
                AiMovement.UpdateMovementTarget(_player.transform.position);
            }
            
        }
        else
        {
            if (_visionState == VisionState.Chase || _visionState == VisionState.Attack)
            {
                _visionState = VisionState.LKP;
                _lkpPosition = _player.transform.position;
                AiMovement.UpdateMovementTarget(_lkpPosition);
            }
            else if(_visionState == VisionState.PlayerUnseen)
            {
                AiMovement.UpdateMovementTarget(AIInterestPoint.NextInterestPoint());
            }
        }
    }


    public void TargetReached()
    {
        if(_visionState == VisionState.PlayerUnseen)
        {
            AIInterestPoint.TargetReached();
        }else if(_visionState == VisionState.LKP)
        {
            _visionState = VisionState.PlayerUnseen;
        }
    }

    private void UpdateColor()
    {
        if(_visionState == VisionState.PlayerUnseen)
        {
            AIRenderer.material.color = Color.white;
        }else if(_visionState == VisionState.Chase)
        {
            AIRenderer.material.color = Color.yellow;
        }else if(_visionState == VisionState.Attack)
        {
            AIRenderer.material.color = Color.red;
        }else if(_visionState == VisionState.LKP)
        {
            AIRenderer.material.color = Color.blue;
        }
    }
}
