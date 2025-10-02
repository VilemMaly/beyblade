using UnityEngine;

public class SpawnBeyblades : MonoBehaviour
{
    public GameObject beybladePrefab;  // Prefab káči
    public float spawnInterval = 1f;   // čas mezi spawnem (v sekundách)
    public Transform spawnPoint;       // místo spawnování (může být prázdný object)

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            // pokud není nastaven spawnPoint, použije se pozice objektu s tímto skriptem
            Vector3 pos = spawnPoint ? spawnPoint.position : transform.position;
            Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.rotation;

            Instantiate(beybladePrefab, pos, rot);

            timer = 0f;
        }
    }
}
