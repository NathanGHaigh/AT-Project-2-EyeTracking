using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerWinCondition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Trigger win condition
            SceneManager.LoadScene("WinScene");
        }
    }
}
