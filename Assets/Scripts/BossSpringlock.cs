using UnityEngine;
public class BossSpringlock : MonoBehaviour
{
    public Transform player;
    public float range = 6f, interval = 5f, duration = 2f;
    private float nextAttack;
    void Update()
    {
        if (!player || Time.time < nextAttack || Vector3.Distance(transform.position, player.position) > range) return;
        var abilities = player.GetComponent<PlayerAbilities>();
        if (!abilities) return;
        abilities.Springlock(duration);
        nextAttack = Time.time + Mathf.Max(duration, interval);
        // TODO: add telegraph, animation, line-of-sight checks and sound.
    }
}
