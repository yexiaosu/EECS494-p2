using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<int> deckCards = new List<int>(8);
    public bool isPlayer = true;

    private List<GameObject> DeckCardsObjects = new List<GameObject>();
    private List<Card> cards = CardsInfo.GetCards();

    // Start is called before the first frame update
    void Start()
    {
        // init deck cards
        if (isPlayer)
        {
            if (GameObject.Find("HasCards"))
            {
                deckCards = new List<int>(GameObject.Find("HasCards").GetComponent<PlayerManager>().playerDeck);
            }
        }
        else
        {
            deckCards = new List<int>(CardsInfo.GetEnemyDeck());
        }
        for (int i = deckCards.Count; i < 8; i++)
        {
            deckCards.Add(-1); // -1: empty card
        }

        // init deck cards gameobjects
        int deckSlotIndex = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<DeckSlot>().deckIndex = deckSlotIndex;
            if (deckCards[deckSlotIndex] != -1)
            {
                // The slot is not empty
                string cardName = cards[deckCards[deckSlotIndex]].Name;
                GameObject currCardPos = child.GetChild(0).gameObject;
                GameObject currCard = (GameObject)Instantiate(Resources.Load(cardName), currCardPos.transform.position, Quaternion.identity);
                Destroy(currCard.GetComponent<DragDrop>());
                currCard.transform.localScale = currCard.transform.localScale;
                currCard.transform.parent = child;
                Destroy(currCardPos);
            }
            DeckCardsObjects.Add(child.gameObject);
            deckSlotIndex++;
        }
    }

    public void UpdateDeck(int cardID, int deckIndex)
    {
        deckCards[deckIndex] = cardID;
    }
}
