using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{

    BlinkController blinkController;

    bool held = false;

    public Transform Player;
    public Camera mainCamera;
    public GameObject scp689Prefab;

    public float minSpawnDistance = 4f;
    public float maxSpawnDistance = 8f;

    public float dotProductThreshold = 0.3f;

    public float EncounterCooldown = 10f;
    [SerializeField]
    private bool encounterActive = false;
    [SerializeField]
    private List<SCP689SpawnPoint> allSpawnPoints = new List<SCP689SpawnPoint>();
    [SerializeField]
    private GameObject active689Instance = null;

    private void Start()
    {
        blinkController = FindAnyObjectByType<BlinkController>();

        allSpawnPoints = FindObjectsByType<SCP689SpawnPoint>(FindObjectsSortMode.None).ToList();
    }

    private void Update()
    {
        if (encounterActive) return;

        EncounterCooldown -= Time.deltaTime;

        if(EncounterCooldown < 0)
        {
            AttemptTrigger689Encounter();
        }
    }

    public void AttemptTrigger689Encounter()
    {
        SCP689SpawnPoint spawnPoint = SelectSpawnPoint();

        if (spawnPoint == null)
        {
            Debug.Log("Spawn Failed");
            EncounterCooldown = 10f;
            return;
        }
        Debug.Log("Spawn Passed " + spawnPoint.name);
        Spawn689(spawnPoint);

    }

    public SCP689SpawnPoint SelectSpawnPoint()
    {
        Vector3 playerPosition = Player.position;
        Vector3 playerForward = mainCamera.transform.forward;
        List<SCP689SpawnPoint> spawnPoints = allSpawnPoints
            .Where(p =>
            {
                float distance = Vector3.Distance(playerPosition, p.transform.position);
                return distance >= minSpawnDistance && distance <= maxSpawnDistance;

            })

            .Where(p =>
            {
                Vector3 directionToPoint = (p.transform.position - playerPosition).normalized;
                float dotProduct = Vector3.Dot(playerForward, directionToPoint);
                return dotProduct >= dotProductThreshold;

            })

            .Where(p =>
            {
                Vector3 directionToPlayer = (playerPosition - p.transform.position).normalized;
                float distanceToPlayer = Vector3.Distance(p.transform.position, playerPosition);

                if (Physics.Raycast(p.transform.position, directionToPlayer, out RaycastHit hit, distanceToPlayer))
                {
                    return hit.collider.CompareTag("Player");
                }
                return true;
            })
            .OrderByDescending(p =>
            {
                Vector3 directionToPoint = (p.transform.position - playerPosition).normalized;
                float dotProduct = Vector3.Dot(playerForward, directionToPoint);

                float distance = Vector3.Distance(playerPosition, p.transform.position);
                float normalizedDistance = 1f - Mathf.InverseLerp(minSpawnDistance, maxSpawnDistance, distance);

                return (dotProduct * -0.7f) + (normalizedDistance * -0.3f);
            })
            .ToList();
        if(spawnPoints.Count <= 0)
        {
            return null;
        }

        return spawnPoints[0];
    }

    public void Spawn689(SCP689SpawnPoint spawnPoint)
    {
        encounterActive = true;
        active689Instance = Instantiate(scp689Prefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    public void Despawn689(GameObject gameObject)
    {
        blinkController.StartBlink(held);
        Destroy(gameObject);
        encounterActive = false;
        active689Instance = null;
        EncounterCooldown = 10f;
        Debug.Log("Triggered Destory Event");
    }
}
