using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockType : MonoBehaviour
{
    public blockType pairedBlock = null;
    public int pointsValue = 0;
    private Animator anim;
    public enum type
    {
        Base,Bouncer,Damager,Phaser,Points,Slomo,Speedster
    }

    public type BlockType;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // var _rand = Random.Range(0f, 60f);
        // anim.SetFloat("Offset", _rand);
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
    }
}

