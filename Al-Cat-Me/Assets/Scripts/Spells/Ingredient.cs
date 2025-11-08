using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private int damage;
    [SerializeField] private int range;
    [SerializeField] private int aoe;
    [SerializeField] private int castType;
    [SerializeField] private int blocking;
    [SerializeField] private int delay;

    public Ingredient(string name, int damage, int range, int aoe, int castType, int blocking, int delay)
    {
        this.name = name;
        this.damage = damage;
        this.range = range;
        this.aoe = aoe;
        this.castType = castType;
        this.blocking = blocking;
        this.delay = delay;
    }

    public int[] GetStats()
    {
        int[] stats = { damage, range, aoe, castType, blocking, delay };
        return stats;
    }

    public string GetName()
    {
        return name;
    }
}
