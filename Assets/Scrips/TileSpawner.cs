using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour {


    DungeonManager dungeonManager;
    
    // Use this for initialization
	void Awake () {
        dungeonManager = FindObjectOfType<DungeonManager>();
        GameObject gameObjetFloor = Instantiate(dungeonManager.floorPrefab, transform.position, Quaternion.identity) as GameObject;
        gameObjetFloor.name = dungeonManager.floorPrefab.name;
        gameObjetFloor.transform.SetParent(dungeonManager.transform);

        if (transform.position.x > dungeonManager.maxX)
        {
            dungeonManager.maxX = transform.position.x;
        }

        if (transform.position.x < dungeonManager.minX)
        {
            dungeonManager.minX = transform.position.x;
        }

        if (transform.position.y > dungeonManager.maxY)
        {
            dungeonManager.maxY = transform.position.y;
        }

        if (transform.position.y < dungeonManager.minY)
        {
            dungeonManager.minY = transform.position.y;
        }

    }

    private void Start()
    {
        LayerMask EnviromentMask = LayerMask.GetMask("Wall", "Floor");
        Vector2 hitSize = Vector2.one * 0.8f;
        
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 targetPosition = new Vector2(transform.position.x + x, transform.position.y + y);
                Collider2D hit = Physics2D.OverlapBox(targetPosition, hitSize,0, EnviromentMask);
                if (!hit)
                {
                    //Agregar un muro. 
                    GameObject gameObjectWall = Instantiate(dungeonManager.wallsPrefab, targetPosition, Quaternion.identity) as GameObject;
                    gameObjectWall.name = dungeonManager.wallsPrefab.name;
                    gameObjectWall.transform.SetParent(dungeonManager.transform);
                }
            }
        }


        Destroy(gameObject);
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one);

    }


}
