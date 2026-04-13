using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Loadbar : MonoBehaviour
{
    [SerializeField] LinearProgressBar progressBar;
    [SerializeField] TextMeshProUGUI text;

    private AsyncOperation asyncOperation;

    public void Start()
    {
        progressBar.minimum = 0f;
        progressBar.maximum = 1f;
        text.text = "Loading... 0%";
        StartCoroutine(BeginLoad("Level"));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadScene(string sceneName)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        StartCoroutine(BeginLoad(sceneName));
    }

    private IEnumerator BeginLoad(string sceneName)
    {
        yield return new WaitForSeconds(2f);
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while(!asyncOperation.isDone)
        {
            UpdateProgressUI(asyncOperation.progress);
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        UpdateProgressUI(asyncOperation.progress);      
        asyncOperation = null;
    }

    private void UpdateProgressUI(float progress)
    {
        Debug.Log($"Progress: {progress}");
        progressBar.currentValue = (float)progress;
        text.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";
    }

}
