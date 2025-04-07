using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBlock : MonoBehaviour
{
    public float falseObject;
    public float trueObject;
    public Animator animator;
    public GameObject activeBox;

    private void Start()
    {
        StartCoroutine(TrueDelay());
    }

    public IEnumerator TrueDelay()
    {
        activeBox.SetActive(true);
        yield return new WaitForSeconds(trueObject - 2f);
        animator.SetBool("_isPlaying", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("_isPlaying", false);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FalseDelay());

    }
    public IEnumerator FalseDelay()
    {
        activeBox.SetActive(false);
        yield return new WaitForSeconds(falseObject);
        StartCoroutine(TrueDelay());
    }
}
