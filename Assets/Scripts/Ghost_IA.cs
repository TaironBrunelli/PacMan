/// <summary>
/// This is the Ghost IA:
/// - Move the Ghost through the waypoints; 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_IA : MonoBehaviour
{
    public GameObject Ghost_Waypoints;
    private Transform[] waypoints;
    int cur = 0;
    public float speed = 0.3f;

    void Start()
    {
        waypoints = Ghost_Waypoints.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != waypoints[cur].position)
        {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position, speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        // Waypoint reached, select next one
        else cur = (cur + 1) % waypoints.Length;
    }

    void OnEnable()
    {
        cur = 0; //-- Reset movement path
    }

}