using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Linq;

public class GameController : MonoBehaviour
{
    private int gold;
    private IReadOnlyDictionary<string, Ingredient> ingredientCodex = new Dictionary<string, Ingredient>
    {
        { "Arrow Shaft", new Ingredient("Arrow Shaft", 0, 2, 0, 0, 0, 0) },
        { "Arrow Head", new Ingredient("Arrow Head", 1, 0, 0, 0, 0, 0) },
        { "Arrow Tail", new Ingredient("Arrow Tail", 0, 0, 0, 0, -1, 0) },
        { "Small Knife", new Ingredient("Small Knife", 1, 0, 0, 0, 0, 0) },
        { "Dynamite", new Ingredient("Dynamite", 1, 0, 3, 0, 0, 0) },
        { "Wooden Shield", new Ingredient("Wooden Shield", 0, 0, 0, 1, 0, 0) },
        { "Oddly Hard Egg", new Ingredient("Oddly Hard Egg", 5, -1, 0, 0, 2, 0) },
        { "Broken Feathers", new Ingredient("Broken Feathers", 0, 0, 0, 0, 1, 0) },
        { "Magma Coal", new Ingredient("Magma Coal", 0, 0, 0, 0, 0, 0) },
        { "Lightning Rod", new Ingredient("Lightning Rod", 0, 0, 0, 0, 1, 0) },
        { "Water Bottle", new Ingredient("Water Bottle", 0, 0, 0, 0, -1, 0) },
        { "Golem Core", new Ingredient("Golem Core", 0, 0, 0, 1, 1, 0) },
        { "Horn of a Unicorn (Legendary)", new Ingredient("Horn of a Unicorn (Legendary)", 5, 5, 2, 0, 2, 0) },
        { "Wet Socks", new Ingredient("Wet Socks", 0, 0, 1, 0, 0, 0) },
        { "Tacks", new Ingredient("Tacks", 1, -1, 0, 0, 0, 0) },
        { "Bone Helm", new Ingredient("Bone Helm", -1, 0, 0, 2, 0, 0) },
        { "Encyclopedia", new Ingredient("Encyclopedia", -1, 3, 1, 0, 0, 0) },
        { "Steel Rod", new Ingredient("Steel Rod", 0, 2, -1, 1, 0, 0) },
        { "Rocket Launcher (Legendary)", new Ingredient("Rocket Launcher (Legendary)", 3, 1, 3, 0, 4, 0) },
        { "Colored Gem", new Ingredient("Colored Gem", 0, 0, 0, 0, 0, 0) },
        { "Multicolored Gem (Legendary)", new Ingredient("Multicolored Gem (Legendary)", 0, 0, 0, 0, 0, 0) },
        { "Double-Edged Sword", new Ingredient("Double-Edged Sword", 3, -2, 0, 0, 1, 0) },
        { "Torn Card 1", new Ingredient("Torn Card 1", 0, 0, 0, 0, 0, 0) },
        { "Torn Card 2", new Ingredient("Torn Card 2", 0, 0, 0, 0, 0, 0) },
        { "Torn Card 3", new Ingredient("Torn Card 3", 0, 0, 0, 0, 0, 0) },
        { "Torn Card 4", new Ingredient("Torn Card 4", 0, 0, 0, 0, 0, 0) },
        { "Torn Card 5", new Ingredient("Torn Card 5", 0, 0, 0, 0, 0, 0) },
        { "Hellbomb", new Ingredient("Hellbomb", 2, -2, 5, 0, 5, 0) },
        { "Bottled Lightning", new Ingredient("Bottled Lightning", 1, 0, 0, 0, 0, 0) },
        { "Broken Compass", new Ingredient("Broken Compass", 0, 2, 0, 0, 1, 0) },
        { "Sheet of Metal", new Ingredient("Sheet of Metal", 0, 0, 0, 3, 1, 0) },
        { "Wallet", new Ingredient("Wallet", 0, 0, 0, 0, 0, 0) }, // +10 Money (special)
        { "Dinosaur Shaped Rock", new Ingredient("Dinosaur Shaped Rock", 0, 1, 1, 0, 0, 0) },
        { "Sparkling Bathwater in a Jar", new Ingredient("Sparkling Bathwater in a Jar", 0, 0, 0, 0, -1, 0) },
        { "Leafblower", new Ingredient("Leafblower", 0, 2, 0, 0, 0, 0) }
    };
    private List<Ingredient> ingredients;
    private List<Spell> deck;
    private Overmap overMap;

    private void Start()
    {
        gold = 10;
        ingredients = new List<Ingredient>();
        GenerateBasicDeck();
    }

    void Update()
    {
        
    }

    public int getGold()
    {
        return gold;
    }

    public void addGold(int amount)
    {
        gold += amount;
    }

    public void addSpell(Spell spell)
    {
        deck.Add(spell);
    }

    public void removeSpell(Spell spell)
    {
        deck.Remove(spell);
    }

    public void addIngredients(List<Ingredient> ingredients)
    {
        ingredients.ForEach(ingredient => this.ingredients.Add(ingredient));
    }

    public void removeIngredients(List<Ingredient> ingredients)
    {
        ingredients.ForEach(ingredient => this.ingredients.Remove(ingredient));
    }

    public Ingredient getRandomIngredient()
    {
        List<Ingredient> ingredientList = ingredientCodex.Values.ToList();
        ingredientList.ForEach(ingredient => Debug.Log(ingredient + "\n"));
        int index = Mathf.FloorToInt(Random.value * ingredientList.Count);
        //Debug.Log("info: \n index: " + index + "\n ingredient: " + ingredientList[index]);
        return ingredientList[index];
    }

    //      These are more or less the two states of a game session, the player is either in the OverMap or in an Encounter
    //      OverMap class (or whatever you call it) will be responsible for manipulating its own data to transition between the two states since it contains that data
    public void callEncounter()
    {
        // overMap.selectEncounter(1);
    }
    
    public void endEncounter()
    {
        // overMap.finishEncounter();
    }

    private void GenerateBasicDeck()
    {
        deck = new List<Spell>();
        for (int i = 0; i < 10; i++)
        {
            Spell newSpell = ScriptableObject.CreateInstance<Spell>();
            int[] offenseStats = { 3, 3, 0, 0, 0, 0 };
            newSpell.SetStats("Blast", offenseStats);
            deck.Add(newSpell);
            newSpell = ScriptableObject.CreateInstance<Spell>();
            int[] defenseStats = { 0, 0, 0, 0, 5, 0 };
            newSpell.SetStats("Magic Barrier", defenseStats);
            deck.Add(newSpell);
        }

        GameObject.Find("CombatMap").GetComponent<Combat>().ShuffleDeck();
        GameObject.Find("CombatMap").GetComponent<Combat>().DrawCards(5);

    }
    public List<Spell> GetSpells()
    {
        return deck;
    }
}
