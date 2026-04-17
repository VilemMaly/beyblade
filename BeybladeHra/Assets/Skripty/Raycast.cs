using UnityEngine;

public class Raycast : MonoBehaviour
{
    public float rayDistance = 100f;
    public LayerMask layerMask = ~0; // všechno
    private kamera camScript;
    Tlacitko button;


    void Start()
    {
        camScript = GetComponent<kamera>();
    }
    void Update()
    {
        Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            HandleHit(hit.collider);
        }

    }

    void HandleHit(Collider col)
    {
        bool handled = false;

        switch (col.tag)
        {
            case "tlacitko":
                HandleButton(col);
                handled = true;
                camScript.ZoomTo(50f, 0.5f);
                break;
            case "talir":
                handled = true;
                camScript.ZoomTo(80f, 0.5f);
                break;
        }

        if (!handled)
        {
            HandleNothing(col);
        }
    }

    void HandleButton(Collider col)
    {
        button = col.GetComponent<Tlacitko>();

        if (button != null)
        {
            button.StartMovement();
        }
    }

    void HandleEnemy(Collider col)
    {
        Debug.Log("Enemy detected: " + col.name);
    }

    void HandleItem(Collider col)
    {
        Debug.Log("Item detected: " + col.name);
    }

    void HandleNothing(Collider col)
    {
        camScript.ResetZoom(0.3f);
    }
}