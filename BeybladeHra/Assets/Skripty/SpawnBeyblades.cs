using UnityEngine;

public class SpawnBeyblades : MonoBehaviour
{
    public GameObject beybladePrefab;   // Prefab káči
    public Transform spawnPoint;         // Místo spawnování

    private GameObject currentBeyblade;  // Odkaz na aktuální káču

    void Update()
    {
        // Pokud ještě žádný Beyblade neexistuje, vytvoř ho
        if (currentBeyblade == null)
        {
            Vector3 pos = spawnPoint ? spawnPoint.position : transform.position;
            Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.rotation;

            currentBeyblade = Instantiate(beybladePrefab, pos, rot);
        }
    }
}
