using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShopIngredient : MonoBehaviour
{
    private GameController gameController;
    public ShopManager shopManager;
    public GameObject ingredientText;
    private Ingredient ingredient;
    private int price;
    private bool available;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        available = true;

        ingredient = gameController.getRandomIngredient();
        price = ingredientPrice();
        Debug.Log("ingredient name string: " + ingredient.GetName());
        ingredientText.GetComponent<TextMeshProUGUI>().SetText(ingredient.GetName() + "\n" + price + " Gold");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        //replace attemptPurchase amount to ingredient cost
        if (available && shopManager.attemptPurchase(price, ingredient))
        {
            ingredientText.GetComponent<TextMeshProUGUI>().SetText("<color=RED>SOLD OUT</color>");
            available = false;
        }
    }

    private int ingredientPrice()
    {
        List<int> stats = new List<int>(ingredient.GetStats());
        int price = 0;
        stats.ForEach(stat => price += stat);
        return price;
    }
}
