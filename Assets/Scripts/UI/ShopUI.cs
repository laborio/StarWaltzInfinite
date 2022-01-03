using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    PlayerController2 playerController2;
    StoreSaveLoad storeSaveLoad;
    ScoreManager scoreManager;
    MenuManager menuManager;
    private Transform container;
    private Transform shopItemTemplate;
    private Transform stars;
    private Text starText;
    private TMP_Text nameText;
    private TMP_Text costText;
    [SerializeField] Button selectButton; //has to be dragged from the inspector cuz can't find a way to ref it via script
    [SerializeField] Button buyButton;
    private TMP_Text selectText;
    private TMP_Text buyText;
    private Image lockIcon;
    [HideInInspector] public int position;
    [HideInInspector] public int startPosition;
    public int giveStarAmount = 500;
    // int sysHour = System.DateTime.Now.Hour;
    // [SerializeField] private TMP_Text timeText;


    void Awake() {
      
        storeSaveLoad = GetComponent<StoreSaveLoad>();

        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        stars = container.Find("Stars");
    
        
        lockIcon = selectButton.GetComponentInChildren<Image>();
        selectText = selectButton.GetComponentInChildren<TMP_Text>();
        buyText = buyButton.GetComponentInChildren<TMP_Text>();
        starText = stars.Find("StarText").GetComponent<Text>();
        nameText = shopItemTemplate.Find("nameText").GetComponent<TextMeshProUGUI>();
        costText = shopItemTemplate.Find("priceText").GetComponent<TextMeshProUGUI>();

        ScoreManager.Instance.OnStarCountChanged += ScoreManager_OnStarCountChanged;

    }

    void OnEnable() {
        starText.text = ScoreManager.Instance.GetStarCount().ToString();

    }
  

    private void ScoreManager_OnStarCountChanged(object sender, System.EventArgs e) {

        if(starText != null)   
        starText.text = ScoreManager.Instance.GetStarCount().ToString();
            
    }


    void Update() {
//        Debug.Log(position);
        switch (position) {
            case 0:
            CreateItemSlot("Boost Lv.15", StoreItem.GetCost(StoreItem.ItemType.BoostTo15), storeSaveLoad.itemUnlockDataList[0].isOwned);
            break;
            case 1:
            CreateItemSlot("Basic", StoreItem.GetCost(StoreItem.ItemType.BasicBall), storeSaveLoad.itemUnlockDataList[1].isOwned);
            break;
            case 2:
            CreateItemSlot("Frost", StoreItem.GetCost(StoreItem.ItemType.FrostBall), storeSaveLoad.itemUnlockDataList[2].isOwned);
            break;
            case 3:
            CreateItemSlot("Nature", StoreItem.GetCost(StoreItem.ItemType.NatureBall), storeSaveLoad.itemUnlockDataList[3].isOwned);
            break;
            case 4:
            CreateItemSlot("Xmas", StoreItem.GetCost(StoreItem.ItemType.XmasBall), storeSaveLoad.itemUnlockDataList[4].isOwned);
            break;
            case 5:
            CreateItemSlot("Love", StoreItem.GetCost(StoreItem.ItemType.LoveBall), storeSaveLoad.itemUnlockDataList[5].isOwned);
            break;
            case 6:
            CreateItemSlot("Electric", StoreItem.GetCost(StoreItem.ItemType.ThunderBall), storeSaveLoad.itemUnlockDataList[6].isOwned);
            break;
            
        }
        
    }

    private void CreateItemSlot(string itemName, int itemCost, bool isOwned) {
        
        nameText.text = itemName;
        costText.text = itemCost.ToString();

        if(itemName == "Boost Lv.15") { //for the boost item
            
             if(isOwned) {
                selectButton.interactable = false;
                buyButton.interactable = true;
                buyText.text = "BUY";
                selectText.text = "x" + PlayerPrefKeys.kBoostCountInt;
                selectText.color = new Color(0, 255, 0, 255);
            }
            else {
                selectButton.interactable = false;
                buyButton.interactable = true;
                buyText.text = "BUY";
                selectText.text = "x" + PlayerPrefKeys.kBoostCountInt;
                selectText.color = new Color(255, 0, 0, 255);
            }

        }

        else { //for skins
       
            if(isOwned) {
                selectButton.interactable = true;
                buyButton.interactable = false;
                buyText.text = "OWNED";
                selectText.text = "SELECT";
                selectText.color = new Color(0, 255, 0, 255);
            }
            else {
                selectButton.interactable = false;
                buyButton.interactable = true;
                buyText.text = "BUY";
                selectText.text = "LOCKED";
                selectText.color = new Color(255, 0, 0, 255);
            }
        }
        
         
    }

    public void ShowNextItemButton() {
        position++;
        if(position >= (storeSaveLoad.itemUnlockDataList.Count) ) position = 0;

        PlayerPrefKeys.kCurrentSkinInt = position;      

        PlayerController2.Instance.AddSelectedSkinToList();
        PlayerController2.Instance.SetPlayerPrefab_GM(2); //we pass int 2 because the PlayerPrefabMenu is on the [2] position in the list 
    }

    public void ShowPreviousItemButton() {
        position--;
        if(position < 0) position = (storeSaveLoad.itemUnlockDataList.Count -1);
        
        PlayerPrefKeys.kCurrentSkinInt = position;      

        PlayerController2.Instance.AddSelectedSkinToList();
        PlayerController2.Instance.SetPlayerPrefab_GM(2); //we pass int 2 because the PlayerPrefabMenu is on the [2] position in the list 
    }

    public void BuyStarsButton() {
        ScoreManager.Instance.AddStar(giveStarAmount);
    }

    
   
}
