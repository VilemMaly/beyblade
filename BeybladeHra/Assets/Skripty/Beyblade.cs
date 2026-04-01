using UnityEngine;

public class Beyblade : MonoBehaviour
{
    [Header("Movement")]
    public float torque = 1000f;
    public float sideForce = 50f;
    public float interval = 1f;

    [Header("Arena Settings")]
    public float arenaRadius = 0.5f; // průměr 1
    public float returnForce = 20f;   // síla návratu do středu
    public float clampForce = 50f;    // dodatečná korekce

    [Header("Grounding")]
    public float groundOffset = 0.1f;
    public LayerMask groundMask;

    [Header("Health Settings")]
    public int maxHealth = 1;
    public float hitForceBounce = 400f;

    private int currentHealth;
    private Rigidbody rb;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        // Rotace
        rb.AddTorque(Vector3.forward * torque, ForceMode.Acceleration);

        // Náhodný pohyb
        timer += Time.fixedDeltaTime;
        if (timer >= interval)
        {
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDir * sideForce, ForceMode.Impulse);
            timer = 0f;
        }

        // --- Kontrola arény ---
        Vector3 flatPos = new Vector3(transform.position.x, 0f, transform.position.z);
        float dist = flatPos.magnitude;

        if (dist > arenaRadius)
        {
            // vždy přegeneruj směr zpět do středu
            Vector3 dirToCenter = (-flatPos).normalized;

            // hlavní návratová síla
            rb.AddForce(dirToCenter * returnForce, ForceMode.Acceleration);

            // tvrdší korekce (aby se nezdržoval mimo)
            rb.AddForce(dirToCenter * clampForce, ForceMode.Impulse);

            // volitelný hard clamp pozice dovnitř
            Vector3 clamped = flatPos.normalized * arenaRadius;
            transform.position = new Vector3(clamped.x, transform.position.y, clamped.z);
        }

        // --- Udržení na zemi ---
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, groundMask))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + groundOffset;
            transform.position = pos;

            // zrušení vertikální rychlosti
            Vector3 vel = rb.linearVelocity;
            vel.y = 0f;
            rb.linearVelocity = vel;
        }

        // fallback
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Beyblade other = collision.collider.GetComponent<Beyblade>();

        if (other != null)
        {
            float mySpeed = rb.linearVelocity.magnitude;
            float otherSpeed = other.rb.linearVelocity.magnitude;

            Vector3 bounceDir = (transform.position - other.transform.position).normalized;

            rb.AddForce(bounceDir * hitForceBounce, ForceMode.Impulse);
            other.rb.AddForce(-bounceDir * hitForceBounce, ForceMode.Impulse);

            if (mySpeed < otherSpeed)
            {
                TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}