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

    private bool isInteracting = false;

    [SerializeField] private float speedBoostMultiplier;
    [SerializeField] private float SloMoMultiplier;

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
    public event Action<PlayerController, float> PlayerTP; 
    public event Action<Vector2> PlayerSpawn;
    public event Action<int> AddTime;
    public event Action<PlayerController> Jump;
    public event Action<PlayerController> Bounce;
    public bool hasWon = false;


    // Start is called before the first frame update
    void Start()
    {
        
        

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        checker = GetComponent<BlockInteraction>();


        checker.BlockAction += async (s) => await HandleBlockInteraction(s);
        checker.BlockAction += async (s) => await HandleBouncer(s);
        checker.BlockAction += async (s) => await HandleDamager(s);
        checker.BlockAction += async (s) => await HandlePhaser(s);
        checker.BlockAction += async (s) => await HandleSpeedster(s);
        checker.BlockAction += async (s) => await HandleSloMo(s);
        checker.BlockAction += async (s) => await HandlePoints(s);


    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheckTransform.position, CheckRadius, GroundLayerMask);
        anim.SetBool("grounded", isGrounded);
        anim.SetBool("jumping", !isGrounded);
        var move = Input.GetAxisRaw("Horizontal");
        PlayerMove(move);

    }

    void Update()
    {
        BetterJump();

        if (isGrounded && Input.GetButtonDown("Jump"))
        {

            PlayerJump();
            Jump?.Invoke(this);

        }
        //  && Input.GetKeyDown(KeyCode.DownArrow) - this turns us manual
        if (isGrounded && !isInteracting)
        {

            checker.CheckGroundType(GroundCheckTransform, GroundLayerMask);

        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        anim.SetFloat("JumpingVel", rb.velocity.y);
    }
    private async Task HandleBlockInteraction(blockType obj)
    {
        //Debug.Log("I'm still standing on " + obj.BlockType.ToString() + ", but this time from an event"); 
        await Task.Yield();
    }

    private async Task HandlePoints(blockType block)
    {
        if (block.BlockType == blockType.type.Points)
        {
            var tta = block.pointsValue;
            AddTime?.Invoke(tta);
            block.pointsValue = 0;
        }

        await Task.Yield();
    }

    private async Task HandleBouncer(blockType block)
    {
        if (block.BlockType == blockType.type.Bouncer)
        {
            var baseJump = jumpForce;
            jumpForce *= 2;
            PlayerJump();
            Jump?.Invoke(this);
            Bounce?.Invoke(this);
            jumpForce = baseJump;
        }
        else
        {
            await Task.Yield();
        }
    }
    private async Task HandleSpeedster(blockType block)
    {
        if (block.BlockType == blockType.type.Speedster)
        {
            isInteracting = !isInteracting;
            var baseSpeed = moveSpeed;
            moveSpeed *= speedBoostMultiplier;
            await Task.Delay(1000);
            moveSpeed = baseSpeed;
            isInteracting = !isInteracting;

        }
        else
        {
            await Task.Yield();
        }
    }
    private async Task HandleSloMo(blockType block)
    {
        if (block.BlockType == blockType.type.Slomo)
        {
            isInteracting = !isInteracting;
            var baseSpeed = moveSpeed;
            moveSpeed *= SloMoMultiplier;
            await Task.Delay(1000);
            moveSpeed = baseSpeed;
            isInteracting = !isInteracting;
        }
        else
        {
            await Task.Yield();
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
            var death = Mathf.FloorToInt(deathDelay);
            await Task.Delay(death);
            PlayerDeath?.Invoke(this);
        }
    }

    private async Task HandlePhaser(blockType block)
    {
        if (block.BlockType == blockType.type.Phaser && Input.GetKeyDown(KeyCode.DownArrow))
        {
            var spawnPoint = block.pairedBlock.gameObject.transform.position;
            spawnPoint.y += 1;
            PlayerSpawn?.Invoke(spawnPoint);
            PlayerTP?.Invoke(this,600f);
        }

        else
        {
           await Task.Yield();
        }
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

    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier -1) * Time.deltaTime;
        }
    } 

}
