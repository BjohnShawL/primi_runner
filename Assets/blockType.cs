﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockType : MonoBehaviour
{
    public blockType pairedBlock = null;
    public int pointsValue = 0;
    public enum type
    {
        Base,Bouncer,Damager,Phaser,Points,Slomo,Speedster
    }

    public type BlockType;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}

