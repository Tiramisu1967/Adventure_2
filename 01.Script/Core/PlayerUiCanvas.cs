using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiCanvas : BaseManager
{
    public Image playerHp;
    public Image[] playerOxygen;
    public Image bag;
    public Image weightDebuff;
    public Image bagDebuff;
    public Image bosster;
    public TextMeshProUGUI money;
    public TextMeshProUGUI playTime;
    public TextMeshProUGUI bagCound;
    public TextMeshProUGUI weight;
    public TextMeshProUGUI boosterText;
    public TextMeshProUGUI boosterTimeText;
    public TextMeshProUGUI interactionText;
    public GameObject startStage;
    public GameObject interactionAlert;
    public GameObject gameOverHp;
    public GameObject gameOverOxygen;
    public GameObject quickInventoryPos;
    public GameObject quickInventory;
    public GameObject alertPos;
    public GameObject alertBox;

    private void Start()
    {
        InventoryQuickSort();
        startStage.gameObject.SetActive(true);
    }

    public IEnumerator GameOver(bool _bisHp)
    {
        if(_bisHp) gameOverHp.SetActive(true);
        else gameOverOxygen.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);
        gameManager.ResetGame();
    }

    public void Update()
    {
        playerHp.fillAmount = (float)gameManager.player.currentHp / (float)gameManager.player.maxHp;

        bag.fillAmount = Inventroy.instance.currentItems.Count / Inventroy.instance.maxBag;

        float currentOxygen = gameManager.player.currentOxygen;
        for(int i = 0; i < playerOxygen.Length; i++)
        {
            playerOxygen[i].fillAmount = Mathf.Clamp(currentOxygen / gameManager.player.startOxygen, 0f, 1f);
            if(Mathf.Clamp(gameManager.player.currentOxygen / gameManager.player.startOxygen, 0f, 1f) != 0)
            {
                playerOxygen[i].gameObject.SetActive(true);
                currentOxygen -= gameManager.player.startOxygen;
            } else
            {
                playerOxygen[i].gameObject.SetActive(false);
            }
        }

        if(Inventroy.instance.currentItems.Count >= Inventroy.instance.maxBag) bagDebuff.gameObject.SetActive(true);
        else bagDebuff.gameObject.SetActive(false);

        if(Inventroy.instance.currentWeight >= Inventroy.instance.maxWeight) weightDebuff.gameObject.SetActive(true);
        else weightDebuff.gameObject.SetActive(false);

        if(gameManager.player.boosterTime > 0)
        {
            bosster.gameObject.SetActive(true);
            boosterTimeText.text = $"{gameManager.player.boosterTime:F0}";
        }
        playTime.text = $"{GameInstance.instance.playTiem}";
    }

    public void InventoryQuickSort()
    {
        for(int i = quickInventoryPos.transform.childCount; i > 0; i--)
        {
            DestroyImmediate(quickInventoryPos.transform.GetChild(i));
        }

        int temp = 0;
        while(quickInventoryPos.transform.childCount < Inventroy.instance.maxBag)
        {
            GameObject _itemIcon = Instantiate(quickInventory, quickInventoryPos.transform);
            if(Inventroy.instance.currentItems.Count > temp)
            {
                _itemIcon.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                _itemIcon.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Inventroy.instance.currentItems[temp].icon;
            } else
            {
                _itemIcon.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
            temp++;
        }
    }

    public void Alert(string _text)
    {
        GameObject _alertBox = Instantiate(alertBox, alertPos.transform);
        _alertBox.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = _text;
        Destroy(_alertBox, 2f);
    }
}
