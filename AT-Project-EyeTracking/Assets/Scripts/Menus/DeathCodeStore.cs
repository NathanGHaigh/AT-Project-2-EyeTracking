using UnityEngine;

public class DeathCodeStore : MonoBehaviour
{
    [SerializeField] public int DeathID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
