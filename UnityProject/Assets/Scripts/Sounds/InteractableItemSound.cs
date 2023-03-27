using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItemSound : Interactable
{

    public AudioSource src;

    public override void OnFocus()
    {
        
    }

    public override void OnInteract()
    {
        src.PlayOneShot(src.clip);
    }

    public override void OnLoseFocus()
    {
        
    }
}
