using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {

    [HideInInspector] public float maxX, minX, maxY, minY;
    public GameObject floorPrefab, wallsPrefab, tilePrefab;
    public int TotalFloorCurrent;

    List<Vector3> floorlist = new List<Vector3>();
    private void Start()
    {
        RandomWalker();
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

    }

}
