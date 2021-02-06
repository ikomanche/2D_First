using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : Enemy
{

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] LayerMask ground;

    private float speed = 20;
    private Collider2D coll;
    private double mBoxSize;

    private bool facingLeft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();        
    }

    // Update is called once per frame
    void Update()
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
                    rb.velocity = new Vector2(-speed, 0); //move                   
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
                    rb.velocity = new Vector2(speed, 0);                    
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }
   
}
