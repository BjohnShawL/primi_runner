using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using TMPro;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    [SerializeField] private float positionTimerMax;

    [SerializeField] private float gameTimeMax;
    private float gameTimeRemaining;
    private float positionTimerRemaining;
    [SerializeField] private float kill_Y = -50f;
    [SerializeField] private float win_Y = 120f;
    private PlayerController player;
    private Vector2 spawnPos;
    public GameObject playerPrefab;
    private CinemachineVirtualCamera cam;
    public TextMeshProUGUI ui;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        positionTimerRemaining = positionTimerMax;
        gameTimeRemaining = gameTimeMax;
        GetPlayer();
        spawnPos = player.gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (positionTimerRemaining > 0)
        {
            positionTimerRemaining -= Time.deltaTime;
        }
        else
        {
            positionTimerRemaining = positionTimerMax;
            if (player!=null)
            {
                if (player.isGrounded)
                {
                    spawnPos = player.gameObject.transform.position;
                    spawnPos.y += 0.35f;
                }
            }
        }

        if (gameTimeRemaining>=0)
        {
            gameTimeRemaining -= Time.deltaTime;
            var gtrInt = Mathf.FloorToInt(gameTimeRemaining);
            ui.text = gtrInt.ToString();
            WinCheck();
        }
        else
        {
            Debug.Log("Game over man, game over");
        }

        // if (player == null)
        // {
        //     GetPlayer();
        // }
        if (player != null)
        {
            if (player.transform.position.y <= kill_Y)
            {
                KillPlayer(player);
            }
        }

        
    }

    private void WinCheck()
    {
        if (player!=null)
        {
            if (player.gameObject.transform.position.y >= win_Y && player.isGrounded)
            {
                Debug.Log("You Win!");
            }
        }
        
    }

    private void KillPlayer(PlayerController player)
    {
        if (player!=null)
        {
            Destroy(player.gameObject);
            SpawnPlayer();
        }
    }

    private async Task KillPlayer(PlayerController player, float timedelay)
    {
        var _td = Mathf.FloorToInt(timedelay);
        Destroy(player.gameObject);
        await Task.Delay(_td);
        SpawnPlayer();
    }

    void GetPlayer()
    {
        var _player = FindObjectOfType<PlayerController>();
        if (_player.tag == "Player")
        {
            player = _player;
            player.PlayerDeath += KillPlayer;
            player.PlayerSpawn += SetPlayerSpawnPosition;
            player.PlayerTP += async (s,b) => await KillPlayer(s,b);
        }
    }

    void SpawnPlayer()
    {
        Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        GetPlayer();
        cam.Follow = player.transform;
    }

    void SetPlayerSpawnPosition(Vector2 position)
    {
        spawnPos = position;
    }
}