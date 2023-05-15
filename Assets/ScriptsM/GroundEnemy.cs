using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    public Transform player;
    // reference for my waypoints
    public List<Transform> points;

    // the int value  for my indexed list
    public int nextId;

    // Declare a int to help us change our nextid
    private int idChangeValue = 1;

    // sets our speed of the enemy
   public float speed = 2;
    // Update is called once per frame
    public bool isFlipped;
    public Player_Movement playerMovement;
    void FixedUpdate()
    {

        if (Vector2.Distance(transform.position, player.position) < 4f)
        {
            LookatPlayer(); 

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            MoveToNextPoint();
            
        }
    }

    void MoveToNextPoint()
    {
        // Declare and set a transform to our next point
        Transform goalPoint = points[nextId];

        // flip our enemy via  the transform to look at the points direction
        // Might need to change based off of the sprites natural face
        if (goalPoint.transform.position.x > transform.position.x)
        {
            //-1
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            //1
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Move the enemy towards our point 
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);

        // Check the direction between and the goalPoint to trigger the next point
        if (Vector2.Distance(transform.position, goalPoint.position) < 1f)
        {
            //Check if we are at the end of the line make our change value -1
            if (nextId == points.Count - 1)
            {
                idChangeValue = -1;
            }
            //Chack if we are at the start of our line make our change value 1
            if (nextId == 0)
            {
                idChangeValue = 1;
            }
            nextId += idChangeValue;
        }
    }
    public void LookatPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= 1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            isFlipped = false;
        }
        if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0, 180, 0);
            isFlipped = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            var healthComponent = collision.GetComponent<Health>();
             if(healthComponent != null)
             {
                playerMovement.KBCounter = playerMovement.KBTolalTIme;
                if(collision.transform.position.x <= transform.position.x)
                {
                    playerMovement.knockFromRight = true;
                }
                if (collision.transform.position.x > transform.position.x)
                {
                    playerMovement.knockFromRight = false;
                }
                healthComponent.TakeDamage(1);
             }
        }
    }

     IEnumerator Speed()
     {
        speed = 4;
        yield return new WaitForSeconds(5);
        speed = 2;
     }
}
