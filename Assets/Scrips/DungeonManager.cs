using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DungeonManager : MonoBehaviour {

    [HideInInspector] public int maxX, minX, maxY, minY;
    public GameObject[] randomItems;
    public GameObject floorPrefab, wallsPrefab, tilePrefab,exitPrefab;
    public int TotalFloorCurrent;

    List<Vector3> floorlist = new List<Vector3>();
    LayerMask floorMask;
    private void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        RandomWalker();
    }

    private void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    private void RandomWalker()
    {
        Vector3 currentPosition = Vector3.zero;
        floorlist.Add(currentPosition);

        while (floorlist.Count < TotalFloorCurrent)
        {
            switch (Random.Range(1, 5))
            {
                case 1: currentPosition += Vector3.up; break;
                case 2: currentPosition += Vector3.right; break;
                case 3: currentPosition += Vector3.down; break;
                case 4: currentPosition += Vector3.left; break;


            }

            bool inFloorList = false;
            for (int i = 0; i < floorlist.Count; i++)
            {
                if (Vector3.Equals(currentPosition, floorlist[i]))
                {
                    inFloorList = true;
                    break;
                }
            }
            if (!inFloorList)
            {
                floorlist.Add(currentPosition);
            }
                       
        }
       

        for (int i = 0; i < floorlist.Count; i++)
        {
            GameObject GameObjectTile = Instantiate(tilePrefab, floorlist[i], Quaternion.identity) as GameObject;
            GameObjectTile.name = tilePrefab.name;
            GameObjectTile.transform.SetParent(transform);
        }
        StartCoroutine(DelayProgress());

    }

    IEnumerator DelayProgress()
    {
        while (FindObjectsOfType<TileSpawner>().Length > 0)
        {
            yield return null;

        }
        InstanciarExitDoor();
        for (int x = minX - 2; x <= maxX+2; x++)
        {
            for (int y = minY - 2; y <= maxY + 2; y++)
            {
                Collider2D hitFloor = Physics2D.OverlapBox(new Vector2(x,y),Vector2.one *0.8f,Quaternion.identity,floorMasak)
            }
        }

    }

    private void InstanciarExitDoor()
    {
        Vector3 doorPosition = floorlist[floorlist.Count - 1];
        GameObject GameObjecDoor = Instantiate(exitPrefab, doorPosition, Quaternion.identity) as GameObject;
        GameObjecDoor.name = exitPrefab.name;
        GameObjecDoor.transform.SetParent(transform);

    }
}
