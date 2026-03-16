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

    [SerializeField]    
    State state;
    public void Start()
    {

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
                    if(item != null)
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
}
