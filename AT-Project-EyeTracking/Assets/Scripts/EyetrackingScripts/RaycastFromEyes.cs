using Eyeware.BeamEyeTracker;
using Eyeware.BeamEyeTracker.Unity;
using UnityEngine;

public class RaycastFromEyes : BeamEyeTrackerMonoBehaviour
{
    [SerializeField] BeamEyeTrackerInputDevice eyeTrackerInputDevice;

    [SerializeField] BlinkController blinkController;

    [SerializeField] Camera mainCamera;

    [SerializeField] Vector3 currentGazePos;

    [SerializeField] float maxRayDistance = 100f;

    [SerializeField] LayerMask hitMask = ~0;

    public GameObject currentViewedObject;

    Vector3 viewportPos;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        eyeTrackerInputDevice = betInputDevice;
    }
    void Start()
    {
        if (eyeTrackerInputDevice == null)
        {
            Debug.LogError("No BeamEyeTrackerMonoBehaviour found in the scene. Please assign one to the RaycastFromEyes script.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (eyeTrackerInputDevice == null || mainCamera == null)
            return;

        currentGazePos = eyeTrackerInputDevice.viewportGazePosition.ReadValue();

        viewportPos = currentGazePos;
        viewportPos.x = Mathf.Clamp01(viewportPos.x);
        viewportPos.y = Mathf.Clamp01(viewportPos.y);

        Ray ray = mainCamera.ViewportPointToRay(viewportPos);

        //if (Physics.Raycast(ray, out RaycastHit hitInfo, maxRayDistance, hitMask))
        //{
        //    Debug.Log("Hit object: " + hitInfo.collider.tag);
        //    currentViewedObject = hitInfo.collider.gameObject.transform.parent.gameObject;
        //}

        if (!blinkController.isBlinking)
        {
            if (Physics.SphereCast(ray, 1.5f, out RaycastHit hitInfo, maxRayDistance, hitMask))
            {
                if (Physics.Raycast(ray, out RaycastHit closeObject, 3f, hitMask))
                {
                    currentViewedObject = closeObject.collider.gameObject;
                }
                else
                {
                    Debug.Log("Hit object: " + hitInfo.collider.tag);
                    currentViewedObject = hitInfo.collider.gameObject;
                }
            }
            else
            {
                currentViewedObject = null;
            }
        }

    }

    void OnDrawGizmos()
    {
        if (mainCamera == null)
            return;
        Gizmos.color = Color.red;
        Ray ray = mainCamera.ViewportPointToRay(viewportPos);
        Gizmos.DrawLine(ray.origin, ray.GetPoint(maxRayDistance));
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 1f);
        for (float i = 0; i < maxRayDistance; i += 0.5f)
        {
            Gizmos.DrawWireSphere(ray.GetPoint(i), 1.5f);
        }


    }
}
