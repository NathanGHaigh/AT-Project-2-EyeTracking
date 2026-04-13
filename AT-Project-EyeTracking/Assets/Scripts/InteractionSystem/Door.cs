using Interaction;
using System.Collections.Generic;
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
    public int levelAccess;

    [SerializeField]
    AudioSource source;
    [SerializeField] public AudioClip dooropen;
    [SerializeField] public AudioClip doorclose;

    [SerializeField] private List<DoorKeycard> linkedButtons = new();

    [SerializeField] private List<DoorButton> linkedButtons2 = new();

    [SerializeField]
    State state;
    public void Start()
    {
        linkedButtons.AddRange(GetComponentsInChildren<DoorKeycard>());
        linkedButtons2.AddRange(GetComponentsInChildren<DoorButton>());
    }

    public void Update()
    {
    }

    public void Interacted(ItemKeyCard item)
    {
        switch (state)
        {
            case State.Normal:
                if (needsKeycard)
                {
                    if (item != null)
                        if (item.AccessLevel >= levelAccess)
                        {
                            if (isOpen)
                            {
                                source.clip = doorclose;
                                source.Play();
                                CloseDoor();
                            }
                            else if (!isOpen)
                            {
                                source.clip = dooropen;
                                source.Play();
                                OpenDoor();
                            }
                        }
                }
                else
                {
                    if (isOpen)
                    {
                        source.clip = doorclose;
                        source.Play();
                        CloseDoor();
                    }
                    else if (!isOpen)
                    {
                        source.clip = dooropen;
                        source.Play();
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

    public void Power()
    {
        if (state == State.UnPowered)
        {
            state = State.Normal;
        }
    }
    public int stateID()
    {
        if (state == State.Normal)
            return 0;
        else if (state == State.Broken)
        {
            return 1;
        }
        else if (state == State.UnPowered)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    public void UpdatePanel()
    {
        if(linkedButtons.Count > 0)
        {
            foreach (DoorKeycard button in linkedButtons)
            {
                button.UpdatePanel();
            }
        }
        if(linkedButtons2.Count > 0)
        {
            foreach (DoorButton button in linkedButtons2)
            {
                button.UpdatePanel();
            }       
        }
    }
}
