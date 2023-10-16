using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int numLevels = 22;
    public Sprite normalLevel;
    public Sprite highlightLevel;
    public Sprite healthLevel;
    public Sprite manaAmpLevel;
    public Sprite cardsLevel;
    public int currLevel = -1;
    public List<int> validNextLevels = new List<int> { 0 };
    public static GameObject levelManagerInstance;

    private GameObject levels;
    private List<List<int>> levelArray = new List<List<int>>
    {
        new List<int>{0},
        new List<int>{1, 2},
        new List<int>{3, 4, 5},
        new List<int>{6, 7, 8},
        new List<int>{9, 10, 11, 12},
        new List<int>{13, 14, 15, 16},
        new List<int>{17, 18},
        new List<int>{19, 20},
        new List<int>{21},
    };
    private List<int> powerUps = new List<int>(); // 0: maxHealth +500, 1: manaAmp +50, 2: 2 cards
    private Subscription<ClickLevelEvent> clickLevelEventSubscription;
    public int currLevelsCol = -1;
    private List<int> path = new List<int>();

    void Awake()
    {
        // init levels
        levels = GameObject.Find("Canvas/LevelMap");
        for (int i = 0; i < numLevels; i++)
        {
            powerUps.Add(Random.Range(0, 3));
        }
        clickLevelEventSubscription = EventBus.Subscribe<ClickLevelEvent>(_OnClickLevl);
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (levelManagerInstance == null)
            levelManagerInstance = gameObject;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level")
        {
            levelManagerInstance.GetComponent<LevelManager>().UpdateLevelMap();
        }
    }

    public int GetLevelPowerUp(int levelIdx)
    {
        return powerUps[levelIdx];
    }

    public void UpdateLevelMap()
    {
        levels = GameObject.Find("Canvas/LevelMap");
        for (int i = 0; i < levels.transform.childCount; i++)
        {
            GameObject levelsCol = levels.transform.GetChild(i).gameObject;
            for (int j = 0; j < levelsCol.transform.childCount; j++)
            {
                GameObject level = levelsCol.transform.GetChild(j).gameObject;
                int levelIdx = level.GetComponent<Level>().LevelIdx;
                if (i < currLevelsCol || path.Contains(levelIdx))
                {
                    level.GetComponent<Image>().color = new Color(level.GetComponent<Image>().color.r, level.GetComponent<Image>().color.g, level.GetComponent<Image>().color.b, 0.5f);
                    level.GetComponent<Image>().sprite = normalLevel;
                    level.GetComponent<Level>().disabled = true;
                }
                else if (validNextLevels.Contains(levelIdx))
                {
                    int powerUp = powerUps[levelIdx];
                    switch (powerUp)
                    {
                        case 0:
                            level.GetComponent<Image>().sprite = healthLevel;
                            break;
                        case 1:
                            level.GetComponent<Image>().sprite = manaAmpLevel;
                            break;
                        case 2:
                            level.GetComponent<Image>().sprite = cardsLevel;
                            break;
                        default:
                            level.GetComponent<Image>().sprite = healthLevel;
                            break;
                    }
                }
                else if (levelIdx == currLevel)
                {
                    level.GetComponent<Image>().sprite = highlightLevel;
                }
                else
                {
                    level.GetComponent<Image>().sprite = normalLevel;
                }
            }
        }
    }

    private void _OnClickLevl(ClickLevelEvent e)
    {
        int targetLevel = e.levelIdx;
        if (validNextLevels.Contains(targetLevel))
        {
            path.Add(currLevel);
            if (currLevelsCol == -1)
            {
                // move to the first level
                currLevel = targetLevel;
                currLevelsCol++;
            }
            if (levelArray[currLevelsCol].Contains(targetLevel))
            {
                // move to siblings
                currLevel = targetLevel;
            }
            else
            {
                // move forward
                currLevel = targetLevel;
                currLevelsCol++;
            }
            if (path.Count > 2)
            {
                // strengthen enemies
                EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
                if (path.Count % 2 == 1)
                    em.UpdateMaxHealth(500);
                else
                    em.UpdateManaAmp(25);
            }
            if (path.Count > 4)
            {
                EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
                em.deckStrength++;
            }
            if (path.Count > 7)
            {
                EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
                em.deckStrength++;
            }
            validNextLevels.Clear();
            int idx = levelArray[currLevelsCol].FindIndex(item => item == targetLevel);
            GameObject curr = levels.transform.GetChild(currLevelsCol).GetChild(idx).gameObject;
            for (int i = 0; i < levelArray[currLevelsCol].Count; i++)
            {
                if (i == idx)
                    continue;
                GameObject target = levels.transform.GetChild(currLevelsCol).GetChild(i).gameObject;
                if (isNeighbour(target, curr))
                    validNextLevels.Add(levelArray[currLevelsCol][i]);
            }
            if (currLevelsCol + 1 < levelArray.Count)
            {
                for (int i = 0; i < levelArray[currLevelsCol + 1].Count; i++)
                {
                    GameObject target = levels.transform.GetChild(currLevelsCol + 1).GetChild(i).gameObject;
                    if (isNeighbour(target, curr))
                        validNextLevels.Add(levelArray[currLevelsCol + 1][i]);
                }
            }
            UpdateLevelMap();
            SceneManager.LoadScene("Preparation");
        }
    }

    private bool isNeighbour(GameObject pos1, GameObject pos2)
    {
        return Vector2.Distance(pos1.transform.position, pos2.transform.position) < 150f;
    }
}
