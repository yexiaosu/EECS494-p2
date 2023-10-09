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
        Destroy(GameObject.Find("HasCards"));
    }
}
