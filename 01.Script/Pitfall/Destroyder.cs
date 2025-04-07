using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyder : MonoBehaviour
{
    public int attack;
    Rigidbody rb;

    private void Start()
    {
        rb =  GetComponent<Rigidbody>();
        StartCoroutine(DestoryDelay());
    }

    public IEnumerator DestoryDelay(){
        yield return new WaitForSeconds(5f);
        this.GetComponent<Collider>().isTrigger = true;
        Destroy(gameObject, 1f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerCharater>().Damage(attack);
            Destroy(gameObject);
        }
    }
}
