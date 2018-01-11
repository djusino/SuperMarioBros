using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;         // For determining which way the player is currently facing.
    [HideInInspector]
    public bool jump = false;               // Condition for whether the player should jump.
    public bool duck = false;               // Condition for whether the player should duck


    public float moveForce = 365f;          // Amount of force added to move the player left and right.
    public float maxSpeed = 5f;             // The fastest the player can travel in the x axis.
    public AudioClip[] jumpClips;           // Array of clips for when the player jumps.
    public float jumpForce = 1000f;         // Amount of force added when the player jumps.
    public AudioClip[] specialClips;        // Array of clips for special situations.
    public float tauntProbability = 50f;    // Chance of a taunt happening.
    public float tauntDelay = 1f;           // Delay for when the taunt should happen.

    public GameObject playerObject;         // To simulate ducking

    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;          // Whether or not the player is grounded.
    private bool obstacled = false;         // Whether or not the player is on an obstacle
    private bool crouched = false;          // Whether the sprite is crouched or not
    private Animator anim;                  // Reference to the player's animator component.
    private PlayerHealth health;            // Reference to the player's health component.
    private AudioSource audioSource;        // Reference to the player's audio source producing the background music.


    void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("groundCheck");
        anim = GetComponent<Animator>();
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        audioSource = playerObject.GetComponent<AudioSource>();
    }


    void Update()
    {
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        obstacled = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Default"));

        // If the jump button is pressed and the player is grounded then the player should jump or be able to duck if the duck button is pressed.
        if (Input.GetButtonDown("Jump") && (grounded || obstacled))
            jump = true;
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && (grounded || obstacled))
            duck = true;
    }


    void FixedUpdate()
    {
        // Cache the horizontal input.
        float h = Input.GetAxis("Horizontal");

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        anim.SetFloat("Speed", Mathf.Abs(h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            // ... add a force to the player.
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (h > 0 && !facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (h < 0 && facingRight)
            // ... flip the player.
            Flip();

        // If the player should jump...
        if (jump)
        {
            // Set the Jump animator trigger parameter.
            anim.SetTrigger("Jump");

            // Play a jump audio clip depending on the health value of the player if...
            if(health.health == 1)
            {
                // The player is at low health.
                audioSource.PlayOneShot(jumpClips[0]);
            }
            if(health.health == 2)
            {
                // The player is at high health.
                audioSource.PlayOneShot(jumpClips[1]);
            }
            

            // If the player is crouched while the player should jump, disable the crouch
            if(crouched)
            {
                playerObject.GetComponent<BoxCollider2D>().enabled = true;
                crouched = false;
            }

            // Add a vertical force to the player.
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
        }
        
        //And if the player should duck...
        if (duck)
        {
            // Set the Duck animator trigger parameter
            anim.SetTrigger("Duck");

            // Check if the sprite is crouched, and if so, enable or disable the hitbox.
            if (!crouched)
            {
                playerObject.GetComponent<BoxCollider2D>().enabled = false;
                crouched = true;
            }
            else
            {
                playerObject.GetComponent<BoxCollider2D>().enabled = true;
                crouched = false;
            }
            
            // Make sure the player can't duck again until the duck conditions from Update are satisfied.
            duck = false;
        }
    }


    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void PlayAudioEffect(string audioType)
    {
        // Play the associated audio effect from the audio source.
        if (audioType == "Mushroom")
        {
            audioSource.PlayOneShot(specialClips[0]);
        }
        if (audioType == "Coin")
        {
            audioSource.PlayOneShot(specialClips[1]);
        }
        if (audioType == "Death")
        {
            audioSource.PlayOneShot(specialClips[2]);
        }
        if (audioType == "Down")
        {
            audioSource.PlayOneShot(specialClips[3]);
        }
        if (audioType == "Break")
        {
            audioSource.PlayOneShot(specialClips[4]);
        }
        if (audioType == "Appear")
        {
            audioSource.PlayOneShot(specialClips[5]);
        }
        if (audioType == "Win")
        {
            audioSource.PlayOneShot(specialClips[6]);
        }
    }
}
