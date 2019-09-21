using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public enum DungeonType {Caverns, Rooms, Winding }
public class DungeonManager : MonoBehaviour {

    [HideInInspector] public float maxX, minX, maxY, minY;
    public GameObject[] randomItems, randomEnemies;
    public GameObject floorPrefab, wallsPrefab, tilePrefab, exitPrefab;
    public GameObject[] roundedEdges;
    [Range(100, 5000)] public int TotalFloorCurrent;
    [Range(0, 100)] public int itemSpawnPercen;
    [Range(0, 100)] public int EnemySpawnPercen;

    List<Vector3> floorlist = new List<Vector3>();
    LayerMask floorMask, wallMask;
    public bool useRoundedEdges;

    public DungeonType dungeonType;

    private Vector2 hitSize;
    private void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        wallMask = LayerMask.GetMask("Wall");
        hitSize = Vector2.one * 0.8f;

        switch (dungeonType)
        {
            case DungeonType.Caverns: RandomWalker();
                break;
            case DungeonType.Rooms: RoomWalker();
                break;
            case DungeonType.Winding: WingWalker();
                break;

        }



        
    }

    //private void WindigWalker()
    //{
    //    throw new NotImplementedException();
    //}

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
            currentPosition += RandomDirection();
           

           
            if (!InFloorList(currentPosition))
            {
                floorlist.Add(currentPosition);
            }

        }

               
        StartCoroutine(DelayProgress());

    }


    private void RoomWalker()
    {
        Vector3 currentPosition = Vector3.zero;
        floorlist.Add(currentPosition);


        while (floorlist.Count < TotalFloorCurrent)
        {
            currentPosition = TakeAHike(currentPosition);
            RandomRoom(currentPosition);
        }


      
        StartCoroutine(DelayProgress());
    }


    private void WingWalker()
    {
        Vector3 currentPosition = Vector3.zero;
        floorlist.Add(currentPosition);


        while (floorlist.Count < TotalFloorCurrent)
        {
            currentPosition = TakeAHike(currentPosition);

            int roll = Random.Range(0, 100);
            if (roll<50)
            {
                RandomRoom(currentPosition);
            }
                       

        }



        StartCoroutine(DelayProgress());
    }

    private Vector3 TakeAHike(Vector3 currentPosition)
    {
        Vector3 walkDirection = RandomDirection();
        int walkLength = Random.Range(9, 18);
        for (int i = 0; i < walkLength; i++)
        {
            if (!InFloorList(currentPosition + walkDirection))
            {
                floorlist.Add(currentPosition + walkDirection);
            }
            currentPosition += walkDirection;
        }

        return currentPosition;
    }

    private void RandomRoom(Vector3 myPosision)
    {
        int width = Random.Range(1, 5);
        int heigth = Random.Range(1, 5);
        for (int w = -width; w <= width; w++)
        {
            for (int h = -heigth; h <= heigth; h++)
            {
                Vector3 offset = new Vector3(w, h, 0);
                if (!InFloorList(myPosision + offset))
                {
                    floorlist.Add(myPosision + offset);
                }

            }
        }
    }

    bool InFloorList(Vector3 myPosition)
    {
                
        for (int i = 0; i < floorlist.Count; i++)
        {
            if (Vector3.Equals(myPosition, floorlist[i]))
            {
                return true;
                
            }
        }
        return false;

    }

    private Vector3 RandomDirection()
    {
        switch (Random.Range(1, 5))
        {
            case 1: return Vector3.up; 
            case 2: return Vector3.right; 
            case 3: return Vector3.down; 
            case 4: return Vector3.left; 
        }
        return Vector3.zero;
    }

    IEnumerator DelayProgress()
    {
        for (int i = 0; i < floorlist.Count; i++)
        {
            GameObject GameObjectTile = Instantiate(tilePrefab, floorlist[i], Quaternion.identity) as GameObject;
            GameObjectTile.name = tilePrefab.name;
            GameObjectTile.transform.SetParent(transform);
        }


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
                        RandomEnemies(hitFloor, hitTop, hitRight, hitButtom, hitLeft);


                    }

                }
                RoundedEdges(x, y);
            }
        }
               
    }

    private void RoundedEdges(int x, int y)
    {
        if (useRoundedEdges)
        {
            Collider2D hitWall = Physics2D.OverlapBox(new Vector2(x, y), hitSize, 0, wallMask);
            if (hitWall)
            {
                Collider2D hitTop = Physics2D.OverlapBox(new Vector2(x, y + 1), hitSize, 0, wallMask);
                Collider2D hitRight = Physics2D.OverlapBox(new Vector2(x + 1, y), hitSize, 0, wallMask);
                Collider2D hitButtom = Physics2D.OverlapBox(new Vector2(x, y - 1), hitSize, 0, wallMask);
                Collider2D hitLeft = Physics2D.OverlapBox(new Vector2(x - 1, y), hitSize, 0, wallMask);
                int bitVal = 0;

                if (!hitTop)
                {
                    bitVal += 1;
                }

                if (!hitRight)
                {
                    bitVal += 2;
                }

                if (!hitButtom)
                {
                    bitVal += 4;
                }

                if (!hitLeft)
                {
                    bitVal += 8;
                }

                if (bitVal > 0)
                {
                    GameObject gameObjecEdge = Instantiate(roundedEdges[bitVal], new Vector2(x, y), Quaternion.identity) as GameObject;
                    gameObjecEdge.name = roundedEdges[bitVal].name;
                    gameObjecEdge.transform.SetParent(transform);

                }



            }

        }
    }

    private void RandomEnemies(Collider2D hitFloor, Collider2D hitTop, Collider2D hitRight, Collider2D hitButtom, Collider2D hitLeft)
    {
        if (!hitTop && !hitRight && !hitButtom && !hitLeft)
        {
            int roll = Random.Range(1, 101);
            if (roll <= EnemySpawnPercen)
            {
                int EnemyIndex = Random.Range(0, randomEnemies.Length);
                GameObject GameObjecEnemy = Instantiate(randomEnemies[EnemyIndex], hitFloor.transform.position, Quaternion.identity) as GameObject;
               
            }
        }
    }

    private void RandomItems(Collider2D hitFloor, Collider2D hitTop, Collider2D hitRight, Collider2D hitButtom, Collider2D hitLeft)
    {
        if ((hitTop || hitRight || hitButtom || hitLeft) && !(hitTop && hitButtom) && !(hitRight && hitLeft))
        {
            int roll = Random.Range(1, 101);
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
