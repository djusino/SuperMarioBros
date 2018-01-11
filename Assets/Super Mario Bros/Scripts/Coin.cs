using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Coin : MonoBehaviour
{
    public int coin = 0;					// The player's coin amount.
    private string coinString;              // The string for the player's coins.
    string format = "00";                   // The format for displaying the string.


    private PlayerControl playerControl;    // Reference to the player control script.
    private PlayerHealth playerHealth;      // Reference to the player health script.
    private int previousCoin = 0;          // The amount of coins in the previous frame.


    void Awake()
    {
        // Setting up the reference.
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }


    void Update()
    {
        // If the amounts of coins is over 99, increase the player's health and reset to zero.
        if(coin > 99)
        {
            coin = 0;
            playerHealth.health++;
        }

        // Learn the current coin size
        coinString = coin.ToString(format);

        // Set the coin text.
        GetComponent<Text>().text = "x " + coinString;

        // If the coin has changed...
        if (previousCoin != coin)
            playerControl.PlayAudioEffect("Coin");

        // Set the previous coin to this frame's coin.
        previousCoin = coin;
    }

}