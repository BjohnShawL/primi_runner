using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 20f;

    [SerializeField] [Range(100,600)] private float deathDelay;

    [SerializeField] [Range(2,5)]private float fallMultiplier; 
    [SerializeField] [Range(2, 5)] private float shortJumpMultiplier;
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;
    private Animator anim;

    private List<Action<blockType>> blockActions; 

    public bool isGrounded;
    public Transform GroundCheckTransform;
    public float CheckRadius;
    public LayerMask GroundLayerMask;
    private BlockInteraction checker;

    public event Action<PlayerController> PlayerDeath;
    public event Action<Vector2> PlayerSpawn;
    


    // Start is called before the first frame update
    void Start()
    {
        //blockActions = new List<Task>(){HandleBlockInteraction(new blockType()),HandleBouncer(new blockType()), HandleDamager(new blockType())};

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        checker = GetComponent<BlockInteraction>();
        //foreach (var blockAction in blockActions)
        //{
             //checker.BlockAction += blockAction;
        //}

        checker.BlockAction += async (s) => await HandleBlockInteraction(s);
        checker.BlockAction += async (s) => await HandleBouncer(s);
        checker.BlockAction += async (s) => await HandleDamager(s);
        checker.BlockAction += async (s) => await HandlePhaser(s);

        //Debug.Log(checker);
    }

    private async Task HandleBlockInteraction(blockType obj)
    {
        Debug.Log("I'm still standing on " + obj.BlockType.ToString() + ", but this time from an event"); 
    }

    private async Task HandleBouncer(blockType block)
    {
        if (block.BlockType == blockType.type.Bouncer)
        {
            var baseJump = jumpForce;
            jumpForce *= 2;
            PlayerJump();
            jumpForce = baseJump;
        }
        else
        {
            Task.Yield();
        }
    }

    private async Task HandleDamager(blockType block)
    {
        if (block.BlockType == blockType.type.Damager)
        {
            var baseJump = jumpForce;
            jumpForce *= .7f;
            PlayerJump();
            await Task.Run(()=> jumpForce = baseJump);
            await Task.Delay(500);
            PlayerDeath?.Invoke(this);
        }
    }

    private async Task HandlePhaser(blockType block)
    {
        if (block.BlockType == blockType.type.Phaser)
        {
            var spawnPoint = block.pairedBlock.gameObject.transform.position;
            spawnPoint.y += 1;
            PlayerSpawn?.Invoke(spawnPoint);
            PlayerDeath?.Invoke(this);
        }

        else
        {
            Task.Yield();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheckTransform.position, CheckRadius, GroundLayerMask);
        anim.SetBool("grounded",isGrounded);
        anim.SetBool("jumping",!isGrounded);
        var move = Input.GetAxisRaw("Horizontal");
        PlayerMove(move);
        //
        // if (isGrounded && Input.GetButtonDown("Jump"))
        // {
        //    //rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
        //    rb.velocity = Vector2.up * jumpForce;
        //    anim.SetBool("jumping",true);
        //    
        //
        // }
        // anim.SetFloat("JumpingVel", rb.velocity.y);
    }

    void Update()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            //rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            PlayerJump();
            

        }
        if (isGrounded && Input.GetKeyDown(KeyCode.DownArrow))
        {
            
            checker.CheckGroundType(GroundCheckTransform, GroundLayerMask);
            
        }

        if (rb.velocity.y<0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier -1) * Time.deltaTime;
        }
        anim.SetFloat("JumpingVel", rb.velocity.y);
    }

    public void PlayerJump()
    {
        rb.velocity = Vector2.up * jumpForce;
        anim.SetBool("jumping", true);
    }

    public void PlayerMove(float move)
    {
        anim.SetFloat("Blend",move);
        rb.velocity= new Vector2(move * moveSpeed, rb.velocity.y);
       
    }

     

}
