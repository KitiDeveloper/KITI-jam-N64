using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //ControlMenu Sliders
    public Slider sensitivitySlider;

    //Graphics Sliders
    public Slider fOVSlider;
    public Slider bloomSlider;
    public Slider brightnessSlider;

    //Controller values
    public float sensitivityValue;

    //Graphics values
    public float fOVValue;
    public float bloomValue;
    public float brightnessValue;

    private void Start()
    {
        sensitivityValue = PlayerPrefs.GetFloat("sensitivityValue");
        sensitivitySlider.value = sensitivityValue;

        fOVValue = PlayerPrefs.GetFloat("fOVValue");
        fOVSlider.value = fOVValue;

        bloomValue = PlayerPrefs.GetFloat("bloomValue");
        bloomSlider.value = bloomValue;

        brightnessValue = PlayerPrefs.GetFloat("brightnessValue");
        brightnessSlider.value = brightnessValue;
    }

    private void Update()
    {
        sensitivityValue = sensitivitySlider.value;
        PlayerPrefs.SetFloat("sensitivityValue", sensitivityValue);

        fOVValue = fOVSlider.value;
        PlayerPrefs.SetFloat("fOVValue", fOVValue);

        bloomValue = bloomSlider.value;
        PlayerPrefs.SetFloat("bloomValue", bloomValue);

        brightnessValue = brightnessSlider.value;
        PlayerPrefs.SetFloat("brightnessValue", brightnessValue);
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(1);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
