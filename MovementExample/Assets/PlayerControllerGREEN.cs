using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGREEN : MonoBehaviour
{
    public float speed = 5;
    public float jumpPower = 10;

    Rigidbody2D rb2d;
    Vector2 movement = new Vector2();
    bool grounded = false;
    bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        float x = Input.GetAxis("Horizontal");

        movement = new Vector2(x * speed, rb2d.velocity.y);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    //Update is called every physics interval (0.02s) before physics
    private void FixedUpdate()
    {
        rb2d.velocity = movement;

        if (jump)
        {
            //Jump with impulse
            rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            //Jump with velocity
            //rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
            jump = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        grounded = true;
    }
}
