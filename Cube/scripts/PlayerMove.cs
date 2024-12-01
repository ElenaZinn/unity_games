using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{

    private int Horizontal = 0;

    public Animator anim;
    public float MovementSpeed = 5f;
    public float Gravity = 1f;
    public CharacterController charController;
    private FacingDirection _myFacingDirection;
    public float JumpHeight = 5f;
    public bool _jumping = false;
    private float degree = 0;
    private Rigidbody player;


    public FacingDirection CmdFacingDirection
    {

        set
        {
            _myFacingDirection = value;
        }
        

    }

    private void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Horizontal") < 0)
            Horizontal = -1;
        else if (Input.GetAxis("Horizontal") > 0)
            Horizontal = 1;
        else
            Horizontal = 0;

        if (Input.GetKeyDown(KeyCode.Space) && !_jumping)
        {
            _jumping = true;
            Debug.Log("StartCoroutine");
            anim.SetBool("Jump", _jumping);
            StartCoroutine(JumpingWait());
        }


        if (anim)
        {
            anim.SetInteger("Horizontal", Horizontal);
            anim.SetBool("Jump", _jumping);
            float moveFactor = MovementSpeed * Time.deltaTime * 10f;
        
            MoveCharacter(moveFactor);

        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8 * Time.deltaTime);

    }

    private void MoveCharacter(float moveFactor)
    {
        Vector3 trans = Vector3.zero;
        if (_myFacingDirection == FacingDirection.Front)
        {
            trans = new Vector3(Horizontal * moveFactor, -Gravity * moveFactor, 0f);
            charController.SimpleMove(trans);
        }
        else if (_myFacingDirection == FacingDirection.Right)
        {
            trans = new Vector3(0f, -Gravity * moveFactor, Horizontal * moveFactor);
            charController.SimpleMove(trans);
        }
        else if (_myFacingDirection == FacingDirection.Back)
        {
            trans = new Vector3(-Horizontal * moveFactor, -Gravity * moveFactor, 0f);
            charController.SimpleMove(trans);
        }
        else if (_myFacingDirection == FacingDirection.Left)
        {
            trans = new Vector3(0f, -Gravity * moveFactor, -Horizontal * moveFactor);
            charController.SimpleMove(trans);
        }
        if (_jumping)
        {
      
            Debug.Log("Jumping");
            transform.Translate( Vector3.up * JumpHeight * Time.deltaTime);
            //transform.position += Vector3.up * 10000f;
            Debug.Log("Jumping");
        }


        //charController.SimpleMove(trans);
    }
    public void UpdateToFacingDirection(FacingDirection newDirection, float angle)
    {

        _myFacingDirection = newDirection;
        degree = angle;

    }

    public IEnumerator JumpingWait()
    {
        yield return new WaitForSeconds(0.35f);
        //Debug.Log ("Returned jump to false");
        _jumping = false;
        anim.SetBool("Jump", _jumping);
        Debug.Log("JumpingWait stop");
    }

}