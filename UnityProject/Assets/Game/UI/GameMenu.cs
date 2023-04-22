using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    //Graphics Sliders
    public Slider fOVSlider;
    public Slider bloomSlider;
    public Slider brightnessSlider;

    //Graphics values
    public float fOVValue;
    public float bloomValue;
    public float brightnessValue;

    public KeyCode pauseKey = KeyCode.Escape;

    public bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject gamePlayHUD;

    // Reference to the global volume component
    public Volume globalVolume;

    // Reference to the bloom component inside the global volume
    public Bloom bloom;

    // Reference to the slider UI element

    private ColorAdjustments colorAdjustments;

    // Start is called before the first frame update
    void Start()
    {
        fOVValue = PlayerPrefs.GetFloat("fOVValue");
        fOVSlider.value = fOVValue;

        // Get the Bloom component from the global volume
        globalVolume.profile.TryGet(out bloom);

        // Set the initial value of the slider to the current value of the bloom intensity
        bloomSlider.value = bloom.intensity.value;

        globalVolume.profile.TryGet(out colorAdjustments);
        brightnessSlider.value = colorAdjustments.postExposure.value;

        bloomValue = PlayerPrefs.GetFloat("bloomValue");
        bloomSlider.value = bloomValue;

        brightnessValue = PlayerPrefs.GetFloat("brightnessValue");
        brightnessSlider.value = brightnessValue;
    }

    private void FixedUpdate()
    {
        OnSliderValueChanged();
    }

    // Update is called once per frame
    void Update()
    {
        fOVValue = fOVSlider.value;
        PlayerPrefs.SetFloat("fOVValue", fOVValue);

        bloomValue = bloomSlider.value;
        PlayerPrefs.SetFloat("bloomValue", bloomValue);
        bloom.intensity.value = bloomValue;

        brightnessValue = brightnessSlider.value;
        PlayerPrefs.SetFloat("brightnessValue", brightnessValue);

        if (Input.GetKeyDown(pauseKey))
        {
            if (!gameIsPaused)
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;
        gamePlayHUD.SetActive(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        gamePlayHUD.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void OnSliderValueChanged()
    {
        // Set the bloom intensity to the value of the slider
        bloom.intensity.value = bloomSlider.value;
    }

    public void UpdateBrightness(float value)
    {
        colorAdjustments.postExposure.value = value;
    }
}
