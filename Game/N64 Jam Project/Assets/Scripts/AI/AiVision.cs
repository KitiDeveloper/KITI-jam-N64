using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;



public class AiVision : MonoBehaviour
{

    //Object 
    private GameObject _player;
    [SerializeField] private GameObject AIMainGameObject;
    [SerializeField] private AiBrain AiBrain;

    
    
    //Settings
    [SerializeField] private float FOVDistance;
    [SerializeField] private float FOVAngle;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }



    public bool PlayerInLOS()
    {
        if(Vector3.Distance(_player.transform.position, AIMainGameObject.transform.position) < FOVDistance)
        {
            Vector3 directionToPlayer = _player.transform.position - AIMainGameObject.transform.position;

            if (Vector3.Angle(directionToPlayer, AIMainGameObject.transform.forward) < FOVAngle / 2 || AiBrain.m_VisionState == VisionState.Attack || AiBrain.m_VisionState == VisionState.Chase || AiBrain.m_VisionState == VisionState.LKP) {
                RaycastHit[] hits1 = Physics.RaycastAll(AIMainGameObject.transform.position, _player.transform.position - AIMainGameObject.transform.position, FOVDistance);
                RaycastHit[] hits2 = Physics.RaycastAll(AIMainGameObject.transform.position, _player.transform.Find("UpVisible").position - AIMainGameObject.transform.position, FOVDistance);
                RaycastHit[] hits3 = Physics.RaycastAll(AIMainGameObject.transform.position, _player.transform.Find("DownVisible").position - AIMainGameObject.transform.position, FOVDistance);
                if (CheckHit(hits1) || CheckHit(hits2) || CheckHit(hits3))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckHit(RaycastHit[] hits)
    {
        System.Array.Sort(hits,(a, b) => (a.distance.CompareTo(b.distance)));
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Player"))
            {
                return true;
            }
            if (!hits[i].transform.CompareTag("Bullet") && !hits[i].transform.CompareTag("AI"))
            {
                return false;
            }
        }
        return false;   
    }

    public void LookAtTarget(GameObject target)
    {
        transform.parent.LookAt(target.transform);
    }

    public void LookAtTarget(Vector3 position)
    {
        transform.parent.LookAt(position);
    }
}
