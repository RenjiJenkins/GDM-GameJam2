using UnityEngine;

// Arcade-style 3D Rigidbody movement. Uses Unity's legacy Input Manager.
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerController3D : MonoBehaviour
{
    public Transform cameraTransform;
    public float moveSpeed = 6f;
    public float jumpSpeed = 8f;
    public Transform groundCheck;
    public float groundRadius = 0.22f;
    public LayerMask groundLayer;
    public float turnSpeed = 12f;

    private Rigidbody body;
    private Vector2 movement;
    private bool jumpQueued;
    private float boostUntil;
    private float boostMultiplier = 1f;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement = Vector2.ClampMagnitude(movement, 1f);
        if (Input.GetButtonDown("Jump")) jumpQueued = true;
    }

    void FixedUpdate()
    {
        Vector3 forward = cameraTransform ? cameraTransform.forward : Vector3.forward;
        Vector3 right = cameraTransform ? cameraTransform.right : Vector3.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();
        Vector3 direction = (right * movement.x + forward * movement.y).normalized;
        float speed = moveSpeed * (Time.time < boostUntil ? boostMultiplier : 1f);
        Vector3 velocity = GetVelocity();
        velocity.x = direction.x * speed;
        velocity.z = direction.z * speed;
        if (jumpQueued && IsGrounded()) velocity.y = jumpSpeed;
        SetVelocity(velocity);
        jumpQueued = false;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            body.MoveRotation(Quaternion.Slerp(body.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }
    }

    bool IsGrounded() => groundCheck && Physics.CheckSphere(
        groundCheck.position, groundRadius, groundLayer, QueryTriggerInteraction.Ignore);

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        var abilities = GetComponent<PlayerAbilities>();
        if (abilities && !abilities.CanUsePowers) return;
        boostMultiplier = Mathf.Max(1f, multiplier);
        boostUntil = Time.time + Mathf.Max(0f, duration);
    }

    public void ResetMotion() => SetVelocity(Vector3.zero);
    Vector3 GetVelocity()
    {
#if UNITY_6000_0_OR_NEWER
        return body.linearVelocity;
#else
        return body.velocity;
#endif
    }
    void SetVelocity(Vector3 v)
    {
#if UNITY_6000_0_OR_NEWER
        body.linearVelocity = v;
#else
        body.velocity = v;
#endif
    }
}
