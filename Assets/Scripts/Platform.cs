using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [SerializeField] private float UpCap;
    [SerializeField] private float DownCap;
    private float Speed = 3f;
    protected Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private bool Top = false;
    // Update is called once per frame
    void Update()
    {        
        if(!Top)
        {
            if (transform.position.y < UpCap)
            {
                rb.velocity = new Vector2(0, Speed);
            }
            else
                Top = true;
        }
        else
        {
            if (transform.position.y > DownCap)
            {
                rb.velocity = new Vector2(0, -Speed);
            }
            else
                Top = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Speed = 4f;
        }
        else
            Speed = 3f;
    }
}
