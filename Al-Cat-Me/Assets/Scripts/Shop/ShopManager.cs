using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private GameController gameController;
    public GameObject yourGoldText;
    public GameObject shopkeeperText;
    public GameObject shopkeeperImage;
    private readonly float ANIM_TIME = 4;
    private float animTimer;
    private bool isDefault;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        yourGoldText.GetComponent<TextMeshProUGUI>().SetText("Your Gold: " + gameController.getGold());
        shopkeeperText.GetComponent<TextMeshProUGUI>().SetText("Buy something, will ya?");
        shopkeeperImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("shopkeeper/Shopkeeper_Fox");
        animTimer = 0f;
        isDefault = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDefault) animTimer += Time.deltaTime;
        if (animTimer >= ANIM_TIME) setDefault();
    }

    public bool attemptPurchase(int amount, Ingredient ingredient)
    {
        var playerGold = gameController.getGold();
        if(amount > playerGold)
        {
            //insufficient funds, do nothing
            shopkeeperText.GetComponent<TextMeshProUGUI>().SetText("You trynna' rip me off, son?");
            //maybe update shopkeeper animation
            setAngy();
            return false;
        }
        else
        {
            List<Ingredient> ingredientAsList = new List<Ingredient> { ingredient };
            gameController.addIngredients(ingredientAsList);
            gameController.addGold(-amount);
            yourGoldText.GetComponent<TextMeshProUGUI>().SetText("Your Gold: " + gameController.getGold());
            setHapp();
            return true;
        }
    }

    void setDefault()
    {
        shopkeeperImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("shopkeeper/Shopkeeper_Fox");
        animTimer = 0f;
        shopkeeperText.GetComponent<TextMeshProUGUI>().SetText("Buy something, will ya?");
        isDefault = true;
    }

    void setAngy()
    {
        shopkeeperImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("shopkeeper/Shopkeeper_Fox_Angy");
        isDefault = false;
    }

    void setHapp()
    {
        shopkeeperImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("shopkeeper/Shopkeeper_Fox_Happy");
        shopkeeperText.GetComponent<TextMeshProUGUI>().SetText("Well now, thank you kindly!");
        isDefault = false;
    }
}
