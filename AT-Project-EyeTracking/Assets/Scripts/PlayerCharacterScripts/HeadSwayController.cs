using UnityEngine;

public class HeadSwayController : MonoBehaviour
{
    public Transform player;

    public PlayerController playerController;

    public float swayAmount = 0.05f;

    private float swaySpeed = 5f;

    private float smoothSwayAmount = 6f;

    private Vector3 initialPosition;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }

        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.IsMoving)
        {
            SwaySideToSide();
        }
        else
        {
            Recentre();
        }

    }

    void LateUpdate()
    {
        SwaySideToSide();
    }

    void SwaySideToSide()
    {
        timer += Time.deltaTime * swaySpeed;
        float swayX = Mathf.Sin(timer) * swayAmount;
        Vector3 target = new Vector3(initialPosition.x + swayX, initialPosition.y, initialPosition.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * smoothSwayAmount);
    }

    void Recentre()
    {
        timer = 0f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * smoothSwayAmount);
    }
}
