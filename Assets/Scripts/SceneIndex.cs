using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SceneIndex", order = 1)]
public class SceneIndex : ScriptableObject
{
    public List<(string Name, int Index)> Scenes = new List<(string, int)>
    {
        ("Menu",1),("Play",2),("Win",3),("Lose",4)


    };

}
