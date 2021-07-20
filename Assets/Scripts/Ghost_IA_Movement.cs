/// <summary>
/// This is the Ghost IA Movement:
/// - IA that move to 'random' directions based on "Colliders" script;
/// </summary> 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ghost_IA_Movement : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    [SerializeField]
    private float speed;
    public List<Vector3> directions;
    private Vector3 direction;
    private Vector3 currentDirection;
    private bool canMove;
    private bool isSeeking; //-- Go to the Player's direction
    public Vector3 startDirection;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        isSeeking = false;
        m_Rigidbody = GetComponent<Rigidbody>();
        directions.Add(startDirection);
        NewDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            transform.position += currentDirection * speed * Time.deltaTime;
        }
    }

    public void NewDirection()
    {
        if(!isSeeking)
        {
            //canMove = false;
            direction = directions.ElementAt(Random.Range(0, directions.Count));
            if (direction == -1 * currentDirection) //-- Check if the directions are opposites
                NewDirection(); //-- Don't let them move back; //-- Performace issues
            else
            {
                currentDirection = direction; //-- New Direction;
                canMove = true;
            }
        }
    }

    public void SeekerDirection(Vector3 seekerVector)
    {
        isSeeking = true;
        canMove = true;
        currentDirection = seekerVector;
    }
    public void StopedSeeking()
    {
        isSeeking = false;
        NewDirection();
    }
}