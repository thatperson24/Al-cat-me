using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Spell cardSpell;
    private bool selected;
    private EncounterMap encounterMap;
    private List<SpriteRenderer> indicated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selected = false;
        indicated = new List<SpriteRenderer>();
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
        if (!selected) {
            characterControl.SetIsLocked(true);
            int height = characterControl.GetRow();
            int column = characterControl.GetCol();
            for (int i = 0; i < cardSpell.GetRange(); i++) {
                if (height + i < encounterMap.GetMapTiles().Length) { 
                    encounterMap.GetMapTiles()[height + i][column].gameObject.transform.Find("AttackIndicator").GetComponent<SpriteRenderer>().enabled = true;
                    indicated.Add(encounterMap.GetMapTiles()[height + i][column].gameObject.transform.Find("AttackIndicator").GetComponent<SpriteRenderer>());
                }
                if (height - i >= 0) { 
                    encounterMap.GetMapTiles()[height - i][column].gameObject.transform.Find("AttackIndicator").GetComponent<SpriteRenderer>().enabled = true;
                    indicated.Add(encounterMap.GetMapTiles()[height - i][column].gameObject.transform.Find("AttackIndicator").GetComponent<SpriteRenderer>());
                }
                if (column + i < encounterMap.GetMapTiles().Max(tileRow => tileRow.Length)) { 
                    encounterMap.GetMapTiles()[height][column + i].gameObject.transform.Find("AttackIndicator").GetComponent<SpriteRenderer>().enabled = true;
                    indicated.Add(encounterMap.GetMapTiles()[height][column + i].gameObject.transform.Find("AttackIndicator").GetComponent<SpriteRenderer>());
                }
                if (column - i >= 0) { 
                    encounterMap.GetMapTiles()[height][column - i].gameObject.transform.Find("AttackIndicator").GetComponent<SpriteRenderer>().enabled = true;
                    indicated.Add(encounterMap.GetMapTiles()[height][column - i].gameObject.transform.Find("AttackIndicator").GetComponent<SpriteRenderer>());
                }
            }
        } else {

            foreach(SpriteRenderer render in indicated)
            {
                render.enabled = false;
            }
            indicated.Clear();
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
