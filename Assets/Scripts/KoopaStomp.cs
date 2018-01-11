using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaStomp : MonoBehaviour {

    public GameObject koopa;                // Reference to the Koopa.
    public Collider2D topCollider;          // Reference to the Top Collider of the Koopa.
    public Collider2D leftCollider;         // Reference to the Left Collider of the Koopa.
    public Collider2D rightCollider;        // Reference to the Right Collider of the Koopa.

    private void OnTriggerEnter2D(Collider2D col)
    {
        // If the colliding object is the player... 
        if (col.tag == "Player")
        {
            // ... Stomp the Koopa from above
            if (col.IsTouching(topCollider)){
                koopa.GetComponent<Koopa>().stomp = true;
            }
            // ... Kick the Koopa to the right
            if (col.IsTouching(leftCollider) && (koopa.GetComponent<Koopa>().stomp == true))
            {
                koopa.GetComponent<Koopa>().kickRight = true;
            }
            // ... Kick the Koopa to the left
            if (col.IsTouching(rightCollider) && (koopa.GetComponent<Koopa>().stomp == true))
            {
                koopa.GetComponent<Koopa>().kickLeft = true;
            }
        }
        // If the colliding object is the kill plane...
        if (col.tag == "Kill Plane")
        {
            // Destroy Him.
            Destroy(koopa);
        }
    }
}
