using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{

    [CreateAssetMenu(fileName = "Lake", menuName = "DungeonGenerator/Lake")]
    public class Lake : ScriptableObject
    {

        public int DeathLimit;
        public int BirthLimit;
        public int SimSteps;
        public float MaxPercentLake;
        public GameObject MainTilePrefab;
        public GameObject EdgeTilePrefab;

        public TileType LakeRandomTile(float percent, TileType orig)
        {
            int randomRange = Random.Range(1, 101);
            if (randomRange <= percent)
            {
                return TileType.Lake;
            }
            return orig;
        }

        public int countLakeNeighbors(Cell cell, int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int neigh_x = x + i;
                    int neigh_y = y + j;

                    if (i == 0 && j == 0) { }
                    else if (neigh_x > 1 && neigh_y > 1 && neigh_x < cell.CellWidth-2 && neigh_y < cell.CellLength-2)
                    {
                        if (cell.TileMap[neigh_y, neigh_x].tileType == TileType.Lake)
                        {
                            count++;
                            //Debug.Log("Lake neighbors equal: " + count);
                        }
                    }
                }
            }

            return count;
        }

    }
}
