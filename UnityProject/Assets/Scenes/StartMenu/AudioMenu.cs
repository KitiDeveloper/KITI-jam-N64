using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void AmbienceVolume(float ambienceVolume)
    {
        audioMixer.SetFloat("ExposedAmbience", ambienceVolume);
    }

    public void MasterVolume (float masterVolume)
    {
        audioMixer.SetFloat("ExposedMaster", masterVolume);
    }

    public void MusicVolume (float musicVolume)
    {
        audioMixer.SetFloat("ExposedMusic", musicVolume);
    }

    public void FoleyVolume (float foleyVolume)
    {
        audioMixer.SetFloat("ExposedFoley", foleyVolume);
    }

    public void VoiceVolume (float voiceVolume)
    {
        audioMixer.SetFloat("ExposedVoice", voiceVolume);
    }

    public void SFXVolume (float sFXVolume)
    {
        audioMixer.SetFloat("ExposedSFX", sFXVolume);
    }
}
