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
        FindObjectOfType<HubManager>().SetPlayerCharacter(itemName);
        FindObjectOfType<HubManager>().GetComponent<ShopManager>().SetPlayerInfo(itemName, description);
    }
}