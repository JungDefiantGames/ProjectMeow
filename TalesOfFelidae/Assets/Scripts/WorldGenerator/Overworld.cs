using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class Overworld : MonoBehaviour
    {
        public int ID;
        public Cell[,] CellMap;    //Organized as [Y, X]

        public void InitializeWorld(int mapWidth = 8, int mapLength = 8)
        {
            CellMap = new Cell[mapLength, mapWidth];

            for (int Y = 0; Y < CellMap.GetLength(0); Y++)
            {
                for(int X = 0; X < CellMap.GetLength(1); X++)
                {
                    CellMap[Y, X] = null;
                }
            }
        }
    }
}

