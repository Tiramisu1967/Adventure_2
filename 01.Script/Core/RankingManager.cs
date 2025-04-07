using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class RankingManager : MonoBehaviour
{
    [System.Serializable]
    public struct Rank
    {
        public string name;
        public int score;
    }

    [Header("메인화면")]
    public GameObject mainCanvas;
    public GameObject rankingCanvas;
    public GameObject helpCanvas;

    [Header("help")]
    public GameObject helpPage;

    [Header("RankingCanvas")]
    public TextMeshProUGUI[] rankingName;
    public TextMeshProUGUI[] rankingScore;

    [Space(15)]
    [Header("랭킹")]
    public int playerScore;
    public TMP_InputField playerName;
    public string path;
    public List<Rank> ranks = new List<Rank>();

    [Header("클릭 사운드")]
    public AudioSource audioSource;

    #region 메인 화면
    public void Close(){
        audioSource.Play();
        mainCanvas.SetActive(true);
        rankingCanvas.SetActive(false);
        helpCanvas.SetActive(false);
    }
    public void Exit()
    {
#if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Help()
    {
        audioSource.Play();
        mainCanvas.SetActive(false);
        rankingCanvas.SetActive(false);
        helpCanvas.SetActive(true);
    }
    public void Ranking()
    {
        audioSource.Play();
        mainCanvas.SetActive(false);
        rankingCanvas.SetActive(true);
        helpCanvas.SetActive(false);
        RankVeiw();
    }
    #endregion

    #region ranking


    public void RankVeiw()
    {
        RankingInit();
        RankingLoad();

        for(int i = 0;i < 5; i++)
        {
            rankingName[i].text = ranks[i].name;
            rankingScore[i].text = $"{ranks[i].score}";
        }
    }

    public void RankSet()
    {
        audioSource.Play();
        if(playerName.text != null)
        {
            RankingInit();
            Rank rank = new Rank();
            rank.name = playerName.text;
            rank.score = playerScore;
            ranks.Add(rank);
            SaveRanking();
            SceneManager.LoadScene("MainMenu");
        }
    }


    public void RankingInit()
    {
        string exePath = Application.dataPath;
#if UNITY_EDITOR
        path = Path.Combine(exePath, "ranking.txt");
#else
        path = Path.Combine(Directory.GetParent(exePath).FullName, "ranking.txt");
#endif
    }

    public void RankingLoad()
    {
        if (!File.Exists(path)) SaveRanking();
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines) {
            string[] parts = line.Split(",");
            if (parts.Length == 2)
            {
               string name = parts[0];
                if (int.TryParse(parts[1], out int score))
                {
                    Rank _rank = new Rank();
                    _rank.name = name;
                    _rank.score = score;
                    ranks.Add( _rank );
                }
            }
        }
        ranks.OrderByDescending(s => s.score).Take(5).ToList();
    }

    public void SaveRanking()
    {
        List<string> lines = new List<string>();
        for (int i = 0; i < 5; i++)
        {
            if(ranks.Count > i) lines.Add($"{ranks[i].name},{ranks[i].score}");
            else lines.Add($"none,0");
        }
        File.WriteAllLines(path, lines);
    }
#endregion
}
