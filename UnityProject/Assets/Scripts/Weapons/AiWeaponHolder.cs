using UnityEngine;

public class AiWeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject startingWeapon;
    private Weapon currentWeapon;
    private GameObject _player;

    Vector3 handPosition;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (startingWeapon)
        {
            GameObject currentWeaponObject = Instantiate(startingWeapon);
            currentWeapon = currentWeaponObject.GetComponent<Weapon>();
            currentWeaponObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            currentWeapon = transform.GetChild(0).Find("Weapon").GetComponent<Weapon>();
        }

    }

    public void TakeWeapon(GameObject weapon)
    {
        currentWeapon = weapon.GetComponent<Weapon>();
    }

    public void Attack()
    {
        currentWeapon.Shoot(_player, this.gameObject);
        
    }

    public bool HasRange(ActionState visionState)
    {
        return currentWeapon.HasRange(_player, visionState);
    }
}
