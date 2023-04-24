using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAI : MonoBehaviour
{

    [SerializeField] private GameObject ai;
    private GameObject nextAI;
    private GameObject player;

    private bool aiDestroyed;
    private float timeBeforeRespawn = 0.0f;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        nextAI = Instantiate(ai);
        nextAI.transform.position = this.transform.position;
        nextAI.SetActive(false);
    }

    private void Update()
    {
        if (aiDestroyed)
        {
            timeBeforeRespawn -= Time.deltaTime;
            if(timeBeforeRespawn <= 0.0f && Vector3.Distance(this.transform.position, player.transform.position) > 10) {
                ai = nextAI;
                ai.SetActive(true);
                nextAI = Instantiate(ai);
                nextAI.transform.position = this.transform.position;
                nextAI.SetActive(false);
                aiDestroyed = false;
            }
        }
        else
        {
            if (!ai)
            {
                aiDestroyed = true;
                timeBeforeRespawn = 10f;

            }
        }
    }


}
