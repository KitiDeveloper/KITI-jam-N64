using UnityEngine;


enum VisionState
{
    PlayerUnseen,
    PlayerInVision,
    LKP,
}
public class AiVision : MonoBehaviour
{

    //Object 
    private GameObject _player;
    [SerializeField] private GameObject AiComponent;
    [SerializeField] private AiMovement AiMovement;
    [SerializeField] private AIInterestPoint AIInterestPoint;

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
    }


    private bool PlayerInFOV()
    {
        if(Vector3.Distance(_player.transform.position, AiComponent.transform.position) < FOVDistance)
        {
            Vector3 directionToPlayer = _player.transform.position - AiComponent.transform.position;

            if(Vector3.Angle(directionToPlayer, AiComponent.transform.forward) < FOVAngle/2) {
                RaycastHit hit;
                if (Physics.Raycast(AiComponent.transform.position, _player.transform.position - AiComponent.transform.position, out hit, FOVDistance))
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
        if (PlayerInFOV())
        {
            _visionState = VisionState.PlayerInVision;
            AiMovement.UpdateMovementTarget(_player.transform.position);
        }
        else
        {
            if (_visionState == VisionState.PlayerInVision)
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
}
