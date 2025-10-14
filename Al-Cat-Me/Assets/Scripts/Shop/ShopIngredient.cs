using UnityEngine;
using TMPro;

public class ShopIngredient : MonoBehaviour
{
    public ShopManager shopManager;
    public GameObject ingredientText;
    public Ingredient ingredient;
    private bool available;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        available = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        //replace attemptPurchase amount to ingredient cost
        if (available && shopManager.attemptPurchase(1))
        {
            ingredientText.GetComponent<TextMeshProUGUI>().SetText("<color=RED>SOLD OUT</color>");
            available = false;
        }
    }
}
