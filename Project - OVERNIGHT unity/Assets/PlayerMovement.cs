using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Moving")]
    public float moveSpeed = 10f;
    [SerializeField] public float airMoveSpeed = 15f;
    public bool facingRight = true;

    [Header("Jumping")]
    public bool isGrounded = false;
    public float ySpeed = 0f;

    [Header("Wall Climbing")]
    public float wallSlideSpeed = 0f;
    public bool isTouchingWall = false;
    public bool isWallSliding = false;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WallClimb();
        Move();
    }

    void Update()
    {
        Jump();
    }

    void Jump() //Jump
    {
        if ((Input.GetButtonDown("Jump") && isGrounded == true)
            || (isWallSliding == true && Input.GetButtonDown("Jump")))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, ySpeed),
                ForceMode2D.Impulse);
        }
    }

    void Move() //x & y movement
    {
       
        Vector3 movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        transform.position += movement * Time.deltaTime * moveSpeed; 

        if (Input.GetAxisRaw("Horizontal") < 0 && facingRight)
        {
            Flip();
        }

        else if (Input.GetAxisRaw("Horizontal") > 0 && !facingRight)
        {
            Flip();
        }

    }

    void Flip() //rotate charater
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    void WallClimb() //grab wall & fall
    { 
        if (isTouchingWall == true && isGrounded == false
            && gameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
        if (isWallSliding == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "wall")
        {
            gameObject.GetComponent<PlayerMovement>().isTouchingWall = true;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "wall")
        {
            gameObject.GetComponent<PlayerMovement>().isTouchingWall = false;

        }
    }
}
