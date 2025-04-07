using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerCharater player;
    public PlayerUiCanvas ui;

    public GameObject[] treasure;
    public GameObject door;

    public Canvas gameClearCanvas;
    public TextMeshProUGUI score;
    public TextMeshProUGUI playerTime;
    public TextMeshProUGUI money;

    public void Start()
    {
        if (player != null) player.Init(this);
        if (ui != null) ui.Init(this);
        GameInstance.instance._gameManager = this;

        if (GameInstance.instance._bisCurrentDoor[GameInstance.instance.currentStage])
        {
            Destroy(door);
        }

        GameInstance.instance.playTiem += Time.deltaTime;


        for (int i = 0; i < treasure.Length; i++)
        {
            treasure[i].GetComponent<ChestInteraction>().num = i;
        }


        for (int i = 0; i < GameInstance.instance._bisCurrentPuzzle.Count; i++)
        {
            if (GameInstance.instance._bisCurrentPuzzle[i].stage == GameInstance.instance.currentStage && GameInstance.instance._bisCurrentPuzzle[i]._bisOpen)
            {
                Destroy(treasure[GameInstance.instance._bisCurrentPuzzle[i].num]);
            }
        }
    }

    public void MoveScene()
    {
        if (GameInstance.instance.currentStage != 0)
        {
            GameInstance.instance.playerHp = player.currentHp;
            GameInstance.instance.playerOxygen = player.currentOxygen;
        } else
        {
            GameInstance.instance.playerHp = 0;
            GameInstance.instance.playerOxygen = 0;
            GameInstance.instance._bisDoor.Clear();
            GameInstance.instance._bisPuzzle.Clear();
            GameInstance.instance._bisDoor = new List<bool>(GameInstance.instance._bisCurrentDoor);
            GameInstance.instance._bisPuzzle = new List<GameInstance.PuzzleSystem>(GameInstance.instance._bisCurrentPuzzle);
        }
        SceneManager.LoadScene($"Stage {GameInstance.instance.currentStage}");
    }

    public void GameOver(bool _bishpOver)
    {
        Time.timeScale = 0;
        player._bisStop = true;
        if (_bishpOver) StartCoroutine(ui.GameOver(true));
        else StartCoroutine(ui.GameOver(false));
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        GameInstance.instance.playerHp = 0;
        GameInstance.instance.playerOxygen = 0;
        GameInstance.instance._bisCurrentDoor.Clear();
        GameInstance.instance._bisCurrentPuzzle.Clear();
        GameInstance.instance._bisCurrentDoor = new List<bool>(GameInstance.instance._bisDoor);
        GameInstance.instance._bisCurrentPuzzle = new List<GameInstance.PuzzleSystem>(GameInstance.instance._bisPuzzle);
    }

}
