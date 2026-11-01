using System.Collections.Generic;
using UnityEngine;

// Aim at surfaces in 3D with the centre-screen camera ray (crosshair).
// If the ray misses, spawn at a point in front of the camera.
public class PlatformGenerator3D : MonoBehaviour
{
    public GameObject platformPrefab;
    public Camera aimCamera;
    public float maxDistance = 5f;
    public float cooldown = 0.5f;
    public int maxActivePlatforms = 5;
    public LayerMask aimSurfaceLayer;
    public LayerMask blockingLayer;
    public Vector3 platformSize = new Vector3(2f, 0.3f, 2f);
    public float surfaceOffset = 0.25f;

    private float nextSpawn;
    private readonly Queue<GameObject> spawned = new Queue<GameObject>();

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E) || Time.time < nextSpawn || !platformPrefab || !aimCamera) return;
        var abilities = GetComponent<PlayerAbilities>();
        if (abilities && !abilities.CanUsePowers) return;

        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 target;
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, aimSurfaceLayer, QueryTriggerInteraction.Ignore))
            target = hit.point + hit.normal * surfaceOffset;
        else
            target = ray.GetPoint(maxDistance);

        // Reject remote placements and overlapping solid geometry.
        if (Vector3.Distance(transform.position, target) > maxDistance + 1f) return;
        Vector3 size = new Vector3(Mathf.Max(.1f, platformSize.x), Mathf.Max(.1f, platformSize.y), Mathf.Max(.1f, platformSize.z));
        if (Physics.CheckBox(target, size * 0.475f, Quaternion.identity, blockingLayer, QueryTriggerInteraction.Ignore)) return;

        // Clean up previously destroyed platforms to keep the cap accurate.
        while (spawned.Count > 0 && !spawned.Peek()) spawned.Dequeue();
        while (spawned.Count >= Mathf.Max(1, maxActivePlatforms))
        {
            var oldest = spawned.Dequeue();
            if (oldest) Destroy(oldest);
        }
        spawned.Enqueue(Instantiate(platformPrefab, target, Quaternion.identity));
        nextSpawn = Time.time + Mathf.Max(0.05f, cooldown);
    }
}
