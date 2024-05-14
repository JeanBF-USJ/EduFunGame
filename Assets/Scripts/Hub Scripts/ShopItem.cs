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
    public GameObject owned;
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
        
        if (owned.activeSelf) shopManager.HideBuyButton();
        else shopManager.EnableBuyButton(hubManager.GetPlayerCoins() >= int.Parse(price.text));
    }
}