/// <summary>
/// This is the Ghost IA Movement Colliders Seekers:
/// - 'Overwrite' the "Colliders" functions to make the Ghost Seeker or Run away from Pacman; 
/// </summary> 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_IA_Movement_Colliders_Seeker : MonoBehaviour
{
    public Vector3 moveDirection;
    private GameObject Ghost;
    private Ghost_IA_Movement GhostScript;

    void Start()
    {
        Ghost = transform.parent.parent.gameObject;
        GhostScript = Ghost.GetComponent<Ghost_IA_Movement>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            if(PacMan_Controller.ghostEater)
                GhostScript.SeekerDirection(-1* moveDirection); //-- Run away !!!--- Bug if the player follow the Ghost to a edge;
            else
                GhostScript.SeekerDirection(moveDirection); //-- Seeker 
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            GhostScript.StopedSeeking(); //-- Pacman got away from 'vision' area -> Stop seeking
        }
    }
}
