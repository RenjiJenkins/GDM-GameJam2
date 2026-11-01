using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invulnerabilitySeconds = 0.8f;
    public Transform checkpoint;
    private int health;
    private float nextDamage;
    void Awake() => health = Mathf.Max(1, maxHealth);
    public void Hit(int amount)
    {
        if (Time.time < nextDamage || amount <= 0) return;
        health -= amount;
        nextDamage = Time.time + invulnerabilitySeconds;
        if (health > 0) return;
        if (checkpoint)
        {
            var body = GetComponent<Rigidbody>();
            if (body)
            {
                body.position = checkpoint.position;
                body.rotation = checkpoint.rotation;
            }
            else transform.SetPositionAndRotation(checkpoint.position, checkpoint.rotation);
            var controller = GetComponent<PlayerController3D>();
            if (controller) controller.ResetMotion();
            var abilities = GetComponent<PlayerAbilities>();
            if (abilities) abilities.ClearSpringlock();
            health = Mathf.Max(1, maxHealth);
        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
