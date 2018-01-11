using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour {

    public float moveSpeed = 2f;        // The speed the enemy moves at.
    public int HP = 2;                  // How many times the enemy can be hit before it dies.
    public AudioClip[] deathClips;      // An array of audioclips that can play when the enemy dies.
    public GameObject hundredPointsUI;  // A prefab of 100 that appears when the enemy dies.
    public float deathSpinMin = -100f;          // A value to give the minimum amount of Torque when dying
    public float deathSpinMax = 100f;			// A value to give the maximum amount of Torque when dying
    public bool shell = false;                  // Whether or not this is a shell-based enemy.
    public GameObject Stomp;                    // A reference to the Stomp object

    private Transform frontCheck;       // Reference to the position of the gameobject used for checking if something is in front.
    private bool dead = false;			// Whether or not the enemy is dead.
    public bool stomp = false;          // Whether or not the enemy is stomped.
    public bool kickLeft = false;       // Whether or not this shell has been kicked to the left
    public bool kickRight = false;      // Whether or not this shell has been kicked to the right
    private Score score;				// Reference to the Score script.
    private Animator anim;              // Reference to the animator.
    private AudioSource audioSource;    // Reference to the audioSource.


    void Awake()
    {
        // Setting up the references.
        frontCheck = transform.Find("frontCheck").transform;
        anim = transform.GetComponent<Animator>();
        score = GameObject.Find("Score").GetComponent<Score>();
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();

        // The Koopa needs to be flipped because it's initially going the wrong way.
        Flip();
    }

    void FixedUpdate()
    {
        // If the Koopa has been hit, but it's not dead or a shell, make it into a shell.
        if (stomp == true && dead == false && shell == false)
        {
            Hurt();
        }
        // If the Koopa is not a shell, allow it to run freely.
        if (shell == false)
        {
            // Create an array of all the colliders in front of the enemy.
            Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

            // Check each of the colliders.
            foreach (Collider2D c in frontHits)
            {
                // If any of the colliders is an Obstacle...
                if (c.tag == "Obstacle")
                {
                    // ... Flip the enemy and stop checking the other colliders.
                    Flip();
                    break;
                }
            }

            // Set the enemy's velocity to moveSpeed in the x direction.
            GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        // If the Koopa is a shell, only allow it to move when it gets kicked.
        if (shell == true)
        {
            if (kickLeft == true || kickRight == true)
            {
                // Create an array of all the colliders in front of the enemy.
                Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

                // Check each of the colliders.
                foreach (Collider2D c in frontHits)
                {
                    // If any of the colliders is an Obstacle...
                    if (c.tag == "Obstacle")
                    {
                        // ... Flip the enemy and stop checking the other colliders.
                        Flip();
                        break;
                    }
                }
                if (kickLeft == true)
                {
                    // Play the kick noise
                    audioSource.PlayOneShot(deathClips[0]);

                    // Set the enemy's velocity to moveSpeed in the x direction.
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localScale.x * moveSpeed * 10, GetComponent<Rigidbody2D>().velocity.y);
                    kickLeft = false;
                }
                if(kickRight == false)
                {
                    // Play the kick noise.
                    audioSource.PlayOneShot(deathClips[0]);

                    // Set the enemy's velocity to moveSpeed in the x direction.
                    GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed * 10, GetComponent<Rigidbody2D>().velocity.y);
                    kickRight = false;
                }
            }
        }
        

        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (HP <= 0 && !dead)
            // ... call the death function.
            Death();
    }

    public void Hurt()
    {
        // Reduce the number of hit points by one.
        HP--;

        if(HP < 2)
        {
            anim.SetTrigger("Dead");
        }
        Stomp.tag = "Shell";
        Stomp.layer = 14;
        this.gameObject.tag = "Shell";
        this.gameObject.layer = 14;
        shell = true;
    }

    void Death()
    {
        // Increase the score by 100 points
        score.score += 100;

        // Set dead to true.
        dead = true;

        // Allow the enemy to rotate and spin it by adding a torque.
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(deathSpinMin, deathSpinMax));

        // Find all of the colliders on the gameobject and set them all to be triggers.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        // Move all sprite parts of the enemy to the front
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in spr)
        {
            s.sortingLayerName = "UI";
        }

        // Play The Goomba's death clip
        audioSource.PlayOneShot(deathClips[0]);

        // Create a vector that is just above the enemy.
        Vector3 scorePos;
        scorePos = transform.position;
        scorePos.y += 1.5f;

        // Instantiate the 100 points prefab at this point.
        Instantiate(hundredPointsUI, scorePos, Quaternion.identity);
    }


    public void Flip()
    {
        // Multiply the x component of localScale by -1.
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
