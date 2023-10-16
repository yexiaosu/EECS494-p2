using System.Collections;
using System.Collections.Generic;
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

        if (GameObject.Find("PlayerManager"))
        {
            handCards = new List<int>(GameObject.Find("PlayerManager").GetComponent<PlayerManager>().handCards);
        }
        if (GameObject.Find("LevelManager").GetComponent<LevelManager>().currLevel == 0)
        {
            // initial cards
            handCards.Add(0);
            handCards.Add(1);
            handCards.Add(5);
            handCards.Add(6);
        }
        if (GameObject.Find("PlayerManager").GetComponent<PlayerManager>().nextPowerUp == 2)
        {
            // draw two new cards, until 10
            int i = 0;
            while (handCards.Count < 10 && i < 2)
            {
                handCards.Add(Draw());
                i++;
            }
        }

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
    }

    public void RemoveHandCards(int cardIndex)
    {
        handCards.RemoveAt(cardIndex);
    }
}

