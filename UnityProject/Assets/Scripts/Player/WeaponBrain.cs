using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WeaponBrain : MonoBehaviour
{

    //Audio Variables
    public AudioSource WeaponPickUpSource;
    public AudioClip[] WeaponPickUpSounds;
    private int CurrentWeaponPickUpNumber;
    [SerializeField] private Weapon _weapon;
    //public AudioMixerGroup WeaponMixerGroup;


    public enum Owner
    {
        Player,
        AI,
        None,
    }

    [SerializeField] private Owner _owner  = Owner.None;
    [SerializeField] private GameObject _pickObject = null;
    [SerializeField] private PickComponent _pickComponent;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Rigidbody _rb;



    private void Start()
    {
        /*WeaponPickUpSource = gameObject.AddComponent<AudioSource>();
        WeaponPickUpSource.outputAudioMixerGroup = WeaponMixerGroup;*/
        ShuffleWeaponPickUpSounds();
        CurrentWeaponPickUpNumber = 0;
    }

    private void Update()
    {
        if(_weapon._currentBullets <= 0 && _owner == Owner.None)
        {
            Destroy(gameObject);
        }
    }

    public Owner GetOwner()
    {
        return _owner;
    }


    public void Pick(Owner owner)
    {
        _owner = owner;
        _pickObject.SetActive(false);
        _boxCollider.enabled = false;
        _rb.isKinematic = true;

        WeaponPickUpSource.clip = WeaponPickUpSounds[CurrentWeaponPickUpNumber];
        WeaponPickUpSource.PlayOneShot(WeaponPickUpSource.clip);
        CurrentWeaponPickUpNumber = CurrentWeaponPickUpNumber + 1;
        if (CurrentWeaponPickUpNumber == WeaponPickUpSounds.Length)
        {
            ShuffleWeaponPickUpSounds();
            CurrentWeaponPickUpNumber = 0;
        }

    }

    public void Drop()
    {
        if(_owner == Owner.AI)
        {
            _weapon._currentBullets = _weapon._magazineSize;
        }
        _owner = Owner.None;
        _pickObject.SetActive(true);
        _boxCollider.enabled = true;
        _rb.isKinematic = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_owner == Owner.None && other.CompareTag("Map"))
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _rb.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    private void ShuffleWeaponPickUpSounds()
    {
        for (int i = 0; i < WeaponPickUpSounds.Length; i++)
        {
            AudioClip tempSound = WeaponPickUpSounds[i];
            int n = Random.Range(i, WeaponPickUpSounds.Length);
            WeaponPickUpSounds[i] = WeaponPickUpSounds[n];
            WeaponPickUpSounds[n] = tempSound;
        }
    }


}
