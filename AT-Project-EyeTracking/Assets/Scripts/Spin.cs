using UnityEngine;

public class Spin : MonoBehaviour
{
    public GameObject model;

    // Update is called once per frame
    void Update()
    {
        model.transform.Rotate(0, 0.5f, 0);
    }
}
