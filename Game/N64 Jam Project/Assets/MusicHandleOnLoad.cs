using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandleOnLoad : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] musicHanlder = GameObject.FindGameObjectsWithTag("MusicHandler");

        if(musicHanlder.Length > 0){
            Destroy(this.gameObject);
        }else
        {
            this.transform.tag = "MusicHandler";
        }

        DontDestroyOnLoad(this.gameObject);
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
