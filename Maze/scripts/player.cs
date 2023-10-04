using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class player : MonoBehaviour
{
    public float speed = 5.0f;

    public int keys = 0;

    public Text keyAmount;

    public Text success;

    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);

        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);

        }

        if (keys == 3 )
        {
            Destroy(door);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "keys")
        {
            keys++;
            keyAmount.text = "Score: " + keys;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "heart" && keys == 3)
        {

            success.text = "You win!";
            Destroy(collision.gameObject);
        }


        if (collision.gameObject.tag == "enemy") 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.gameObject.tag == "walls")
        {
            Debug.Log("walls hit");
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);

            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(0, speed * Time.deltaTime, 0);

            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);

            }
        }
    }
}
