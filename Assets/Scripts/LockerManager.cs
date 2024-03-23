using TMPro;
using UnityEngine;

public class LockerManager : MonoBehaviour
{
    [SerializeField] private Transform lockerItemsContainer;
    [SerializeField] private GameObject lockerItemPrefab;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void EmptyLockerItems()
    {
        foreach (Transform child in lockerItemsContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayLockerItems(UserAccessory[] userAccessories)
    {
        EmptyLockerItems();
        
        Texture2D texture;
        foreach (UserAccessory userAccessory in userAccessories)
        {
            GameObject newItem = Instantiate(lockerItemPrefab, lockerItemsContainer);
            LockerItem lockerItem = newItem.GetComponent<LockerItem>();
            lockerItem.id = userAccessory._id;
            lockerItem.itemName = userAccessory.name;
            lockerItem.description = userAccessory.description;

            texture = Resources.Load<Texture2D>("PlayerIcons/" + userAccessory.name);
            lockerItem.image.texture = texture;
        }
    }
    
    public void SetPlayerInfo(string name, string description)
    {
        itemName.text = name;
        itemDescription.text = description;
    }

    public void ResetPlayerInfo()
    {
        itemName.text = "";
        itemDescription.text = "";
    }
}
