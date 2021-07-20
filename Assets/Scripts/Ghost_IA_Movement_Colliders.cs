/// <summary>
/// This is the Ghost IA Movement Colliders:
/// - The "Colliders" script;
/// </summary> 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_IA_Movement_Colliders : MonoBehaviour
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
        if (coll.gameObject.CompareTag("Scenario"))
        {
            GhostScript.directions.Remove(moveDirection);
            //GhostScript.directions.RemoveAll(Vector3 => Vector3 == moveDirection);
            GhostScript.NewDirection();
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Scenario"))
        {
            GhostScript.directions.Add(moveDirection);
            GhostScript.NewDirection();
        }
    }

}
