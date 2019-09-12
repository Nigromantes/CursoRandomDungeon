using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DungeonManager : MonoBehaviour {

    [HideInInspector] public float maxX, minX, maxY, minY;
    public GameObject[] randomItems;
    public GameObject floorPrefab, wallsPrefab, tilePrefab, exitPrefab;
    [Range(100, 5000)] public int TotalFloorCurrent;
    [Range(0, 100)] public int itemSpawnPercen;

    List<Vector3> floorlist = new List<Vector3>();
    LayerMask floorMask, wallMask;
    private void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        wallMask = LayerMask.GetMask("Wall");
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

        Vector2 hitSize = Vector2.one * 0.8f;
        for (int x = (int)minX - 2; x <= (int)maxX + 2; x++)
        {
            for (int y = (int)minY - 2; y <= (int)maxY + 2; y++)
            {
                Collider2D hitFloor = Physics2D.OverlapBox(new Vector2(x, y), Vector2.one * 0.8f, 0, floorMask);
                if (hitFloor)
                {
                    if (!Vector2.Equals(hitFloor.transform.position, floorlist[floorlist.Count - 1]))
                    {
                        Collider2D hitTop = Physics2D.OverlapBox(new Vector2(x, y + 1), hitSize, 0, wallMask);
                        Collider2D hitRight = Physics2D.OverlapBox(new Vector2(x + 1, y), hitSize, 0, wallMask);
                        Collider2D hitButtom = Physics2D.OverlapBox(new Vector2(x, -y), hitSize, 0, wallMask);
                        Collider2D hitLeft = Physics2D.OverlapBox(new Vector2(x - 1, y), hitSize, 0, wallMask);

                        RandomItems(hitFloor, hitTop, hitRight, hitButtom, hitLeft);


                    }

                }
            }
        }
               
    }

    private void RandomItems(Collider2D hitFloor, Collider2D hitTop, Collider2D hitRight, Collider2D hitButtom, Collider2D hitLeft)
    {
        if ((hitTop || hitRight || hitButtom || hitLeft) && !(hitTop && hitButtom) && !(hitRight && hitLeft))
        {
            int roll = Random.Range(0, 101);
            if (roll <= itemSpawnPercen)
            {
                int itemIndex = Random.Range(0, randomItems.Length);
                GameObject GameObjecIteam = Instantiate(randomItems[itemIndex], hitFloor.transform.position, Quaternion.identity) as GameObject;
                GameObjecIteam.name = randomItems[itemIndex].name;
                GameObjecIteam.transform.SetParent(transform);
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
