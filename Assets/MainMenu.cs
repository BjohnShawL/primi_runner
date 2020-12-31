using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int MenuIndex = 1;
    [SerializeField] private int PlayIndex = 2;

    private MetaManagement meta;

    public event Action<int> Loader;
    public event Action Quiter;

    // Start is called before the first frame update
    void Start()
    {
        meta = FindObjectOfType<MetaManagement>();
        meta.MenuReady();
        if (SceneManager.GetActiveScene().buildIndex == MenuIndex)
        {
            Time.timeScale = 1;
        }
    }

    public void PlayGame()
    {
        Loader?.Invoke(PlayIndex);
    }

    public void QuitGame()
    {
        Quiter?.Invoke();
    }

    public void BackToMenu()
    {
        Loader?.Invoke(MenuIndex);
    }
}
