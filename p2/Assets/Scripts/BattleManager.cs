using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject playerStatusNumber;
    public GameObject enemyStatusNumber;
    public GameObject winOrLose;
    public int maxTurns = 20;
    public Text turn;

    private List<int> playerDeck;
    private List<int> enemyDeck;
    private int currPlayerCardIdx = 0;
    private int lastPlayerCardIdx = -1;
    private int currEnemyCardIdx = 0;
    private int lastEnemyCardIdx = -1;
    private List<int> playerRune = new List<int>();
    private List<int> enemyRune = new List<int>();
    private List<int> playerSkip = new List<int>();
    private List<int> enemySkip = new List<int>();
    private GameObject player;
    private GameObject enemy;
    private int currTurn = 1;
    private PlayerManager pm;

    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.Find("HasCards").GetComponent<PlayerManager>();
        playerDeck = new List<int>(pm.playerDeck);
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(1);
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        // deal with power ups
        int powerUp = pm.nextPowerUp;
        if (powerUp == 0)
        {
            // max health +500
            pm.UpdateMaxHealth(500);

        } else if (powerUp == 1)
        {
            // max manaAmp +50
            pm.UpdateManaAmp(50);
        }
        player.GetComponent<HealthSystemForDummies>().AddToMaximumHealth(pm.maxHealth - 1000);
        player.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(pm.maxHealth - 1000);
        player.GetComponent<Attributes>().UpdateManaAmp(pm.manaAmp);
        enemyDeck = new List<int>(GameObject.Find("EnemyDeck").GetComponent<Deck>().deckCards);
        enemyDeck.RemoveAll(item => item == -1);
        //playerDeck = new List<int>(GameObject.Find("PlayerDeck").GetComponent<Deck>().deckCards);
        //playerDeck.RemoveAll(item => item == -1);
        turn.text = currTurn.ToString();
        StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        // player use card
        currPlayerCardIdx = UseCard(playerDeck, currPlayerCardIdx, playerSkip, playerRune, player, enemy, playerStatusNumber, enemyStatusNumber);
        Transform currCard = GameObject.Find("PlayerDeck").transform.GetChild(currPlayerCardIdx);
        currCard.position = new Vector3(currCard.position.x, currCard.position.y + 1, currCard.position.z);
        if (lastPlayerCardIdx >= 0)
        {
            Transform lastCard = GameObject.Find("PlayerDeck").transform.GetChild(lastPlayerCardIdx);
            lastCard.position = new Vector3(lastCard.position.x, lastCard.position.y - 1, lastCard.position.z);
        }
        lastPlayerCardIdx = currPlayerCardIdx;
        currPlayerCardIdx++;
        if (currPlayerCardIdx >= playerDeck.Count)
        {
            currPlayerCardIdx = 0;
        }
        yield return new WaitForSeconds(1);
        currTurn++;
        turn.text = currTurn.ToString();
        if (currTurn > maxTurns)
            Lose();
        else if (enemy.GetComponent<HealthSystemForDummies>().IsAlive)
        {
            StartCoroutine(EnemyTurn());
        }
        else
            Win();
    }

    private IEnumerator EnemyTurn()
    {
        // enemy use card
        currEnemyCardIdx = UseCard(enemyDeck, currEnemyCardIdx, enemySkip, enemyRune, enemy, player, enemyStatusNumber, playerStatusNumber);
        Transform currCard = GameObject.Find("EnemyDeck").transform.GetChild(currEnemyCardIdx);
        currCard.position = new Vector3(currCard.position.x, currCard.position.y + 1, currCard.position.z);
        if (lastEnemyCardIdx >= 0)
        {
            Transform lastCard = GameObject.Find("EnemyDeck").transform.GetChild(lastEnemyCardIdx);
            lastCard.position = new Vector3(lastCard.position.x, lastCard.position.y - 1, lastCard.position.z);
        }
        lastEnemyCardIdx = currEnemyCardIdx;
        currEnemyCardIdx++;
        if (currEnemyCardIdx >= enemyDeck.Count)
        {
            currEnemyCardIdx = 0;
        }
        yield return new WaitForSeconds(1);
        currTurn++;
        turn.text = currTurn.ToString();
        if (currTurn > maxTurns)
            Lose();
        else if (player.GetComponent<HealthSystemForDummies>().IsAlive)
            StartCoroutine(PlayerTurn());
        else
            Lose();
    }

    private int UseCard(List<int> deck, int cardIndex, List<int> skip, List<int> rune, GameObject self, GameObject enemy, GameObject selfStatus, GameObject enemyStatus)
    {
        int manaAmp = self.GetComponent<Attributes>().manaAmp;
        // resolve current effects
        self.GetComponent<Attributes>().UpdateShieldEveryTurn();
        // rune
        foreach (int ID in rune)
        {
            Card runeCard = CardsInfo.GetCard(ID);
            // Heal
            Heal(manaAmp, runeCard.Heal, self, selfStatus);
        }
        // skip cards
        while (skip.Exists(item => item == cardIndex))
        {
            cardIndex++;
            if (cardIndex >= deck.Count)
            {
                cardIndex = 0;
            }
        }
        // get card
        int cardID = deck[cardIndex];
        Card card = CardsInfo.GetCard(cardID);
        if (card.IsRune)
        {
            // The card is rune, should be removed from the flow after first use
            rune.Add(card.ID);
            skip.Add(cardIndex);
        }
        else
        {
            // Check cost
            if (self.GetComponent<Attributes>().mana < card.Cost)
            {
                // if mana is not enough, add 1 mana and skip the card
                self.GetComponent<Attributes>().UpdateMana(1);
                return cardIndex;
            }
            self.GetComponent<Attributes>().UpdateMana(-1 * card.Cost);
            // The card should take effect for card.Times times
            for (int i = 0; i < card.Times; i++)
            {
                // Make damage
                if (card.Damage > 0)
                    Damage(manaAmp, card.Damage, enemy, enemyStatus);
                // Heal
                if (card.Heal > 0)
                    Heal(manaAmp, card.Heal, self, selfStatus);
                // Shield
                if (card.Sheid > 0)
                    Shield(manaAmp, card.Sheid, self, selfStatus);
                // Mana
                if (card.Mana > 0)
                    Mana(card.Mana, self, selfStatus);
            }
        }
        return cardIndex;
    }

    private void Heal(int manaAmp, int amount, GameObject target, GameObject status)
    {
        target.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(amount * manaAmp);
        GameObject statusNumber = (GameObject)Instantiate(Resources.Load("HealNumber"), status.transform.position + new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0), Quaternion.identity);
        statusNumber.GetComponent<Text>().text = "+" + (amount * manaAmp).ToString();
        statusNumber.transform.SetParent(status.transform);
    }

    private void Damage(int manaAmp, int amount, GameObject target, GameObject status)
    {
        // check shield
        if (target.GetComponent<Attributes>().shield > 0)
            target.GetComponent<Attributes>().UpdateShield(-1 * amount * manaAmp);
        else
            target.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(-1 * amount * manaAmp);
        GameObject statusNumber = (GameObject)Instantiate(Resources.Load("DamageNumber"), status.transform.position + new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0), Quaternion.identity);
        statusNumber.GetComponent<Text>().text = "-" + (amount * manaAmp).ToString();
        statusNumber.transform.SetParent(status.transform);
    }

    private void Shield(int manaAmp, int amount, GameObject target, GameObject status)
    {
        target.GetComponent<Attributes>().UpdateShield(amount * manaAmp);
        GameObject statusNumber = (GameObject)Instantiate(Resources.Load("ShieldNumber"), status.transform.position + new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0), Quaternion.identity);
        statusNumber.GetComponent<Text>().text = "+" + (amount * manaAmp).ToString();
        statusNumber.transform.SetParent(status.transform);
    }

    private void Mana(int amount, GameObject target, GameObject status)
    {
        target.GetComponent<Attributes>().UpdateMana(amount);
        GameObject statusNumber = (GameObject)Instantiate(Resources.Load("ManaNumber"), status.transform.position + new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0), Quaternion.identity);
        statusNumber.GetComponent<Text>().text = "+" + amount.ToString();
        statusNumber.transform.SetParent(status.transform);
    }

    private void Lose()
    {
        winOrLose.SetActive(true);
        winOrLose.transform.GetChild(0).GetComponent<Text>().text = "You lose!";
        winOrLose.transform.GetChild(2).gameObject.SetActive(true);
        winOrLose.transform.GetChild(3).gameObject.SetActive(false);
        winOrLose.transform.GetChild(4).gameObject.SetActive(false);
    }

    private void Win()
    {
        winOrLose.SetActive(true);
        winOrLose.transform.GetChild(0).GetComponent<Text>().text = "You win!";
        winOrLose.transform.GetChild(2).gameObject.SetActive(false);
        if (GameObject.Find("LevelManager").GetComponent<LevelManager>().currLevel == 21)
        {
            winOrLose.transform.GetChild(4).gameObject.SetActive(true);
            winOrLose.transform.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            winOrLose.transform.GetChild(4).gameObject.SetActive(false);
            winOrLose.transform.GetChild(3).gameObject.SetActive(true);
        }
        EventBus.Publish<BattleWinEvent>(new BattleWinEvent());
    }
}

public class BattleWinEvent
{
    public BattleWinEvent() {  }
}
