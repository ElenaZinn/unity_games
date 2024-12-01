using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Move left and right
 * Jump
 */
public class MyScripts : MonoBehaviour
{

    public float speed = 6f;
    private float direction = 0f;
    private Rigidbody2D player;
    public float jumpSpeed = 4.5f;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        Debug.Log("Hello world");
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxis("Horizontal");

        if (direction > 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            //transform.localScale = new Vector2(0.2469058f, 0.2469058f);
        }
        else if (direction < 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            //transform.localScale = new Vector2(-0.2469058f, 0.2469058f);
        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        if (Input.GetButtonDown("Jump"))
        {
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
        }
    }
}
