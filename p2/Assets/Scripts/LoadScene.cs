using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string scene;

    public void Load()
    {
        SceneManager.LoadScene(scene);
    }

    public void Clear()
    {
        LevelManager.levelManagerInstance = null;
        Destroy(GameObject.Find("HasCards"));
        Destroy(GameObject.Find("LevelManager"));
    }
}
