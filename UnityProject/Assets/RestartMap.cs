using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (other.gameObject.CompareTag("Map"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("AI"))
        {
            other.transform.Find("Brain").GetComponent<AiBrain>().TakeDamage(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (other.gameObject.CompareTag("Map"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("AI"))
        {
            other.transform.Find("Brain").GetComponent<AiBrain>().TakeDamage(1);
        }
    }
}
