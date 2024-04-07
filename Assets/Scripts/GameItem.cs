using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameItem : MonoBehaviour
{
    public string id;
    public TextMeshProUGUI itemName;
    public string description;
    public string category;
    public string minAge;
    public string maxAge;
    public bool favorite;
    public RawImage image;
    public Button button;
    void Start()
    {
        button.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        FindObjectOfType<HubManager>().GetComponent<GameDescriptionManager>().OpenGameDescription(this);
    }
}
