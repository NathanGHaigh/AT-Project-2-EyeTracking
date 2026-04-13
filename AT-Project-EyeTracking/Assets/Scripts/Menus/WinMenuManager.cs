using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenuManager : MonoBehaviour
{
    public Button backButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backButton.onClick.AddListener(BackToMainMenu);
    }

    void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
