using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Transform shopItemsContainer;
    [SerializeField] private GameObject itemPrefab;

    private void Start()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, shopItemsContainer);
            ShopItem shopItem = newItem.GetComponent<ShopItem>();
            // shopItem.image.texture = LoadTexture("Assets/Images/NinjaIcon.png");
            shopItem.price.text = "" + (i * 100 + 100);
        }
    }
    
    private Texture2D LoadTexture(string path)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadRawTextureData(fileData);
        texture.Apply();

        return texture;
    }
}
