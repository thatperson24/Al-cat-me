using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Combat : MonoBehaviour
{
    public GameController gameController;

    private List<Spell> spellList;
    private List<Spell> discard;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform CardSpawn;
    [SerializeField] private List<Transform> cardSlots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpellCard currentSpell;
    void Start()
    {

    }

    public void startEncounter()
    {

    }

    public void endEncounter()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShuffleDeck()
    {
        spellList = gameController.GetSpells();

        int count = spellList.Count;
        int last = count - 1;

        for (int i = 0; i < last; ++i)
        {
            int r = Random.Range(i, count);
            Spell tmp = spellList[i];
            spellList[i] = spellList[r];
            spellList[r] = tmp;
        }
    }

    public void DrawCards(int numCards)
    {
        StartCoroutine(Move(0));
    }

    private IEnumerator Move(int cardNum)
    {
        GameObject newCard = Instantiate(cardPrefab, GameObject.Find("Cards").transform);
        Spell spell = ScriptableObject.CreateInstance<Spell>();
        spell.CopySpell(spellList[cardNum]);
        newCard.GetComponent<SpellCard>().SetSpell(spell);
        newCard.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = spell.GetSpellName();
        newCard.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Damage: " + spell.GetDamage() +
                                                                                                    "\nRange: " + spell.GetRange() +
                                                                                                    "\nAOE: " + spell.GetAoe() +
                                                                                                    "\nCast Type: " + spell.GetType() +
                                                                                                    "\nBlocking: " + spell.GetBlocking() +
                                                                                                    "\nDelay: " + spell.GetDelay(); 
        float moveDuration = .25f;
        //Make a note of where we are and where we want to go
        Vector2 startPos = CardSpawn.position;
        Vector2 endPos = cardSlots[cardNum].position;

        float elaspedTime = 0;
        while (elaspedTime < moveDuration)
        {
            elaspedTime += Time.deltaTime;
            float percent = elaspedTime / moveDuration;
            newCard.transform.position = Vector2.Lerp(startPos, endPos, percent);
            yield return null;
        }

        //Make sure we go where we want
        newCard.transform.position = endPos;
        if (cardNum + 1 < cardSlots.Count)
        {
            StartCoroutine(Move(++cardNum));
        }
    }

    public void SetSpellCard(SpellCard newCard)
    {
        currentSpell = newCard;
    }

    public void DestroySpellCard()
    {
        Destroy(currentSpell.gameObject);
    }
}
