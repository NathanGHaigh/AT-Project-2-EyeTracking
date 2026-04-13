using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPreloader : MonoBehaviour
{
    AsyncOperation async;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        async = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateScene()
    {
        async.allowSceneActivation = true;
    }
}
