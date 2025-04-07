using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpwan : MonoBehaviour
{
    public GameObject arrow;
    public GameObject pos;
    public float delay;

    private void Start()
    {
        StartCoroutine(spwan());
    }

    public IEnumerator spwan()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(arrow, pos.transform.position, pos.transform.rotation);
        StartCoroutine(spwan());
    }
}
