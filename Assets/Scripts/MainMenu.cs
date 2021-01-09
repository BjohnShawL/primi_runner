using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class MainMenu : MonoBehaviour
{
    private MetaManagement meta;
    public event Action<string> Loader;
    public event Action<int> LoadByInt;
    public event Action Quiter;

    public bool isPauseMenu = true;
    [CanBeNull] public GameObject WinPanel;
    [CanBeNull] public GameObject QuitPanel;

    // Start is called before the first frame update
    void Start()
    {
        meta = FindObjectOfType<MetaManagement>();
        meta.MenuReady();
        
        var sceneDict = meta.sc
                .Scenes.ToDictionary(x => x.Name, x => x.Index);
        if (SceneManager.GetActiveScene().buildIndex == sceneDict["Menu"])
        {
            isPauseMenu = false;
            Time.timeScale = 1;
        }
    }

    public void PlayGame()
    {
        Loader?.Invoke("Play");
    }

    public void WinGame()
    {
        Loader?.Invoke("Win");
    }
    public void LoseGame()
    {
        Loader?.Invoke("Lose");
    }
    public void WinLevel()
    {
        Debug.Log("Level won");
        LoadByInt?.Invoke(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void QuitGame()
    {
        Quiter?.Invoke();
    }

    public void BackToMenu()
    {
        Loader?.Invoke("Menu");
    }

    public void WinSwitch()
    {
        // var b = QuitPanel.GetComponent<GameObject>();
        // var winner = WinPanel.GetComponent<GameObject>();
        //
        // winner.SetActive(true);
        QuitPanel.SetActive(false);
        WinPanel.SetActive(true);
    }
}
