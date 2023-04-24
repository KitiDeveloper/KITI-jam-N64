using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoField : MonoBehaviour
{
    [SerializeField] private WeaponHolder _playerWeaponHolder;
    private TextMeshProUGUI _textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _textMeshPro.text = _playerWeaponHolder.transform.GetChild(0).Find("Weapon").GetComponent<Weapon>()._currentBullets.ToString() + "/" + _playerWeaponHolder.transform.GetChild(0).Find("Weapon").GetComponent<Weapon>()._magazineSize.ToString();

    }
}
