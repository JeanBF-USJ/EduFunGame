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
        HubManager hubManager = FindObjectOfType<HubManager>();
        
        PlayerPrefs.SetString(hubManager.GetID(), itemName);
        PlayerPrefs.Save();
        
        hubManager.SetPlayerCharacter(null);
        hubManager.GetComponent<LockerManager>().SetPlayerInfo(itemName, description);
    }
}
