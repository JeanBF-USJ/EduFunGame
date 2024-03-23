using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockerItem : MonoBehaviour
{
    public string id;
    public string itemName;
    public string description;
    public RawImage image;
    public Button button;
    
    void Start()
    {
        button.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        PlayerPrefs.SetString("playerCharacter", itemName);
        FindObjectOfType<HubManager>().SetPlayerCharacter(null);
        FindObjectOfType<HubManager>().GetComponent<LockerManager>().SetPlayerInfo(itemName, description);
    }
}
