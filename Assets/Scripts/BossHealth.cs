using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public UnityEvent onDefeated;
    private int health;
    void Awake() => health = Mathf.Max(1, maxHealth);
    public void TakeDamage(int damage)
    {
        health -= Mathf.Max(0, damage);
        if (health > 0) return;
        onDefeated?.Invoke();
        Destroy(gameObject);
    }
    // Have your teammate call TakeDamage(1) when a thrown slab hits the boss.
}
