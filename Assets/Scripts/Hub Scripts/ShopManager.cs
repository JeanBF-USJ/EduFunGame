using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Transform shopItemsContainer;
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Button buyButton;
    [SerializeField] private GameObject failedToBuyText;

    private APIManager _apiManager;
    private ShopItem _previewedShopItem;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();

        string apiEndpoint = "/accessories/get";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, null, true, DisplayShopItems));
    }

    private void DisplayShopItems(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture;
            AccessoriesResponse response = JsonUtility.FromJson<AccessoriesResponse>(www.downloadHandler.text);
            foreach (Accessory accessory in response.accessories)
            {
                GameObject newItem = Instantiate(shopItemPrefab, shopItemsContainer);
                ShopItem shopItem = newItem.GetComponent<ShopItem>();
                shopItem.id = accessory._id;
                shopItem.itemName = accessory.name;
                shopItem.description = accessory.description;
                shopItem.price.text = "" + accessory.price;
                if (accessory.owned) shopItem.owned.SetActive(true);

                texture = Resources.Load<Texture2D>("PlayerIcons/" + accessory.name);
                shopItem.image.texture = texture;
            }
        }
    }

    public void SetPlayerInfo(ShopItem shopItem)
    {
        _previewedShopItem = shopItem;
        itemName.text = shopItem.itemName;
        itemDescription.text = shopItem.description;
    }
    
    public void ResetPlayerInfo()
    {
        itemName.text = "";
        itemDescription.text = "";
        buyButton.gameObject.SetActive(false);
    }

    public void EnableBuyButton(bool enable)
    {
        buyButton.interactable = enable;
        if (!buyButton.gameObject.activeSelf) buyButton.gameObject.SetActive(true);
    }

    public void HideBuyButton()
    {
        if (buyButton.gameObject.activeSelf) buyButton.gameObject.SetActive(false);
    }

    public void BuyItem()
    {
        string apiEndpoint = "/accessories/buy";
        string jsonStr = "{\"accessory_id\":\"" + _previewedShopItem.id + "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, jsonStr, true, HandleBuyResponse));
        if (failedToBuyText.activeSelf) failedToBuyText.SetActive(false);
    }

    private void HandleBuyResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            FindObjectOfType<HubManager>().SetUserProfile();
            _previewedShopItem.owned.SetActive(true);
            HideBuyButton();
        } else failedToBuyText.SetActive(true);
    }
}

[Serializable]
public class AccessoriesResponse
{
    public Accessory[] accessories;
}

[Serializable]
public class Accessory
{
    public string _id;
    public string name;
    public string description;
    public int price;
    public bool owned;
}