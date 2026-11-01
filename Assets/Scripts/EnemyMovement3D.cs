using UnityEngine;

// Assign one movement type per enemy prefab. X = patrol, Y = up, Z = depth.
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class EnemyMovement3D : MonoBehaviour
{
    public enum Movement { Scroller, Lagger, Ping }
    public Movement movement;
    public float speed = 2f;
    public float range = 2f;
    public float jumpInterval = 2f;
    public float jumpImpulse = 6f;
    public float lagPause = 0.35f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundRadius = 0.18f;

    private Rigidbody body;
    private Vector3 origin;
    private float nextJump;
    private float startTime;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        origin = transform.position;
        startTime = Time.time;
        nextJump = Time.time + jumpInterval;
        body.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        if (movement == Movement.Ping) { body.useGravity = false; body.isKinematic = true; }
    }

    void FixedUpdate()
    {
        float elapsed = Time.time - startTime;
        if (movement == Movement.Ping)
        {
            Vector3 offset = new Vector3(Mathf.Sin(elapsed * speed) * range,
                Mathf.Sin(elapsed * speed * 0.65f) * range * 0.5f, 0f);
            body.MovePosition(origin + offset);
            return;
        }

        float phase = Mathf.PingPong(elapsed * speed, 2f * Mathf.Max(0.1f, range));
        float direction = phase < range ? 1f : -1f;
        float moveX = direction * speed;

        if (movement == Movement.Lagger)
        {
            if (Time.time >= nextJump)
            {
                if (groundCheck && Physics.CheckSphere(groundCheck.position, groundRadius,
                    groundLayer, QueryTriggerInteraction.Ignore))
                    body.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
                nextJump = Time.time + Mathf.Max(0.2f, jumpInterval);
            }
            if (nextJump - Time.time < lagPause) moveX = 0f;
        }

        Vector3 velocity = GetVelocity();
        SetVelocity(new Vector3(moveX, velocity.y, 0f));
    }

    Vector3 GetVelocity()
    {

        return body.linearVelocity;
#endif
    }
    void SetVelocity(Vector3 velocity)
    {
#if UNITY_6000_0_OR_NEWER
        body.linearVelocity = velocity;
#else
        body.velocity = velocity;
#endif
    }
}
