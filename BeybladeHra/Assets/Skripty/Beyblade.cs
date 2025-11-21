using UnityEngine;

public class Beyblade : MonoBehaviour
{
    [Header("Movement")]
    public float torque = 1000f;
    public float sideForce = 50f;
    public float interval = 1f;

    [Header("Arena Settings")]
    public float arenaRadius = 10f;
    public float wallPushForce = 300f;

    [Header("Health Settings")]
    public int maxHealth = 1;        // nastavitelné životy
    public float hitForceBounce = 400f; // odraz při zásahu
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

        // Náhodné pohyby
        timer += Time.fixedDeltaTime;
        if (timer >= interval)
        {
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDir * sideForce, ForceMode.Impulse);
            timer = 0f;
        }

        // Kruhová bariéra
        Vector3 center = Vector3.zero;
        Vector3 flatPos = new Vector3(transform.position.x, 0f, transform.position.z);

        float dist = Vector3.Distance(flatPos, center);
        if (dist > arenaRadius)
        {
            Vector3 pushDir = (center - flatPos).normalized;
            rb.AddForce(pushDir * wallPushForce, ForceMode.Acceleration);
        }

        // Pokud spadne pod arénu → zmizí
        if (transform.position.y < 0)
        {
            Destroy(gameObject);
        }
    }


    // --- KOLIZE S JINÝM BEYBLADEM ---
    private void OnCollisionEnter(Collision collision)
    {
        Beyblade other = collision.collider.GetComponent<Beyblade>();

        if (other != null)
        {
            // Zjisti relativní rychlosti – kdo je útočník?
            float mySpeed = rb.linearVelocity.magnitude;
            float otherSpeed = other.rb.linearVelocity.magnitude;

            // Odraz obou
            Vector3 bounceDir = (transform.position - other.transform.position).normalized;
            rb.AddForce(bounceDir * hitForceBounce, ForceMode.Impulse);
            other.rb.AddForce(-bounceDir * hitForceBounce, ForceMode.Impulse);

            // Pokud JÁ jsem pomalejší → dostávám damage
            if (mySpeed < otherSpeed)
            {
                TakeDamage(1);
            }
            // Pokud já jsem rychlejší → nic se mi nestane
        }
    }


    // --- UBÍRÁNÍ ŽIVOTŮ ---
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
