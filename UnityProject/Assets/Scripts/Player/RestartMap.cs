using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMap : MonoBehaviour
{

    [SerializeField] private BulletMovement _bulletMovement;

    private ScoreField _scoreField;

    private void Start()
    {
        _scoreField = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreField>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _bulletMovement.owner != WeaponBrain.Owner.Player)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (other.gameObject.CompareTag("Map"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("AI") && _bulletMovement.owner != WeaponBrain.Owner.AI)
        {
            if (other.transform.Find("Brain").GetComponent<AiBrain>().TakeDamage(1))
            {
                _scoreField.AddScore();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _bulletMovement.owner != WeaponBrain.Owner.Player)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (other.gameObject.CompareTag("Map"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("AI") && _bulletMovement.owner != WeaponBrain.Owner.AI)
        {
            if (other.transform.Find("Brain").GetComponent<AiBrain>().TakeDamage(1))
            {
                _scoreField.AddScore();
            }
        }
    }
}
