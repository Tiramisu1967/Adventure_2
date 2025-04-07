using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct ItemData
{
    public string name;
    public GameObject data;
}


public class Inventroy : MonoBehaviour
{
    public static Inventroy instance;
    public int bagLevel;
    public int oxygenLevel;
    [HideInInspector] public int maxBag;
    [HideInInspector] public int maxWeight;
    [HideInInspector] public int currentWeight;
    public List<BaseItem> items = new List<BaseItem>();
    public List<BaseItem> currentItems = new List<BaseItem>();
    public List<ItemData> _itemData = new List<ItemData>();

    public int select;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }

    public void ResetItem()
    {
        
        currentItems.Clear();
        currentItems = new List<BaseItem> (items);
        GameInstance.instance._gameManager.ui.InventoryQuickSort();
    }
    public void SaleItem()
    {
        for (int i = currentItems.Count; i > 0; i--)
        {
            if (currentItems[i].type == ItemType.forage)
            {
                GameInstance.instance.money += currentItems[i].praice;
                currentItems.RemoveAt(i);
            }
        }
        items.Clear();
        items = new List<BaseItem> (currentItems);
    }
    public bool GetItem(BaseItem _item)
    {
        if(currentItems.Count < maxBag)
        {
            currentItems.Add(_item);

            GameInstance.instance._gameManager.ui.InventoryQuickSort();
            return true;
        } else
        {
            return false;
        }

    }

    public void GiveItem()
    {
        if(select < currentItems.Count)
        {
            if (currentItems[select].type != ItemType.equment)
            {
                Vector3 _itempos = GameInstance.instance._gameManager.player.head.transform.position;
                Vector3 spwan = _itempos + GameInstance.instance._gameManager.player.head.transform.forward * 2;
                Instantiate(_itemData.Find(o => o.name == currentItems[select].name).data, spwan, Quaternion.identity);
                currentItems.RemoveAt(select);
            }
        }
        GameInstance.instance._gameManager.ui.InventoryQuickSort();
    }

    public void UseItem()
    {
        if(select < currentItems.Count)
        {
            switch(currentItems[select].use)
            {
                case ItemUse.mideice:
                    currentItems.RemoveAt(select);
                    break;
                case ItemUse.Oxygen:
                    currentItems.RemoveAt(select);
                    break;
                case ItemUse.booster:
                    currentItems.RemoveAt(select);
                    break;
                case ItemUse.strong_booster:
                    currentItems.RemoveAt(select);
                    break;
                case ItemUse.compass:
                    currentItems.RemoveAt(select);
                    break;
                case ItemUse.deodrante:
                    currentItems.RemoveAt(select);
                    break;
                default:
                    break;
            }
        }
        GameInstance.instance._gameManager.ui.InventoryQuickSort();
    }

    public void mideice()
    {
        if (GameInstance.instance._gameManager.player.currentHp < GameInstance.instance._gameManager.player.maxHp - 25) GameInstance.instance._gameManager.player.currentHp += 25;
        else GameInstance.instance._gameManager.player.currentHp = GameInstance.instance._gameManager.player.maxHp;
    }

    public void Oxygen()
    {
        if (GameInstance.instance._gameManager.player.currentOxygen < GameInstance.instance._gameManager.player.maxOxygen - 25) GameInstance.instance._gameManager.player.currentOxygen += 25;
        else GameInstance.instance._gameManager.player.currentOxygen = GameInstance.instance._gameManager.player.maxOxygen;
    }

    public void booster(int _strong)
    {
        GameInstance.instance._gameManager.player.boosterTime = 3f;
        GameInstance.instance._gameManager.player.boosterMove = _strong;
    }

    GameObject targetTreasure = null;
    float compassTime;
    public void Compass()
    {
        GameObject[] treasures = GameObject.FindGameObjectsWithTag("Treasure");
        GameObject neasure = null;
        float minDistance = Mathf.Infinity;
        if(treasures.Length != 0)
        {
            foreach (GameObject treasure in treasures)
            {
                float dist = Vector3.Distance(GameInstance.instance._gameManager.player.transform.position, treasure.transform.position);
                if (minDistance > dist)
                {
                    neasure = treasure;
                    minDistance = dist;
                }
            }
        }

        if(neasure != null)
        {
            GameInstance.instance._gameManager.player.compass.transform.GetChild(0).gameObject.SetActive(true);
            targetTreasure = neasure;
            compassTime = 5f;
        }
    }

    public void Deodrante()
    {
        GameInstance.instance._gameManager.player.noneTargetTime = 6f;
    }

    private void Update()
    {
        if(compassTime > 0)
        {
            GameInstance.instance._gameManager.player.compass.transform.GetChild(0).LookAt(targetTreasure.transform.position);
            compassTime -= Time.deltaTime;
        } else
        {
            GameInstance.instance._gameManager.player.compass.transform.GetChild(0).gameObject.SetActive(false);
        }

        currentWeight = currentItems.Sum(p => p.praice);
    }
}
    