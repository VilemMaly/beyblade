using UnityEngine;

public class Beyblade : MonoBehaviour
{
    public float torque = 1000f;   // síla otáčení kolem Z
    public float sideForce = 50f;  // síla postrčení do strany
    public float interval = 1f;    // interval změny směru

    private Rigidbody rb;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // točení kolem Z osy (stálé)
        rb.AddTorque(Vector3.forward * torque, ForceMode.Acceleration);

        // časovač
        timer += Time.fixedDeltaTime;
        if (timer >= interval)
        {
            // vyber náhodný směr v rovině XZ
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

            // přidej postrčení
            rb.AddForce(randomDir * sideForce, ForceMode.Impulse);

            // reset časovače
            timer = 0f;
        }
    }
}
