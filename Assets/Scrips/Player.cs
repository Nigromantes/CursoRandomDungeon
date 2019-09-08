using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Vector2 targetPosition;
    Transform transform;
    float flipX;
    LayerMask obstaculeMask;
    bool isMoving;
    public float speed;

	void Start () {
        transform = GetComponent<Transform>();
        flipX = transform.localScale.x;
        obstaculeMask = LayerMask.GetMask("Wall", "Enamy");
		
	}
	
	
	void Update () {
        float horizontal = System.Math.Sign(Input.GetAxisRaw("Horizontal"));
        float vertical = System.Math.Sign(Input.GetAxisRaw("Vertical"));
        if (Mathf.Abs(horizontal)>0 || Mathf.Abs(vertical)>0)
        {
            if (Mathf.Abs(horizontal) > 0)
            {
                transform.localScale = new Vector2(flipX * horizontal, transform.localScale.y);
            }

            if (!isMoving)
            {
                if (Mathf.Abs(horizontal) > 0)
                {
                    targetPosition = new Vector2(transform.position.x + horizontal,
                        transform.position.y);
                     
                }
                else if (Mathf.Abs(vertical) > 0)
                {
                    targetPosition = new Vector2(transform.position.x,
                        transform.position.y + vertical);
                }
                //Revisar collisiones. 
                Vector2 hitSize = Vector2.one * 0.8f;
                Collider2D hit = Physics2D.OverlapBox(targetPosition, hitSize, 0,
                    obstaculeMask);
                if (!hit)
                {
                    StartCoroutine(SmoothMove());
                }
            }
            




        }
    }

    IEnumerator SmoothMove()
    {
        isMoving = true;
        while (Vector2.Distance(transform.position,targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                targetPosition, speed * Time.deltaTime);
            yield return null;
  
        }
        transform.position = targetPosition;
        
        isMoving = false;
    }
}
