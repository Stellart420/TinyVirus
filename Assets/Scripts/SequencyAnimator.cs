using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencyAnimator : MonoBehaviour
{
    [SerializeField] float waitTimeForNextLetter = 0.15f;
    [SerializeField] float waitEnd = 0.5f;
    List<Animator> animators;


    private void OnEnable()
    {
        animators = new List<Animator>(GetComponentsInChildren<Animator>());
        StartCoroutine(DoAnimation());
    }

    IEnumerator DoAnimation()
    {
        while(true)
        {
            foreach (var animator in animators)
            {
                animator.SetTrigger("DoAnimation");
                yield return new WaitForSeconds(waitTimeForNextLetter);
            }
            yield return new WaitForSeconds(waitEnd);
        }
    }
}
