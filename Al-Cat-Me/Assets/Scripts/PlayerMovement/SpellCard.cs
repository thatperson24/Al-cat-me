using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpellCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Spell cardSpell;
    private bool selected;
    private EncounterMap encounterMap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selected = false;
        encounterMap = GameObject.Find("CombatMap").GetComponent<EncounterMap>();
        gameObject.GetComponent<Button>().onClick.AddListener(ClickSpell);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickSpell()
    {
        CharacterControl characterControl = GameObject.Find("Character").GetComponent<CharacterControl>();
        if (characterControl.GetIsLocked() && !selected) { return; }
        if (!selected)
        {
            GameObject.Find("CombatMap")
                .GetComponent<Combat>()
                .SetSpellCard(this);
            characterControl.SetIsLocked(true);
            int height = characterControl.GetRow();
            int column = characterControl.GetCol();
            MapTile[][] mapTiles = encounterMap.GetMapTiles();
            for (int i = 0; i < cardSpell.GetRange(); i++)
            {
                if (height + i < mapTiles.Length)
                {
                    mapTiles[height + i][column].IndicateAttack();
                    encounterMap.AddIndicated(mapTiles[height + i][column]);
                }
                if (height - i >= 0)
                {
                    mapTiles[height - i][column].IndicateAttack();
                    encounterMap.AddIndicated(mapTiles[height - i][column]);
                }
                if (column + i < mapTiles.Max(tileRow => tileRow.Length))
                {
                    mapTiles[height][column + i].IndicateAttack();
                    encounterMap.AddIndicated(mapTiles[height][column + i]);
                }
                if (column - i >= 0)
                {
                    mapTiles[height][column - i].IndicateAttack();
                    encounterMap.AddIndicated(mapTiles[height][column - i]);
                }
            }
        }
        else
        {

            encounterMap.ClearIndicated();
            characterControl.SetIsLocked(false);
        }
        selected = !selected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void SetSpell(Spell spell)
    {
        cardSpell = spell;
    }
}
