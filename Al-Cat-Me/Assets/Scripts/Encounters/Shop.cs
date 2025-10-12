using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour, Encounter
{
    public GameController gameController;
    private List<Ingredient> shopIngredients;

    public void startEncounter()
    {
        generateStock();
    }

    public void endEncounter()
    {

    }

    public void generateStock()
    {
        //when there are actual ingredients, pick random ones
        shopIngredients = new List<Ingredient>();
        for(int i = 0; i < 10; i++)
        {
            shopIngredients.Add(new Ingredient());
        }
    }

    public void buyIngredient(Ingredient ingredient)
    {
        shopIngredients.Remove(ingredient);
        gameController.addIngredients(new List<Ingredient> { ingredient });
        gameController.addGold(-10);//where do I get the ingredient price?
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
