using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandCards : MonoBehaviour
{
    public List<int> handCards = new List<int>();

    private List<Card> cards = CardsInfo.GetCards();
    private List<GameObject> handCardsObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // init handcards gameobjects
        foreach (Transform child in transform)
        {
            handCardsObjects.Add(child.gameObject);
        }

        // draw four new cards when initialize
        handCards.Add(Draw());
        handCards.Add(Draw());
        handCards.Add(Draw());
        handCards.Add(Draw());
        handCards.Add(Draw());
        handCards.Add(Draw());
        handCards.Add(Draw());
        handCards.Add(Draw());
        handCards.Add(Draw());
        handCards.Add(Draw());

        UpdateHandCards();
    }

    public int Draw()
    {
        return Random.Range(0, cards.Count);
    }

    public void UpdateHandCards()
    {
        for (int i = 0; i < handCardsObjects.Count; i++)
        {
            if (i < handCards.Count)
            {
                GameObject currCardPos = handCardsObjects[i];
                int cardID = handCards[i];
                string cardName = cards[cardID].Name;
                GameObject currCard = (GameObject)Instantiate(Resources.Load(cardName), currCardPos.transform.position, Quaternion.identity);
                if (currCardPos.transform.childCount > 0)
                {
                    Destroy(currCardPos.transform.GetChild(0).gameObject);
                }
                currCard.transform.parent = currCardPos.transform;
                currCard.GetComponent<CardDetail>().indexInHand = i;
            }
            else
            {
                GameObject currCardPos = handCardsObjects[i];
                GameObject currCard = (GameObject)Instantiate(Resources.Load("NoCard"), currCardPos.transform.position, Quaternion.identity);
                if (currCardPos.transform.childCount > 0)
                {
                    Destroy(currCardPos.transform.GetChild(0).gameObject);
                }
                currCard.transform.parent = currCardPos.transform;
            }
        }
    }

    public void AddHandCards(int cardID)
    {
        handCards.Add(cardID);
        UpdateHandCards();
    }

    public void RemoveHandCards(int cardIndex)
    {
        handCards.RemoveAt(cardIndex);
        UpdateHandCards();
    }
}

