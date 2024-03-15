using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Transform shopItemsContainer;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private APIManager _apiManager;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();

        string apiEndpoint = "/accessories/get";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, null, true, HandleResponse));
    }

    private void HandleResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture;
            AccessoriesResponse response = JsonUtility.FromJson<AccessoriesResponse>(www.downloadHandler.text);
            foreach (Accessory accessory in response.accessories)
            {
                GameObject newItem = Instantiate(itemPrefab, shopItemsContainer);
                ShopItem shopItem = newItem.GetComponent<ShopItem>();
                shopItem.name = accessory.name;
                shopItem.description = accessory.description;
                shopItem.price.text = "" + accessory.price;

                texture = Resources.Load<Texture2D>("PlayerIcons/" + accessory.name);
                shopItem.image.texture = texture;
            }
        }
    }

    public void SetPlayerInfo(string name, string description)
    {
        itemName.text = name;
        itemDescription.text = description;
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
    public string name;
    public string description;
    public int price;
}