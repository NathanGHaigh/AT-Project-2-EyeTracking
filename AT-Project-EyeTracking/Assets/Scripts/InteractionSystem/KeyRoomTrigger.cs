using UnityEngine;

public class KeyRoomTrigger : MonoBehaviour
{
    [SerializeField]
    EncounterManager encounterManager;

    private void Start()
    {
        encounterManager = FindAnyObjectByType<EncounterManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            encounterManager.inKeyRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            encounterManager.inKeyRoom = false;
        }
    }
}
