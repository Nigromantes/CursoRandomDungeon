using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


    public Vector2 patrolInterval;

    private LayerMask obstaculeMask;
    Vector2 currentPosition;
    List<Vector2> avaibleMovementList = new List<Vector2>();
    private bool isMoving;

    
    // Use this for initialization

	void Start () {
        obstaculeMask = LayerMask.GetMask("Wall", "Enemy", "Player");
        currentPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (!isMoving)
        {
            Patrol(); 	
        }
                
	}

    private void Patrol()
    {
        avaibleMovementList.Clear();

        Vector2 size = Vector2.one * 0.8f;

        
        DetectarDireccion(size,Vector2.up);
        DetectarDireccion(size,Vector2.right);
        DetectarDireccion(size,Vector2.left);
        DetectarDireccion(size,Vector2.down);

        if (avaibleMovementList.Count > 0)
        {
            int randomIndex = Random.Range(0, avaibleMovementList.Count);
            currentPosition += avaibleMovementList[randomIndex];
        }

        StartCoroutine(SmootMove());

    }

    private void DetectarDireccion(Vector2 size,Vector2 direccionFinal)
    {
        Collider2D direccion = Physics2D.OverlapBox(currentPosition + direccionFinal, size, 0, obstaculeMask);

        if (!direccion)
        {
            avaibleMovementList.Add(direccionFinal);
        }
    }

    IEnumerator SmootMove()
    {
        isMoving = true;
        while (Vector2.Distance(transform.position,currentPosition)>0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentPosition, 5f * Time.deltaTime);
            yield return null;
        }
        transform.position = currentPosition;
        yield return new WaitForSeconds(Random.Range(patrolInterval.x, patrolInterval.y));
    

        isMoving = false;

    }
}
