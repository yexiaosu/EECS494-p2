using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int maxHealth = 1000;
    public int manaAmp = 100;
    public int deckStrength = 0;
    private static GameObject sampleInstance;

    void Awake()
    {
        if (sampleInstance == null)
            sampleInstance = gameObject;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateManaAmp(int amount)
    {
        manaAmp = Mathf.Clamp(manaAmp + amount, 0, int.MaxValue);
    }

    public void UpdateMaxHealth(int amount)
    {
        maxHealth = Mathf.Clamp(maxHealth + amount, 0, int.MaxValue);
    }
}
