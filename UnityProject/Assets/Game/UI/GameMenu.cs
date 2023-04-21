using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameMenu : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;

    public bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject gamePlayHUD;

    // Reference to the global volume component
    public Volume globalVolume;

    // Reference to the bloom component inside the global volume
    public Bloom bloom;

    // Reference to the slider UI element
    public UnityEngine.UI.Slider bloomSlider;
    public UnityEngine.UI.Slider brightnessSlider;

    private ColorAdjustments colorAdjustments;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Bloom component from the global volume
        globalVolume.profile.TryGet(out bloom);

        // Set the initial value of the slider to the current value of the bloom intensity
        bloomSlider.value = bloom.intensity.value;

        globalVolume.profile.TryGet(out colorAdjustments);
        brightnessSlider.value = colorAdjustments.postExposure.value;
    }

    private void FixedUpdate()
    {
        OnSliderValueChanged();
    }

    // Update is called once per frame
    void Update()
    {
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
