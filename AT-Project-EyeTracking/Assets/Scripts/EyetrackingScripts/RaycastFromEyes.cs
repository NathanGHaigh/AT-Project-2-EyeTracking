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

    public bool hasSeenFace = false;

    [SerializeField] LayerMask LayerMask173;

    [SerializeField] LayerMask LayerMask096Face;

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
            // --- CHANGE: was hitMask, now watchEnemyMask ---
            if (Physics.SphereCast(ray, 1.5f, out RaycastHit hitInfo, maxRayDistance, LayerMask173))
            {
                Debug.Log("SphereCast Hit:" + hitInfo.collider.name + "Layer" + LayerMask.LayerToName(hitInfo.collider.gameObject.layer) + " | Tag: " + hitInfo.collider.tag);
                if (Physics.Raycast(ray, out RaycastHit closeObject, 3f, LayerMask173))
                {
                    currentViewedObject = closeObject.collider.gameObject;
                    Debug.Log("Close raycast hit: " + closeObject.collider.name);
                    
                }
                else
                {
                    currentViewedObject = hitInfo.collider.gameObject;
                    Debug.Log("Hit object: " + hitInfo.collider.tag);

                }
            }
            else
            {
                Debug.Log("SphereCast hit NOTHING");
                currentViewedObject = null;
            }

            // --- ADD: separate raycast for face-trigger enemy ---
            if (Physics.Raycast(ray, out RaycastHit faceHit, maxRayDistance, LayerMask096Face))
            {
                if (faceHit.collider.CompareTag("096Face"))
                {
                    hasSeenFace = true;
                    Debug.Log("SeenFace");                 
                }
                else
                {
                    hasSeenFace = false;
                }
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
