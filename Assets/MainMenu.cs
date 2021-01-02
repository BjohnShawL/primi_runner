using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private MetaManagement meta;
    public event Action<string> Loader;
    public event Action Quiter;

    // Start is called before the first frame update
    void Start()
    {
        meta = FindObjectOfType<MetaManagement>();
        meta.MenuReady();
        var sceneDict = meta.sc
                .Scenes.ToDictionary(x => x.Name, x => x.Index);
        if (SceneManager.GetActiveScene().buildIndex == sceneDict["Menu"])
        {
            Time.timeScale = 1;
        }
    }

    public void PlayGame()
    {
        Loader?.Invoke("Play");
    }

    public void QuitGame()
    {
        Quiter?.Invoke();
    }

    public void BackToMenu()
    {
        Loader?.Invoke("Menu");
    }
}
