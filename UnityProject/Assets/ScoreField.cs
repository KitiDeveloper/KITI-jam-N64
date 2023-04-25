using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreField : MonoBehaviour
{
    [SerializeField] private WeaponHolder _playerWeaponHolder;
    private TextMeshProUGUI _textMeshPro;

    private int currentScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _textMeshPro.text = currentScore.ToString();

    }

    public void AddScore()
    {
        currentScore++;
    }
}
