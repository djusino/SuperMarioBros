using UnityEngine;
using System.Collections;

public class Goomba : MonoBehaviour
{
    public float moveSpeed = 2f;        // The speed the Goomba moves at.
    public int HP = 2;                  // How many times the Goomba can be hit before it dies.
    public AudioClip[] deathClips;      // An array of audioclips that can play when the Goomba dies.
    public GameObject hundredPointsUI;  // A prefab of 100 that appears when the Goomba dies.
    public float deathSpinMin = -100f;          // A value to give the minimum amount of Torque when dying
    public float deathSpinMax = 100f;			// A value to give the maximum amount of Torque when dying

    private Transform frontCheck;       // Reference to the position of the gameobject used for checking if something is in front.
    private bool dead = false;			// Whether or not the enemy is dead.
    public bool stomp = false;          // Whether or not the enemy is stomped.
    private Score score;				// Reference to the Score script.
    private Animator anim;              // An animator for the Goomba.
    private GameObject playerObject;    // Reference to the playerObject.
    private AudioSource audioSource;    // Reference to the audioSource.


    void Awake()
    {
        // Setting up the references.
        frontCheck = transform.Find("frontCheck").transform;
        anim = transform.GetComponent<Animator>();
        score = GameObject.Find("Score").GetComponent<Score>();
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // If the Goomba has been stomped and is not dead, hurt the Goomba.
        if (stomp && !dead)
        {
            Hurt();
        }

        // Create an array of all the colliders in front of the enemy.
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

        // Check each of the colliders.
        foreach (Collider2D c in frontHits)
        {
            // If any of the colliders is an Obstacle...
            if (c.tag == "Obstacle")
            {
                // ... Flip the Goomba and stop checking the other colliders.
                Flip();
                break;
            }
        }

        // Set the Goomba's velocity to moveSpeed in the x direction.
        GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

        // If the Goomba has zero or fewer hit points and isn't dead yet...
        if (HP <= 0 && !dead)
            // ... call the death function.
            Death();
    }

    public void Hurt()
    {
        // Reduce the number of hit points by one.
        HP--;
    }

    void Death()
    {
        // Increase the score by 100 points
        score.score += 100;

        // Set dead to true.
        dead = true;

        // Allow the Goomba to rotate and spin it by adding a torque.
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(deathSpinMin, deathSpinMax));

        // Find all of the colliders on the gameobject and set them all to be triggers.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        // Move all sprite parts of the Goomba to the front
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in spr)
        {
            s.sortingLayerName = "UI";
        }

        // Play The Goomba's death clip
        audioSource.PlayOneShot(deathClips[0]);

        // Create a vector that is just above the Goomba.
        Vector3 scorePos;
        scorePos = transform.position;
        scorePos.y += 1.5f;

        // Instantiate the 100 points prefab at this point.
        Instantiate(hundredPointsUI, scorePos, Quaternion.identity);

        // Animate the death.
        anim.SetTrigger("Dead");
    }


    public void Flip()
    {
        // Multiply the x component of localScale by -1.
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
