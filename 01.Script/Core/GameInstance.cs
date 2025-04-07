using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    [System.Serializable]
    public struct PuzzleSystem
    {
        public int stage;
        public int num;
        public bool _bisOpen; 
    }
    public GameManager _gameManager;
    public int currentStage;
    public int money;
    public float playTiem;
    public int playerHp;
    public float playerOxygen;
    public static GameInstance instance;
    public List<bool> _bisCurrentDoor = new List<bool>();
    public List<bool> _bisDoor = new List<bool>();
    public List<PuzzleSystem> _bisCurrentPuzzle = new List<PuzzleSystem>();
    public List<PuzzleSystem> _bisPuzzle = new List<PuzzleSystem>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
}
