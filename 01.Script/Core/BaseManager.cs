using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    protected GameManager gameManager;
    public GameManager GameManger {  get { return gameManager; } }

    public virtual void Init(GameManager _gameManager)
    {
        gameManager = _gameManager;
    }
    
    
}
