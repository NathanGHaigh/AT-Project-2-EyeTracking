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

    [SerializeField] float rayDist096 = 10f;

    public bool hasSeenFace = false;

    public bool watching689 = false;

    [SerializeField] LayerMask LayerMask173;

    [SerializeField] LayerMask wallMask;

    [SerializeField] LayerMask LayerMask096Face;

    [SerializeField] LayerMask LayerMask689;

    public GameObject currentViewedObject;

    Vector3 viewportPos;

    public float SpherCastScale;

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

        if (!blinkController.isBlinking)
        {
            // Raycast for Viewing SCP 173
            if (Physics.SphereCast(ray, SpherCastScale, out RaycastHit hitInfo, maxRayDistance, LayerMask173))
            {
                //Debug.Log("SphereCast Hit:" + hitInfo.collider.name + "Layer" + LayerMask.LayerToName(hitInfo.collider.gameObject.layer) + " | Tag: " + hitInfo.collider.tag);
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
                //Debug.Log("SphereCast hit NOTHING");
                currentViewedObject = null;
            }

            // Raycasr for Viewing SCP 096s face
            if (Physics.SphereCast(ray, 1.5f, out RaycastHit faceHit, rayDist096, LayerMask096Face))
            {
                if (faceHit.collider.CompareTag("096Face"))
                {
                    bool wallInWay = Physics.Raycast(
                        ray.origin, ray.direction,
                        faceHit.distance, wallMask);

                    if (!wallInWay)
                    {
                        hasSeenFace = true;
                        Debug.Log("SeenFace");
                    }
                    else
                    {
                        hasSeenFace = false;
                        Debug.Log("Wall In the way");
                    }
                }
                else
                {
                    hasSeenFace = false;
                }
            }

            //Raycast to detect that player staring at SCP 689
            if(Physics.SphereCast(ray, 1.5f, out RaycastHit watch689, maxRayDistance, LayerMask689))
            {

                if(watch689.collider.CompareTag("689"))
                {
                    watching689 = true;
                }
                else
                {
                    watching689 = false;
                }
            }
            else 
            {
                watching689 = false;
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
            Gizmos.DrawWireSphere(ray.GetPoint(i), SpherCastScale);
        }


    }
}
