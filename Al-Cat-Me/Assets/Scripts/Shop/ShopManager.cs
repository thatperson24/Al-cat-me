using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private GameController gameController;
    public GameObject yourGoldText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        yourGoldText.GetComponent<TextMeshProUGUI>().SetText("Your Gold: " + gameController.getGold());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool attemptPurchase(int amount)
    {
        var playerGold = gameController.getGold();
        if(amount > playerGold)
        {
            //insufficient funds, do nothing
            return false;
        }
        else
        {
            gameController.addGold(-amount);
            //gameController.addIngredients();
            yourGoldText.GetComponent<TextMeshProUGUI>().SetText("Your Gold: " + gameController.getGold());
            return true;
        }
    }
}
