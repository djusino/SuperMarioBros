using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        // ... If the "R" key
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel); /// LoadLevel is used since reloading scenes usually breaks any lighting effects
        }
	}
}
