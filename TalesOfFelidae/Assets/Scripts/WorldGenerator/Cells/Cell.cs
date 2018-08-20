using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class Cell : MonoBehaviour
    {
        public int[,] MapCoordinates;
        public int CellWidth;
        public int CellLength;
        public int OverworldLocationX;
        public int OverworldLocationY;
        public Tile[,] TileMap;
        public bool[] Directions = new bool[4];   //TerrainCell thing; 0 is North, 1 is West, 2 is South, 3 is East
        public List<Tile> DoorList;
        public int DifficultyLevel = 0;
        public bool isMachine = false;
        public CellType thisCellType;
        public Lake lakePrefab;
        public GameObject floorPrefab;
        public GameObject wallPrefab;

        public float PercentWalls;  //TerrainCell thing
        public int BirthLimit, DeathLimit;  //TerrainCell thing
        public int SimSteps;    //TerrainCell thing

        void OnEnable()
        {
            for(int c = 0; c < transform.childCount; c++)
            {
                transform.GetChild(c).gameObject.SetActive(true);
            }
        }

        public Cell()
        {
            CellLength = 40;
            CellWidth = 40;

            if (!isMachine) PercentWalls = 30;
            else PercentWalls = 0;
        }

        public Cell(int width, int length, Tile[,] map, float percentWalls = 30)
        {
            CellLength = length;
            CellWidth = width;
            TileMap = new Tile[CellLength, CellWidth];
            TileMap = map;

            if (!isMachine) PercentWalls = percentWalls;
            else PercentWalls = 0;
        }

        public void InitializeTiles()
        {
            //TileMap is organized in coordinates of Y, X

            TileMap = new Tile[CellLength, CellWidth];
            for (int Y = 0; Y < CellLength; Y++)
            {
                for (int X = 0; X < CellWidth; X++)
                {
                    TileMap[Y, X] = new Tile();
                    TileMap[Y, X].mapLocationX = X;
                    TileMap[Y, X].mapLocationY = Y;
                    TileMap[Y, X].tileType = TileType.Wall;
                }
            }
        }
    }

    public enum CellType { Forest, Beach, Cave, Swamp, Mountain }
}
