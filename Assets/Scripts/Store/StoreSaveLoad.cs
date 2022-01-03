using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class ItemUnlockData {
    public int positionIndex;
    public bool isOwned;
}

public class UnlockData {
    public List<ItemUnlockData> itemUnlockDataList;
}

public class StoreSaveLoad : MonoBehaviour
{
   public List<ItemUnlockData> itemUnlockDataList;
   ShopUI shopUI;
   MenuManager menuManager;
   ScoreManager scoreManager;
   
   void Awake() {

       shopUI = GetComponent<ShopUI>();
       menuManager = FindObjectOfType<MenuManager>();
       
        itemUnlockDataList = new List<ItemUnlockData>() {
            new ItemUnlockData{ positionIndex = 0, isOwned = false}, //cuz n 0 is boost item
            new ItemUnlockData{ positionIndex = 1, isOwned = true},
            new ItemUnlockData{ positionIndex = 2, isOwned = false},
            new ItemUnlockData{ positionIndex = 3, isOwned = false},
            new ItemUnlockData{ positionIndex = 4, isOwned = false},
            new ItemUnlockData{ positionIndex = 5, isOwned = false},
            new ItemUnlockData{ positionIndex = 6, isOwned = false}
        };

          LoadUnlockDataList();

   }

    private void OnEnable() {
         if(PlayerPrefs.HasKey(PlayerPrefKeys.kCurrentSkin)) {
            shopUI.position = PlayerPrefs.GetInt(PlayerPrefKeys.kCurrentSkin, 1);
            shopUI.startPosition = shopUI.position;
         }
    }

     private void OnDisable() {
         //if(PlayerPrefs.HasKey(PlayerPrefKeys.kCurrentSkin)) {
            shopUI.position = PlayerPrefs.GetInt(PlayerPrefKeys.kCurrentSkin, 1);
            shopUI.startPosition = shopUI.position;
        // }
    }

    public void SaveUnlockDataList() {
        UnlockData unlockData = new UnlockData {itemUnlockDataList = itemUnlockDataList};
        string json = JsonUtility.ToJson(unlockData);
        PlayerPrefs.SetString("ItemUnlockTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("ItemUnlockTable"));
    }

    public void LoadUnlockDataList() {

        if(PlayerPrefs.HasKey("ItemUnlockTable")){
            string jsonString = PlayerPrefs.GetString("ItemUnlockTable");
           UnlockData unlockData = JsonUtility.FromJson<UnlockData>(jsonString);
           itemUnlockDataList = unlockData.itemUnlockDataList;
        }
        else {
            SaveUnlockDataList();
        }   
        
        
    }

    //  public void ResetItemDataKey() {

    //     if(PlayerPrefs.HasKey("ItemUnlockTable")) {
    //         if(itemUnlockDataList != null) {
    //             foreach (ItemUnlockData item in itemUnlockDataList)
    //             {
    //                 item.isOwned = false;
    //             }
    //         }
    //     }
    //      itemUnlockDataList = new List<ItemUnlockData>() {
    //                 new ItemUnlockData{ positionIndex = 0, isOwned = true},
    //                 new ItemUnlockData{ positionIndex = 1, isOwned = false},
    //                 new ItemUnlockData{ positionIndex = 2, isOwned = false},
    //                 new ItemUnlockData{ positionIndex = 3, isOwned = false}
    //                 };
    //             SaveUnlockDataList();
        
    // }

///////////// BUTTONS /////////////


      public void testBuy() {

         switch (shopUI.position) {
            
            case 0:
            TryBuyItem(StoreItem.ItemType.BoostTo15);
            break;
            case 1:
            TryBuyItem(StoreItem.ItemType.BasicBall);
            break;
            case 2:
            TryBuyItem(StoreItem.ItemType.FrostBall);
            break;
            case 3:
            TryBuyItem(StoreItem.ItemType.NatureBall);
            break;
            case 4:
            TryBuyItem(StoreItem.ItemType.XmasBall);
            break;
            case 5:
            TryBuyItem(StoreItem.ItemType.LoveBall);
            break;
            case 6:
            TryBuyItem(StoreItem.ItemType.ThunderBall);
            break;
        }
    }

    public void TryBuyItem(StoreItem.ItemType itemType) {
        if(ScoreManager.Instance.TrySpendStarAmount(StoreItem.GetCost(itemType))) {
            //can afford so buy it
            itemUnlockDataList[shopUI.position].isOwned = true;
            SaveUnlockDataList();
            Analytics.CustomEvent("Store Purchase Skin" + itemUnlockDataList[shopUI.position]);

            if(itemType == StoreItem.ItemType.BoostTo15) {
                itemUnlockDataList[shopUI.position].isOwned = true;
                
                PlayerPrefKeys.kBoostCountInt++;
                PlayerPrefs.SetInt(PlayerPrefKeys.kBoostCount, PlayerPrefKeys.kBoostCountInt);
                PlayerPrefs.Save();
                SaveUnlockDataList();
                 Analytics.CustomEvent("Store Purchase Boost" + PlayerPrefKeys.kBoostCountInt);
            }
        
        }
    }


    public void SelectCurrentSkin() {
        PlayerPrefs.SetInt(PlayerPrefKeys.kCurrentSkin, PlayerPrefKeys.kCurrentSkinInt);
        PlayerPrefs.Save();
        
        menuManager.BackToMenuButton();

    }
}
