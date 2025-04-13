using System.Collections.Generic;
using UnityEngine;

public class CardsControl : MonoBehaviour
{
    [SerializeField] GameObject[] Cards;
    public void OnCardSync(string id)
    {
        foreach (var card in Cards)
        {
            if (card.GetComponent<CardManager>().cardData.ID == id)
            {
                card.GetComponent<CardManager>().FlipCard();
            }
        }
    }

    public void ShowAllCards()
    {
        foreach (var card in Cards)
        {
            card.SetActive(true);
        }
    }

    public void HideAllCards()
    {
        foreach (var card in Cards)
        {
            card.SetActive(false);
        }
    }
}
