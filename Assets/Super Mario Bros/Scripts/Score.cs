using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour
{
	public int score = 0;					// The player's score.
    private string scoreLength;             // The string to hold the player's score.
    string format = "000000";               // The format for the string.
	private int previousScore = 0;			// The score in the previous frame.

	void Update ()
	{
        // Learn the current score size
        scoreLength = score.ToString(format);

		// Set the score text.
		GetComponent<Text>().text = "Score: " + scoreLength;

		// Set the previous score to this frame's score.
		previousScore = score;
	}

}
