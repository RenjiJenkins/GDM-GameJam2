using UnityEngine;
[RequireComponent(typeof(Collider))]
public class EnemyContact : MonoBehaviour
{
    public float springlockSeconds = 0f;
    public float contactCooldown = 1f;
    private float nextHit;
    void OnCollisionStay(Collision collision) => Affect(collision.collider);
    void OnTriggerStay(Collider other) => Affect(other);
    private void Affect(Collider other)
    {
        if (Time.time < nextHit) return;
        var abilities = other.GetComponentInParent<PlayerAbilities>();
        if (!abilities) return;
        if (springlockSeconds > 0f) abilities.Springlock(springlockSeconds);
        var health = abilities.GetComponent<PlayerHealth>();
        if (health) health.Hit(1);
        nextHit = Time.time + Mathf.Max(0.1f, contactCooldown);
    }
}
