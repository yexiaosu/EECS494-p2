using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<int> handCards;
    public List<int> playerDeck;
    public int nextPowerUp = -1;
    public int maxHealth = 1000;
    public int manaAmp = 100;

    private static GameObject sampleInstance;
    private Subscription<BattleWinEvent> battleWinEventEventSubscription;

    // Start is called before the first frame update
    void Awake()
    {
        if (sampleInstance == null)
            sampleInstance = gameObject;
        else
            Destroy(gameObject);
        battleWinEventEventSubscription = EventBus.Subscribe<BattleWinEvent>(_OnBattleWin);
        DontDestroyOnLoad(gameObject);
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

    public void UpdateManaAmp(int amount)
    {
        manaAmp = Mathf.Clamp(manaAmp + amount, 0, int.MaxValue);
    }

    public void UpdateMaxHealth(int amount)
    {
        maxHealth = Mathf.Clamp(maxHealth + amount, 0, int.MaxValue);
    }

    private void _OnBattleWin(BattleWinEvent e)
    {
        LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        nextPowerUp = lm.GetLevelPowerUp(lm.currLevel);
    }
}
