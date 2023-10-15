using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private GameObject handCards;
    private GameObject deck;
    private Subscription<CardDropEvent> cardDropEventSubscription;

    // Start is called before the first frame update
    void Start()
    {
        handCards = GameObject.Find("HandCards");
        deck = GameObject.Find("Deck");
        cardDropEventSubscription = EventBus.Subscribe<CardDropEvent>(_OnCardDrop);
    }

    private void _OnCardDrop(CardDropEvent e)
    {
        GameObject card = e.card;
        Vector3 initPos = e.initPos;

        // The card was in hand before drag
        if (card.transform.parent.parent.name == "HandCards")
        {
            // Check if the destination is in a slot
            foreach (Transform slot in deck.transform)
            {
                if (slot.gameObject.GetComponent<BoxCollider2D>().bounds.Contains(card.transform.position))
                {
                    // The card is placed into a slot of the deck
                    GameObject previous = slot.GetChild(0).gameObject;
                    card.transform.position = previous.transform.position;
                    card.transform.parent = slot;
                    Destroy(previous);
                    handCards.GetComponent<HandCards>().RemoveHandCards(card.GetComponent<CardDetail>().indexInHand);
                    if (!previous.name.StartsWith("Empty"))
                    {
                        // The slot was not empty before
                        handCards.GetComponent<HandCards>().AddHandCards(previous.GetComponent<CardDetail>().cardID);
                    }
                    // update the deck
                    deck.GetComponent<Deck>().UpdateDeck(card.GetComponent<CardDetail>().cardID, slot.GetComponent<DeckSlot>().deckIndex);
                    return;
                }
            }
        }
        // The card was in deck before drag
        else
        {
            // Check if the destination is in a slot
            foreach (Transform slot in deck.transform)
            {
                if (slot.gameObject.GetComponent<BoxCollider2D>().bounds.Contains(card.transform.position))
                {
                    // The card is placed into a slot of the deck
                    GameObject previous = slot.GetChild(0).gameObject;
                    previous.transform.parent = card.transform.parent;
                    card.transform.parent = slot;
                    card.transform.position = previous.transform.position;
                    previous.transform.position = initPos;
                    // update the deck
                    deck.GetComponent<Deck>().UpdateDeck(card.GetComponent<CardDetail>().cardID, slot.GetComponent<DeckSlot>().deckIndex);
                    if (previous.GetComponent<CardDetail>())
                        deck.GetComponent<Deck>().UpdateDeck(previous.GetComponent<CardDetail>().cardID, previous.transform.parent.GetComponent<DeckSlot>().deckIndex);
                    else
                        deck.GetComponent<Deck>().UpdateDeck(-1, previous.transform.parent.GetComponent<DeckSlot>().deckIndex);
                    return;
                }
            }
            // check if the destination is in hand
            if (handCards.GetComponent<BoxCollider2D>().bounds.Contains(card.transform.position))
            {
                // The card is placed into hand
                GameObject emptySlot = (GameObject)Instantiate(Resources.Load("EmptyCard"), initPos, Quaternion.identity);
                emptySlot.transform.parent = card.transform.parent;
                handCards.GetComponent<HandCards>().AddHandCards(card.transform.gameObject.GetComponent<CardDetail>().cardID);
                // update the deck
                deck.GetComponent<Deck>().UpdateDeck(-1, emptySlot.transform.parent.GetComponent<DeckSlot>().deckIndex);
                Destroy(card);
            }
        }
        // If the destination is not a valid target, reset the card's position
        card.transform.position = initPos;
    }
}
