using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<int> deckCards = new List<int>(8);

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < deckCards.Count; i++)
        {
            deckCards[i] = -1; // -1: empty card
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
