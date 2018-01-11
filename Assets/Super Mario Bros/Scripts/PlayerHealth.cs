using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{	
	public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
	public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
	public float hurtForce = 10f;				// The force with which the player is pushed when hurt.
	public float damageAmount = 10f;			// The amount of damage to take when enemies touch the player
    public GameObject hundredPointsUI;          // A prefab of 100 that appears when the player gets a coin
    public GameObject winText;
    public AudioSource audioSource;

    private float lastHitTime;					// The time at which the player was last hit.
	private PlayerControl playerControl;		// Reference to the PlayerControl script.
	private Animator anim;                      // Reference to the Animator on the player
    private Coin coin;                          // A reference to the coin script
    private Score score;                        // A reference to the score script
    private bool played = false;                        // Required due to a bug with having double colliders


    void Awake ()
	{
		// Setting up references.
		playerControl = GetComponent<PlayerControl>();
		anim = GetComponent<Animator>();
        coin = GameObject.Find("CoinText").GetComponent<Coin>();
        score = GameObject.Find("Score").GetComponent<Score>();
    }


	void OnCollisionEnter2D (Collision2D col)
	{
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Enemy")
		{
			// ... and if the time exceeds the time of the last hit plus the time between hits...
			if (Time.time > lastHitTime + repeatDamagePeriod) 
			{
				// ... and if the player still has health...
				if(health == 2f)
				{
                    playerControl.PlayAudioEffect("Down");
					// ... take damage and reset the lastHitTime.
					TakeDamage(col.transform); 
					lastHitTime = Time.time;
				}
				// If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
				else
				{
					// Find all of the colliders on the gameobject and set them all to be triggers.
					Collider2D[] cols = GetComponents<Collider2D>();
					foreach(Collider2D c in cols)
					{
						c.isTrigger = true;
					}

					// Move all sprite parts of the player to the front
					SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
					foreach(SpriteRenderer s in spr)
					{
						s.sortingLayerName = "UI";
					}

					// ... disable user Player Control script
					GetComponent<PlayerControl>().enabled = false;

                    // ... Trigger the 'Die' animation state
                    if (played == false)
                    {
                        audioSource.Stop();
                        playerControl.PlayAudioEffect("Death");
                        played = true;

                        anim.SetTrigger("Die");

                        winText.GetComponent<Text>().text = "Game Over!" + "\n" + "Press 'R' To Retry!";
                    }

                }
			}
		}
        // If the colliding object is a mushroom...
        if(col.gameObject.tag == "Mushroom")
        {
            //Play the power up noise
            playerControl.PlayAudioEffect("Mushroom");
            score.score += 100;

            var scorePos = transform.position;

            // Instantiate the 100 points prefab at this point.
            Instantiate(hundredPointsUI, scorePos, Quaternion.identity);

            // Destroy the object if we're at full HP.
            if (health == 2f)
            {
                Destroy(col.gameObject);
            }
            // Increase HP if we are not at full HP.
            if(health == 1f)
            { 
                // Increase Health
                health++;
                Destroy(col.gameObject);

                // Make Mario Grow
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                anim.SetTrigger("Mushroom");
            }
        }
        // If the colliding object is a brick...
        if(col.gameObject.tag == "Brick")
        {
            // Break the brick
            playerControl.PlayAudioEffect("Break");
            Destroy(col.gameObject);
        }
	}


	void TakeDamage (Transform enemy)
	{
		// Make sure the player can't jump.
		playerControl.jump = false;

		// Create a vector that's from the enemy to the player with an upwards boost.
		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
		GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

		// Reduce the player's health by 10.
		health -= 1;

        // Disable the top collider and trigger the animation
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        anim.SetTrigger("Hit");
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        // If the colliding object is a coin...
        if (col.tag == "Coin")
        {
            // Increase the coin and score count, and then destroy the object.
            Destroy(col.gameObject);
            coin.coin++;
            score.score += 100;

            var scorePos = transform.position;

            // Instantiate the 100 points prefab at this point.
            Instantiate(hundredPointsUI, scorePos, Quaternion.identity);
        }
        // If the colliding object is the finishing flag...
        if (col.tag == "Finish Flag")
        {
            // Freeze the player.
            var rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            // Find all of the colliders on the gameobject and set them all to be triggers.
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (Collider2D c in cols)
            {
                c.isTrigger = true;
            }

            // Move all sprite parts of the player to the front
            SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer s in spr)
            {
                s.sortingLayerName = "UI";
            }

            // ... disable user Player Control script
            GetComponent<PlayerControl>().enabled = false;

            // ... Trigger the 'Die' animation state
            audioSource.Stop();
            playerControl.PlayAudioEffect("Win");
            anim.SetTrigger("Win");

            // ... Send the winning text message to the screen.
            winText.GetComponent<Text>().text = "You won!" + "\n" + "Press 'R' To Retry!";
        }
    }


    private void OnTriggerExit2D(Collider2D col)
    {
        // If the colliding object is the kill plane...
        if (col.tag == "Kill Plane")
        {
            // Find all of the colliders on the gameobject and set them all to be triggers.
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (Collider2D c in cols)
            {
                c.isTrigger = true;
            }

            // Move all sprite parts of the player to the front
            SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer s in spr)
            {
                s.sortingLayerName = "UI";
            }

            // ... disable user Player Control script
            GetComponent<PlayerControl>().enabled = false;

            // ... Trigger the 'Die' animation state
            if(played == false) /// This is what prevents the kill plane from triggering this sequence twice due to Mario's double box colliders!!
            {
                audioSource.Stop();
                playerControl.PlayAudioEffect("Death");
                played = true;


                anim.SetTrigger("Die");

                winText.GetComponent<Text>().text = "Game Over!" + "\n" + "Press 'R' To Retry!";
            }
        }
    }
}
