using UnityEngine;

public class Lampa : MonoBehaviour
{
    public HingeJoint hingeJoint;

    [Header("Nastavení")]
    public float switchInterval = 2f;   // èas mezi pøepnutím (sekundy)
    public float force = 100f;          // síla motoru
    public float velocity = 100f;       // základní rychlost (kladná)

    private float timer;
    private int direction = 1;

    void Start()
    {
        if (hingeJoint == null)
            hingeJoint = GetComponent<HingeJoint>();

        var motor = hingeJoint.motor;
        motor.force = force;
        motor.targetVelocity = velocity;
        hingeJoint.motor = motor;
        hingeJoint.useMotor = true;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            timer = 0f;
            direction *= -1; // otoèí znaménko

            var motor = hingeJoint.motor;
            motor.force = force;
            motor.targetVelocity = velocity * direction;
            hingeJoint.motor = motor;
        }
    }
}