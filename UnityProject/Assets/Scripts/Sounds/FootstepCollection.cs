using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New footstep Collection", menuName = "Create a new footstep Collection")]


public class FootstepCollection : ScriptableObject
{
    public List<AudioClip> WalkFootsteps = new List<AudioClip>();
    public List<AudioClip> RunFootsteps = new List<AudioClip>();
    public List<AudioClip> SneakFootsteps = new List<AudioClip>();
    public AudioClip Sliding;
    public List<AudioClip> JumpLanding = new List<AudioClip>();


}
