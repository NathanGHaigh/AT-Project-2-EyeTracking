using UnityEngine;
using UnityEngine.UI;
using Eyeware.BeamEyeTracker;
using Eyeware.BeamEyeTracker.Unity;

namespace Interaction
{
    public class InteractableControl : BeamEyeTrackerMonoBehaviour
    {
        [SerializeField]
        Camera playerCamera;

        [SerializeField]
        RawImage interactionPrompt;

        [SerializeField] 
        BeamEyeTrackerInputDevice eyeTrackerInputDevice;

        [SerializeField]
        LayerMask interactableLayerMask;

        [SerializeField] 
        Vector3 currentGazePos;

        [SerializeField]
        Canvas interactionCanvas;

        [SerializeField]
        float maxInteractionDistance = 3f;
        
        [SerializeField]
        public IInteractable currentInteractable;

        [SerializeField]
        Vector3 viewPortPos;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            eyeTrackerInputDevice = betInputDevice;
        }
        void Start()
        {
                if (playerCamera == null)
                {
                    playerCamera = Camera.main;
                }
    
                if (interactionCanvas != null)
                {
                    interactionCanvas.worldCamera = playerCamera;
            }

        }

        // Update is called once per frame
        void Update()
        {
            UpdateCurrentInteractable();
        }

        void UpdateCurrentInteractable()
        { 
            if(eyeTrackerInputDevice == null || playerCamera == null)
            {
                return;
            }
            currentGazePos = eyeTrackerInputDevice.viewportGazePosition.ReadValue();


            viewPortPos = new Vector3(currentGazePos.x, currentGazePos.y, 0); 
            viewPortPos.x = Mathf.Clamp01(viewPortPos.x);
            viewPortPos.y = Mathf.Clamp01(viewPortPos.y);

            Ray ray = playerCamera.ViewportPointToRay(viewPortPos);

            if (Physics.SphereCast(ray, 0.5f, out RaycastHit hitInfo, maxInteractionDistance, interactableLayerMask))
            {
                Debug.Log($"Hit: {hitInfo.collider.name}");

                IInteractable interactable = hitInfo.collider?.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    currentInteractable = interactable;
                    Debug.Log($"Current interactable: {hitInfo.collider.name}");
                }
                else
                {
                    currentInteractable = null;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (playerCamera != null)
            {
                Gizmos.color = Color.green;
              
                Ray ray = playerCamera.ViewportPointToRay(viewPortPos);
                Gizmos.DrawLine(ray.origin, ray.GetPoint(maxInteractionDistance));
                Gizmos.DrawWireSphere(ray.GetPoint(maxInteractionDistance), 0.5f);
            }
        }
    }
    
}