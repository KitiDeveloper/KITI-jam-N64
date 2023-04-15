using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;



public class AiVision : MonoBehaviour
{

    //Object 
    private GameObject _player;
    [SerializeField] private GameObject AIMainGameObject;
    [SerializeField] private AiBrain AiBrain;

    
    
    //Settings
    [SerializeField] public float FOVDistance;
    [SerializeField] private float FOVAngle;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }



    public void SetPlayerInLOS()
    {
        if(Vector3.Distance(_player.transform.position, AIMainGameObject.transform.position) < FOVDistance)
        {
            Vector3 directionToPlayer = _player.transform.position - AIMainGameObject.transform.position;

            RaycastHit[] hits1 = Physics.RaycastAll(AIMainGameObject.transform.position, _player.transform.position - AIMainGameObject.transform.position, FOVDistance);
            RaycastHit[] hits2 = Physics.RaycastAll(AIMainGameObject.transform.position, _player.transform.Find("UpVisible").position - AIMainGameObject.transform.position, FOVDistance);
            RaycastHit[] hits3 = Physics.RaycastAll(AIMainGameObject.transform.position, _player.transform.Find("DownVisible").position - AIMainGameObject.transform.position, FOVDistance);
            if (CheckHit(hits1) || CheckHit(hits2) || CheckHit(hits3))
            {
                AiBrain.m_SoftVisionOnPlayer = true;
                if (Vector3.Angle(directionToPlayer, AIMainGameObject.transform.forward) < FOVAngle / 2)
                {
                    AiBrain.m_DirectVisionOnPlayer = true;
                }
                else
                {
                    AiBrain.m_DirectVisionOnPlayer = false;
                }
            }
            else
            {
                AiBrain.m_SoftVisionOnPlayer = false;
                AiBrain.m_DirectVisionOnPlayer = false;
            }
        }
        else
        {
            AiBrain.m_SoftVisionOnPlayer = false;
            AiBrain.m_DirectVisionOnPlayer = false;
        }
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
