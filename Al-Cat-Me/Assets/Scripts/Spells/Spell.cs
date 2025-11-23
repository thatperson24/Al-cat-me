using UnityEngine;
using System.Collections.Generic;

public class Spell : ScriptableObject
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


    public void CopySpell(Spell spell)
    {
        spellName = spell.GetSpellName();
        damage = spell.GetDamage();
        range = spell.GetRange();
        aoe = spell.GetAoe();
        castType = spell.GetCastType();
        blocking = spell.GetBlocking();
        delay = spell.GetDelay();

    }
    public string GetSpellName() { return spellName;}
    public int GetDamage() { return damage; }
    public int GetRange() { return range; }
    public int GetAoe() { return aoe; }
    public int GetCastType() { return castType; }
    public int GetBlocking() { return blocking; }
    public int GetDelay() { return delay; }
}
