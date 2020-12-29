using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    [SerializeField] private float positionTimerMax;
    private float positionTimerRemaining;
    [SerializeField] private float kill_Y = -50f;
    private PlayerController player;
    private Vector2 spawnPos;
    public GameObject playerPrefab;
    private CinemachineVirtualCamera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        positionTimerRemaining = positionTimerMax;
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
            if (player.isGrounded)
            {
                spawnPos = player.gameObject.transform.position;
                spawnPos.y += 0.35f;
            }
        }

        if (player == null)
        {
            GetPlayer();
        }

        if (player.transform.position.y <= kill_Y)
        {
            KillPlayer(player);
        }
    }

    private void KillPlayer(PlayerController player)
    {
        Destroy(player.gameObject);
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