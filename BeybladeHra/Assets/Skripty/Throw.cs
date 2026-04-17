using UnityEngine;

public class Throw : StateMachineBehaviour
{
    private float startY;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // uložíme si počáteční rotaci
        startY = animator.transform.eulerAngles.y;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float t = stateInfo.normalizedTime % 1f; // jistota pro looping

        float angle;

        if (t < 0.5f)
        {
            // 0 → 90
            angle = Mathf.Lerp(0f, 90f, t * 2f);
        }
        else
        {
            // 90 → 0
            angle = Mathf.Lerp(90f, 0f, (t - 0.5f) * 2f);
        }

        Vector3 rot = animator.transform.eulerAngles;
        rot.y = startY + angle;
        animator.transform.eulerAngles = rot;
    }
}