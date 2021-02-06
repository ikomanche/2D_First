using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{    
    //Start() variables
    private Rigidbody2D rb;     //rb created at Unity. We told that rb is the rigidbody which we have created before in Unity.
    private Animator anim;        
    private Collider2D coll;    //BoxCollider object in Unity    

    //public GameObject ui;
    
    
    //FSM
    private enum State { idle, running, jumping, falling, hurt, climb }
    private State state = State.idle;

    //Ladder Variables
    [HideInInspector]public bool canClimb = false;
    [HideInInspector] public bool bottomLader = false;
    [HideInInspector] public bool topLadder = false;
    public Ladder ladder;
    private float naturalGravity;
    [SerializeField] private float climbSpeed = 3f;


    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 7f;    
    [SerializeField] private float Jumpforce = 17f;    
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource cherrySound;
    [SerializeField] private AudioSource gemSound;
    [SerializeField] private AudioSource mushSound;
    [SerializeField] private float Hurtforce = 15f;
    [SerializeField] private AudioSource gameoverSound;
    private static bool isPaused = false;

    [SerializeField]private GameObject Pause;

    private void Start()
    {   //initializing private variables...
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        PermanentUI.perm.healthText.text = PermanentUI.perm.Health.ToString();
        naturalGravity = 3.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        //ui.SetActive(true);
        if (state == State.climb)
        {
            Climb();
        }
        else if (state != State.hurt)
        {
            Movement();
        }                

        AnimationState();    //Get the state
        anim.SetInteger("state", (int)state);   //perform the correct animation of the state

        if (Input.GetKey(KeyCode.P))
        {
            pauseGame();
        }
    }

    private void pauseGame()
    {
        if (isPaused)
        {
            resumeGame();
        }
        else
        {
            Pause.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
    }

    private void resumeGame()
    {
        Pause.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Collectable")
        {
            cherrySound.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries += 1;
            PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
        } 
        else if(collision.tag == "Gem")
        {
            gemSound.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.gems += 1;
            PermanentUI.perm.gemText.text = PermanentUI.perm.gems.ToString();
            PermanentUI.perm.Health += 1;
            PermanentUI.perm.healthText.text = PermanentUI.perm.Health.ToString();
        }
        else if(collision.tag == "PowerUp")
        {
            mushSound.Play();
            Destroy(collision.gameObject);
            Jumpforce = 20f;
            speed = 10f;
            GetComponent<SpriteRenderer>().color = Color.green;
            StartCoroutine(ResetPower());
        }
        else if(collision.tag == "Obstacle")
        {
            state = State.hurt;
            GetComponent<SpriteRenderer>().color = Color.red;
            CheckDeath();
            StartCoroutine(BackToNormalColor());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(state == State.falling)
            {
                enemy.JumpedOn();
                //Destroy(collision.gameObject);
                JumpedByEnemy();
            }
            else
            {
                state = State.hurt;                
                GetComponent<SpriteRenderer>().color = Color.red;
                CheckDeath();
                StartCoroutine(BackToNormalColor());
                
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    //
                    rb.velocity = new Vector2(-Hurtforce, rb.velocity.y);
                }
                else
                {
                    //
                    rb.velocity = new Vector2(Hurtforce, rb.velocity.y);
                }
            }

        }
    }

    private void CheckDeath()
    {
        PermanentUI.perm.Health -= 1;
        PermanentUI.perm.healthText.text = PermanentUI.perm.Health.ToString();
        if (PermanentUI.perm.Health <= 0)
        {
            gameoverSound.Play();
            StartCoroutine(goToScene());            
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");    //hDirection = Horizontal input of Unity. We access built-in Unity Input (Horizontal)

        if (canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;
        }

        //Move Right
        if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        //Move Left
        else if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //Jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
        state = State.jumping;
    }

    private void JumpedByEnemy()
    {
        rb.velocity = new Vector2(rb.velocity.x, Jumpforce+10);
        state = State.jumping;
    }

    private void AnimationState()
    {       

        if(state == State.climb)
        {
            Climb();
        }
        else if(state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;            
        }
        else
        {
            state = State.idle;
        }        
    }

    private void Footstep()
    {
        footstep.Play();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(10);
        Jumpforce = 17f;
        speed = 7f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator goToScene()
    {
        yield return new WaitWhile(() => gameoverSound.isPlaying);
        SceneManager.LoadScene("GameOver");
    }

    private IEnumerator BackToNormalColor()
    {
        yield return new WaitForSeconds(1.25f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Climb()
    {
        if (Input.GetButtonDown("Jump"))
        {                       
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = 3.5f;
            anim.speed = 1f;
            Jump();
            return;
        }

        float vDirection = Input.GetAxis("Vertical");

        if(vDirection > .1f && !topLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        else if(vDirection < -.1f && !bottomLader)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        else
        {
            anim.speed = 0f;
        }
    }
}
