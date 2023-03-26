using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OcclusionDetection : MonoBehaviour
{
    Transform Player;
    public LayerMask layerMask = 1;
    public AudioMixerSnapshot indoor;
    public AudioMixerSnapshot outdoor;
    private Vector3 objV;
    Vector3 asf;
    Vector3 asf1;


    private void Awake()
    {
        Player = GameObject.FindObjectOfType<AudioListener>().transform;
        objV = gameObject.transform.position;
        asf.Set(0, 0, 0);
        asf1.Set(984,654,300);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //hit.collider.CompareTag("Player")
        RaycastHit hit;

        Physics.Linecast(objV, Player.position, out hit, layerMask);

        
        if (hit.collider.CompareTag("Player"))
        {
            outdoor.TransitionTo(2f);

        }
        else
        {
            indoor.TransitionTo(2f);
        }

    }
}
