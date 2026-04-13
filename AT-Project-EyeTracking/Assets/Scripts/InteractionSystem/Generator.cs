using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private Door doorToPower;

    [SerializeField] public bool isPowered;


    void Start()
    {
        isPowered = false;
    }

    public void TurnedOn()
    {
        isPowered = true;
        doorToPower.Power();
        doorToPower.UpdatePanel();
    }
}
