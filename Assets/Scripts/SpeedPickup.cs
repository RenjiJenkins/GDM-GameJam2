using UnityEngine;
[RequireComponent(typeof(Collider))]
public class SpeedPickup : MonoBehaviour
{
    public float multiplier = 1.5f, duration = 5f;
    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<PlayerController3D>();
        if (!player) return;
        var abilities = player.GetComponent<PlayerAbilities>();
        if (abilities && !abilities.CanUsePowers) return;
        player.ApplySpeedBoost(multiplier, duration);
        Destroy(gameObject);
    }
}
