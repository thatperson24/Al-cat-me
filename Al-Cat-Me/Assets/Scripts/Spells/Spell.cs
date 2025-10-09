using UnityEngine;
using System.Collections.Generic;

public class Spell : MonoBehaviour
{
    [SerializeField] private string spellName;
    [SerializeField] private int damage;
    [SerializeField] private int range;
    [SerializeField] private int aoe;
    [SerializeField] private int castType;
    [SerializeField] private int blocking;
    [SerializeField] private int delay;

    public void SetStats(string name, int[] stats)
    {
        spellName = name;
        damage = stats[0];
        range = stats[1];
        aoe = stats[2];
        castType = stats[3];
        blocking = stats[4];
        delay = stats[5];
    }

}
