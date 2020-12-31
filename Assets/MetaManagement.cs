using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaManagement : MonoBehaviour
{
    public int n_once;
    [SerializeField]private MainMenu MainMenu;
    [CanBeNull] private GameManagement gameManagement;

    void Awake()
    {

       
        //TODO: Think this is being intantiated twice
        //Debug.Log("Hey, I exist, and I only exist once - or do I?");
        DontDestroyOnLoad(this);
        //MenuSetUp();
        if (SceneManager.GetActiveScene().buildIndex==0)
        {
            StartCoroutine(LoadSceneAsynchronous());
            
        }
        
    }

    IEnumerator LoadSceneAsynchronous()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while (!asyncLoad.isDone)
        {
            
            yield return null;
        }
    }

    private void MenuSetUp()
    {
        MainMenu = FindObjectOfType<MainMenu>();
        gameManagement = FindObjectOfType<GameManagement>();
        Debug.Log("Working");
        MainMenu.Loader += SceneLoad;
        MainMenu.Quiter += GameQuit;
        if (gameManagement)
        {
            Debug.Log("Pause should be working");
            MainMenu.gameObject.SetActive(false);
            gameManagement.Pause += GamePause;
        }
    }

    private void SceneLoad(int index)
    {
        SceneManager.LoadScene(index);
        //MenuSetUp();
    }

    private void GameQuit()
    {
        Application.Quit();
    }

    private void GamePause()
    {
        var isPaused = Convert.ToBoolean(Time.timeScale);
        Time.timeScale = Convert.ToSingle(!isPaused);
        MainMenu.gameObject.SetActive(!MainMenu.gameObject.activeInHierarchy);
    }
    //this appears to be the only way I can get the main menu to load before my MenuSetup method is called.
    //It's a tight coupling, and I hate it - but I can refactor when it's working.
    public void MenuReady()
    {
        MenuSetUp();
    }
}
