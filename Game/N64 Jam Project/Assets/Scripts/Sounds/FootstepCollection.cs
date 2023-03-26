using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New footstep Collection", menuName = "Create a new footstep Collection")]
public class FootstepCollection : ScriptableObject
{
    public List<AudioClip> footsteps = new List<AudioClip>();
}
