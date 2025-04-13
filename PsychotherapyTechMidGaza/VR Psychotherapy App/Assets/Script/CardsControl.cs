using System.Collections.Generic;
using UnityEngine;

public class CardsControl : MonoBehaviour
{
    [SerializeField] GameObject[] Cards;
    private bool flipped = false;
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

    public void FlipAllCards()
    {
        foreach (GameObject cardGroup in Cards)
        {
            CardManager[] cardManagers = cardGroup.GetComponentsInChildren<CardManager>();
            foreach (CardManager manager in cardManagers)
            {
                manager.FlipCard();
            }
        }

        flipped = !flipped;
    }
}
