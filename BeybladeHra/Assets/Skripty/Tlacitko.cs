using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Tlacitko : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference buttonAction;

    [Header("První objekt")]
    public Transform object1;
    public float distance1 = 2f;
    public float duration1 = 1f;

    [Header("Druhý objekt")]
    public Transform object2;
    public float distance2 = 2f;
    public float duration2 = 1f;

    public event System.Action OnButtonPressed;

    // Stav tlačítka (externě čitelný)
    public bool IsPressed { get; private set; }

    private void OnEnable()
    {
        if (buttonAction != null)
            buttonAction.action.Enable();
    }

    private void OnDisable()
    {
        if (buttonAction != null)
            buttonAction.action.Disable();
    }

    public void StartMovement()
    {
        // kontrola, že je tlačítko právě zmáčknuté
        if (buttonAction == null || !buttonAction.action.triggered)
            return;

        IsPressed = true;

        StartCoroutine(MoveSequence());
    }

    private IEnumerator MoveSequence()
    {
        if (object1 != null)
            yield return StartCoroutine(MoveDown(object1, distance1, duration1));

        if (object2 != null)
            yield return StartCoroutine(MoveDown(object2, distance2, duration2));

        IsPressed = false;
        // 🔥 tady vyšleš impuls do světa
        OnButtonPressed?.Invoke();
    }

    private IEnumerator MoveDown(Transform obj, float distance, float duration)
    {
        Vector3 startPos = obj.position;
        Vector3 endPos = startPos + Vector3.down * distance;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            obj.position = Vector3.Lerp(startPos, endPos, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.position = endPos;
    }
}