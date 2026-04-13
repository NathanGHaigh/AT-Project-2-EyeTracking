using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    MenuSoundManager soundManager;
    [SerializeField] private string sceneName;

    private void Awake()
    {
        soundManager = FindAnyObjectByType<MenuSoundManager>();
        GetComponent<Button>().onClick.AddListener(LoadScene);

    }

    public void LoadScene()
    {
        soundManager.PlaySFX(soundManager.menuSFXClick);
        SceneManager.LoadScene(sceneName);
    }

}
