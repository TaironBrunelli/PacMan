/// <summary>
/// This is the Portal Script:
/// - Teleport Player and Ghost to the other side;
/// </summary>
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] //-- Unity will create the required components

public class Portal : MonoBehaviour
{
	public Vector3 newPosition;
	void OnTriggerEnter2D(Collider2D coll)
	{
		//-- Player and Ghost collider
		if ((coll.gameObject.CompareTag("Player")) || (coll.gameObject.CompareTag("Enemy")))
		{
			coll.gameObject.transform.position = newPosition;
		}
	}
}