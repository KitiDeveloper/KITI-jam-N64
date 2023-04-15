using UnityEngine;

public class AiWeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject startingWeapon;
    [SerializeField] private AiVision AIVision;
    private Weapon currentWeapon;
    private GameObject _player;
    [SerializeField] private GameObject _offset;
    [SerializeField] private GameObject _weaponCenter;

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
        if (currentWeapon)
        {
            Debug.Log(_offset.transform.position);
            currentWeapon.Shoot(_player, _offset);
        }
    }

    public bool HasRange(ActionState visionState)
    {
        if(currentWeapon.HasRange(_player, visionState))
        {
            RaycastHit[] hits1 = Physics.RaycastAll(_offset.transform.position, _player.transform.position - _offset.transform.position, AIVision.FOVDistance);
            return CheckHit(hits1);
        }
        return false;
    }

    public void Drop()
    {
        GameObject lastWeapon = transform.GetChild(0).gameObject;
        lastWeapon.transform.parent = null;
        lastWeapon.transform.rotation = Quaternion.identity;
        WeaponBrain lastWeaponBrain = lastWeapon.GetComponent<WeaponBrain>();
        lastWeaponBrain.Drop();
    }

    private bool CheckHit(RaycastHit[] hits)
    {
        System.Array.Sort(hits, (a, b) => (a.distance.CompareTo(b.distance)));
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
}
