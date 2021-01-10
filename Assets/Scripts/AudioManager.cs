using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FMODUnity;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [CanBeNull] private PlayerController player;
    public SceneIndex si;
    private Dictionary<string, int> sceneDict;

    // Start is called before the first frame update
    void Start()
    {
        sceneDict = si
            .Scenes.ToDictionary(x => x.Name, x => x.Index);


    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > sceneDict["Menu"] && player == null)
        {
            player = FindObjectOfType<PlayerController>();
            if (player)
            {
                PlayerSetup();
            }

        }
    }

    private void PlayerSetup()
    {
        player.Jump += Player_Jump;
        player.Bounce += Player_Bounce;
    }

    private void Player_Bounce(PlayerController obj)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Bounce");
    }

    private void Player_Jump(PlayerController obj)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Jump");

    }



}
