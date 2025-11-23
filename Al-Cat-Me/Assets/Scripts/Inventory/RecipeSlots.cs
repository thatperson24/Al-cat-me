using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RecipeSlots : MonoBehaviour
{
    [SerializeField] private List<GameObject> inventorySlots;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private Button createButton;
    private GameController gameController;
    // private int numSlots;
    private int[] stats = new int[6];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //    numSlots = transform.childCount;
        createButton.onClick.AddListener(CreateSpell);
        gameController = transform.Find("GameController").GetComponent<GameController>();
    }

    public void UpdateRecipe()
    {
        stats = new int[6];
        bool isFull = true;
        foreach (GameObject slot in inventorySlots)
        {

            if (slot.transform.childCount > 0)
            {
                GameObject ingredient = slot.transform.GetChild(0).gameObject;
                for (int i = 0; i < stats.Length; i++)
                {
                    stats[i] += ingredient.GetComponent<Ingredient>().GetStats()[i];
                }
            }
            else
            {
                isFull = false;
            }
        }
        if (isFull)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject newSlot = Instantiate(inventorySlotPrefab, this.gameObject.transform);
                inventorySlots.Add(newSlot);
            }
        }
        statText.text = "Damage: " + stats[0] +
                        "\nRange: " + stats[1] +
                        "\nAOE: " + stats[2] +
                        "\nCast Type: " + stats[3] +
                        "\nBlocking: " + stats[4] +
                        "\nDelay: " + stats[5] +
                        "\nCost: " + stats.Sum()/6;
    }

    public void CreateSpell()
    {
        Spell newSpell = ScriptableObject.CreateInstance<Spell>();
        newSpell.SetStats(spellName.text, stats);

        if (spellName.text.Equals(""))
        {
            newSpell.name = GenerateDefaultName();
        }
        else
        {
            newSpell.name = spellName.text;
        }
        Debug.Log("newSpell name: " + newSpell.name);
        gameController.addSpell(newSpell);
        ClearRecipe();
    }

    public void ClearRecipe()
    {
        foreach (GameObject slot in inventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    private string GenerateDefaultName()
    {
        // Labels for each stat index
        string[] statNames = { "Strike", "Reach", "Splash", "Focus", "Guard", "Tempo" };

        // Find the highest value
        int bestValue = int.MinValue;
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] > bestValue)
                bestValue = stats[i];
        }

        // Collect all stat names that match the highest value
        List<string> bestStats = new List<string>();
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] == bestValue)
                bestStats.Add(statNames[i]);
        }

        // Join them into a single name
        string combined = string.Join(" ", bestStats);
        return $"{combined} {bestValue}";
    }

}
