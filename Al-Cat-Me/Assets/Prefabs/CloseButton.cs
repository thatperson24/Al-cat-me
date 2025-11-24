using UnityEngine;
using TMPro;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private GameObject Cards;
    private bool cardsShown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardsShown = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCards()
    {

        cardsShown = !cardsShown;
        Cards.SetActive(cardsShown);
    }
}
