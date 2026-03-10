using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

enum State
{
    Normal,
    Broken,
    UnPowered,
};

public class Door : MonoBehaviour
{
    [SerializeField]
    bool isOpen;
    [SerializeField]
    bool isOpening;
    [SerializeField]
    Animator doorAnim;
    [SerializeField]
    bool needsKeycard;
    [SerializeField]
    int levelAccess;
    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;
    public float textTimer;

    [SerializeField]    
    State state;
    public void Start()
    {

    }

    public void Update()
    {
        if (needsKeycard)
        {
            if (textTimer > 0)
            {
                textTimer -= Time.deltaTime;
            }
            if (textTimer <= 0)
            {
                textMeshProUGUI.SetText("");
                textTimer = 0;

            }
        }
    }

    public void Interacted(ItemKeyCard item)
    {
        switch (state)
        {
            case State.Normal:
                if (needsKeycard)
                {
                    textMeshProUGUI.SetText("Needs a KeyCard");
                    textTimer = 5;
                    if(item != null)
                        if (item.AccessLevel == levelAccess)
                        {
                            if (isOpen)
                            {
                                CloseDoor();
                            }
                            else if (!isOpen)
                            {
                                OpenDoor();
                            }
                        }
                }
                else
                {
                    if (isOpen)
                    {
                        CloseDoor();
                    }
                    else if (!isOpen)
                    {
                        OpenDoor();
                    }
                }
                break;
            case State.Broken:
                Debug.Log("Nothing Happens");
                break;
            case State.UnPowered:
                Debug.Log("Needs Power to Operate");
                break;
        }
    }
    

    public void OpenDoor()
    {
        isOpen = true;
        doorAnim.SetTrigger("Open");

    }

    public void CloseDoor()
    {
        isOpen = false;
        doorAnim.SetTrigger("Close");

    }
}
