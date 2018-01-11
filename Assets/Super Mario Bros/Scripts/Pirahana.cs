using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirahana : MonoBehaviour {

    public float speed = 5f;            // Speed of the cycles.
    public float height = 30f;          // Height of the cycles.
    private Vector3 origin;             // The origin point of the plants.

    void Awake()
    {
        origin = transform.position;
    }

    void FixedUpdate()
    {
        // Move the pirahana plant up and down.
        float newY = Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(origin.x, newY * height, origin.z);
    }
}
