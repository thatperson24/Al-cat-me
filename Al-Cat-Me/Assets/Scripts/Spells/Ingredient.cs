using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int range;
    [SerializeField] private int aoe;
    [SerializeField] private int castType;
    [SerializeField] private int blocking;
    [SerializeField] private int delay;

    public int[] GetStats()
    {
        int[] stats = { damage, range, aoe, castType, blocking, delay };
        return stats;
    }
}
