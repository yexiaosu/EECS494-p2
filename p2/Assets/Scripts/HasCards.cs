using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasCards : MonoBehaviour
{
    public List<int> handCards;
    public List<int> playerDeck;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("HandCards"))
        {
            handCards = new List<int>(GameObject.Find("HandCards").GetComponent<HandCards>().handCards);
        }
        if (GameObject.Find("Deck"))
        {
            playerDeck = new List<int>(GameObject.Find("Deck").GetComponent<Deck>().deckCards);
            playerDeck.RemoveAll(item => item == -1);
        }
    }
}
