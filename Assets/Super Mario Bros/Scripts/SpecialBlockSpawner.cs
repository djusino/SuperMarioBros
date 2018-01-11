using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBlockSpawner : MonoBehaviour {

    private Animator anim;                      // The Animator for the block.
    private PlayerControl playerControl;        // The controller for the player.
    private bool get = false;                   // The value for indicating if the box is empty.
    private int currentPoints = 0;              // The current amount of points given out by the box.
    private Score score;                        // The score for incrementing.

    public GameObject spawn;                    // The game object for the spawn location.
    public GameObject hundredPointsUI;          // The game object for the 100 points prefab.
    public GameObject item;                     // The game object for the item prefab.
    public int howManyPoints;                   // The amount of points the game developer wants inside the block.
    public bool points;                         // The value for discerning if this is a point block or not.

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        score = GameObject.Find("Score").GetComponent<Score>();
    }

    // On Collision, Check...
    private void OnCollisionEnter2D(Collision2D col)
    {
        // If the player collided into the block.
        if(col.gameObject.tag == "Player" && !get)
        {
            // If this block is a point block, or an item block.
            if (points == true)
            {
                if (currentPoints < howManyPoints)
                {
                    playerControl.PlayAudioEffect("Appear");

                    // Create a vector that is just above the block.
                    Vector3 scorePos;
                    scorePos = transform.position;
                    scorePos.y += 10f;

                    // Instantiate the 100 points prefab at this point.
                    Instantiate(hundredPointsUI, scorePos, Quaternion.identity);
                    score.score += 100;
                    currentPoints++;
                    
                    //If we have reached the current amount of points..
                    if(howManyPoints == currentPoints)
                    {
                        //Deactivate the box.
                        anim.SetTrigger("Hit");
                        get = true;
                    }
                }
            }
            else
            {
                // If this block is an item block...
                playerControl.PlayAudioEffect("Appear");
                anim.SetTrigger("Hit");

                // Create the item.
                Instantiate(item, spawn.transform.position, Quaternion.identity);

                // Deactivate the box.
                get = true;
            }
        }
    }
}
