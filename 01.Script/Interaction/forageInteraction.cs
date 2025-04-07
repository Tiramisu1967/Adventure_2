using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forageInteraction : BaseInteraction
{
    public BaseItem item;

    public override void Interaction()
    {
        if (Inventroy.instance.GetItem(item)) Destroy(gameObject);
        else GameInstance.instance._gameManager.ui.Alert("∞°πÊ¿Ã ∞°µÊ √°Ω¿¥œ¥Ÿ.");
    }
}
