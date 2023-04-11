using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBrain : MonoBehaviour
{

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
    }

    public void Drop()
    {
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
}
