using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDetail : MonoBehaviour
{
    public int cardID;
    public int indexInHand;

    private List<Card> cards = CardsInfo.GetCards();

    public Card getCardDetail()
    {
        return cards[cardID];
    }
}
