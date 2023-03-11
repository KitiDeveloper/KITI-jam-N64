using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Playables;


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

    //State
    private VisionState _visionState;
    
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
        
    }



    private void PlayerInFOV()
    {
        if(Vector3.Distance(_player.transform.position, AiComponent.transform.position) < FOVDistance)
        {
            Vector3 directionToPlayer = _player.transform.position - AiComponent.transform.position;

            if(Vector3.Angle(directionToPlayer, AiComponent.transform.forward) < FOVAngle/2) {
                Debug.Log("Player is in AI FOV");
            }
        }
    }
}
