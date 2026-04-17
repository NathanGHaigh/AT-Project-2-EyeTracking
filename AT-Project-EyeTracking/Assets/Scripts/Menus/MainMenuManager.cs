using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    MenuSoundManager soundManager;

    public GameObject ButtonsContainer;
    public Button creditsButton;
    public Button optionsButton;
    public Button exitButton;

    public GameObject OptionsPanel;

    public GameObject CreditsPanel;

    public TMP_Dropdown graphicsDropDown;
    public Slider masterSlider, sfxSLider, musicSlider;
    public AudioMixer mainAudioMixer;

    void Start()
    {
        soundManager = FindAnyObjectByType<MenuSoundManager>();
        creditsButton.onClick.AddListener(OpenCredits);
        optionsButton.onClick.AddListener(OpenOptions);
        exitButton.onClick.AddListener(ExitGame);

        masterSlider.value = 0f; // Set to default value, adjust as needed
        sfxSLider.value = 0f;
        musicSlider.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Button Functions
    void OpenCredits()
    {
        // Load the credits scene or display the credits UI
        Debug.Log("Opening Credits...");
        CreditsPanel.SetActive(true);
        soundManager.PlaySFX(soundManager.menuSFXClick);

    }

    public void CloseCredits()
    {
        // Close the credits UI and return to the main menu
        Debug.Log("Closing Credits...");
        CreditsPanel.SetActive(false);
        soundManager.PlaySFX(soundManager.menuSFXClick);
    }

    void OpenOptions()
    {
        // Load the options scene or display the options UI
        Debug.Log("Opening Options...");
        OptionsPanel.SetActive(true);
        soundManager.PlaySFX(soundManager.menuSFXClick);
        //ButtonsContainer.SetActive(false);
    }

    public void CloseOptions()
    {
        // Close the options UI and return to the main menu
        Debug.Log("Closing Options...");
        OptionsPanel.SetActive(false);
        soundManager.PlaySFX(soundManager.menuSFXClick);
        //ButtonsContainer.SetActive(true);
    }

    public void OpenGithub()
    {
        Application.OpenURL("https://github.com");
    }

    public void OpenIchio()
    {
        Application.OpenURL("https://www.youtube.com");
    }

    void ExitGame()
    {
        // Exit the game
        Debug.Log("Exiting Game...");
        soundManager.PlaySFX(soundManager.menuSFXClick);
        Application.Quit();
    }
    #endregion

    #region Options Functions
    public void ChangeGraphicsQual()
    {
        QualitySettings.SetQualityLevel(graphicsDropDown.value);
    }

    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterAudio", masterSlider.value);
    }

    public void ChangeSFXVolume()
    {
        mainAudioMixer.SetFloat("SFXAudio", sfxSLider.value);
    }

    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicAudio", musicSlider.value);
    }

    #endregion
}
