using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField]private float leftCap;
    [SerializeField]private float rightCap;

    [SerializeField] float jumpLength = 10f;
    [SerializeField] float jumpHeight = 15f;
    [SerializeField] LayerMask ground;
    private Collider2D coll;
    

    private bool facingLeft = true;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();        
    }

    private void Update()
    {
        if (anim.GetBool("Jump"))
        {
            if(rb.velocity.y < .1f)
            {
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Fall"))
        {
            anim.SetBool("Fall", false);
        }
    }

    //called bu animation event "Idle"
    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)  //beyond the leftCap
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (coll.IsTouchingLayers(ground))  //on the ground
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight); //jump
                    anim.SetBool("Jump", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jump", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }




}
