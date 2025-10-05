using UnityEngine;
using System.Collections.Generic;

public class Spell : MonoBehaviour
{
    private string spellName;
    [SerializeField] private List<Ingredient> ingredients;

    [SerializeField] private int damage;
    [SerializeField] private int range;
    [SerializeField] private int aoe;
    [SerializeField] private int castType;
    [SerializeField] private int blocking;
    [SerializeField] private int delay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddIngredient(Ingredient newIngredient)
    {

    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        
    }
}
