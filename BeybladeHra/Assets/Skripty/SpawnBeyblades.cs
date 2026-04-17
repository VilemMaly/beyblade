using UnityEngine;
using System.Collections;
using System;

public class SpawnBeyblades : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject beybladePrefab;

    [Header("Target spawn point (druhý bod)")]
    public Transform spawnPoint;

    [Header("Movement settings")]
    public float moveDuration = 1.5f;
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Reference na tlacitko")]
    public Tlacitko tlacitko; // ← ten slot do UI

    private GameObject currentBeyblade;
    private Coroutine spawnRoutine;

    // Volat z jiného skriptu
    public void SpawnBeyblade()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = StartCoroutine(SpawnAndMove());
    }

    void Start()
    {
        tlacitko.OnButtonPressed += SpawnBeyblade;
    }

    private IEnumerator SpawnAndMove()
    {
        Debug.Log("Spavnuji");
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = spawnPoint ? spawnPoint.position : transform.position;
        Quaternion endRot = spawnPoint ? spawnPoint.rotation : transform.rotation;

        currentBeyblade = Instantiate(beybladePrefab, startPos, startRot);

        Rigidbody rb = currentBeyblade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // 👈 vypne fyziku
        }

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float easedT = easeCurve.Evaluate(Mathf.Clamp01(t));

            currentBeyblade.transform.position = Vector3.Lerp(startPos, endPos, easedT);
            currentBeyblade.transform.rotation = Quaternion.Slerp(startRot, endRot, easedT);

            yield return null;
        }

        currentBeyblade.transform.position = endPos;
        currentBeyblade.transform.rotation = endRot;

        spawnRoutine = null;

        if (rb != null)
        {
            rb.isKinematic = false; // 👈 zapne fyziku zpět
        }
        currentBeyblade.GetComponent<Beyblade>().begin();
    }
}