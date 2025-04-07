using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    public float delay;
    public GameObject block;

    private void Start()
    {
        StartCoroutine(Delay());
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(block, transform);
    }
}
