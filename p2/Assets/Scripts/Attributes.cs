using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attributes : MonoBehaviour
{
    public int mana = 0;
    public int manaAmp = 100;
    public int shield = 0;
    public Text manaAmpText;
    public Text shieldText;
    public Text manaText;

    // Start is called before the first frame update
    void Start()
    {
        manaAmpText.text = "Mana Amplification: " + manaAmp.ToString();
        shieldText.text = "Shield: " + shield.ToString();
        manaText.text = "Mana: " + mana.ToString();
    }

    public void UpdateMana(int amount)
    {
        mana = Mathf.Clamp(mana + amount, 0, int.MaxValue);
        manaText.text = "Mana: " + mana.ToString();
    }

    public void UpdateManaAmp(int amount)
    {
        manaAmp = Mathf.Clamp(manaAmp + amount, 0, int.MaxValue);
        manaAmpText.text = "Mana Amplification: " + manaAmp.ToString();
    }

    public void UpdateShield(int amount)
    {
        shield = Mathf.Clamp(shield + amount, 0, int.MaxValue);
        shieldText.text = "Shield: " + shield.ToString();
    }

    public void UpdateShieldEveryTurn()
    {
        shield = shield / 2;
        shieldText.text = "Shield: " + shield.ToString();
    }
}
