using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public string id;
    public string itemName;
    public string description;
    public RawImage image;
    public TextMeshProUGUI price;
    public Button button;
    
    void Start()
    {
        button.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        HubManager hubManager = FindObjectOfType<HubManager>();
        ShopManager shopManager = hubManager.GetComponent<ShopManager>();

        hubManager.SetPlayerCharacter(itemName);
        shopManager.SetPlayerInfo(this);
        shopManager.EnableBuyButton(hubManager.GetPlayerCoins() > int.Parse(price.text));
    }
}