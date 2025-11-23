using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Linq;
using Mono.Cecil;
using Unity.VisualScripting.Antlr3.Runtime.Misc;


public class GameController : MonoBehaviour
{
    private int gold;
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

    public Ingredient getIngredient()
    {
        return new Ingredient();
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
