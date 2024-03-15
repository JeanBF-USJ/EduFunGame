using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public string name;
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
        PlayerPrefs.SetString("playerCharacter", name);
        FindObjectOfType<HubManager>().SetPlayerCharacter();
        FindObjectOfType<HubManager>().SetPlayerInfo(name, description);
    }
}