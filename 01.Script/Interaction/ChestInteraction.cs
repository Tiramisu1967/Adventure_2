using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : BaseInteraction
{
    public GameObject puzzle;
    public GameObject dropObject;
    public int num;

    public override void Interaction()
    {
        GameObject _puzzle = Instantiate(puzzle);
        _puzzle.GetComponent<LockPicPuzzle>().chest = this;
    }

    public void Open()
    {
        Instantiate(dropObject, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
