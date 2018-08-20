using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    [System.Serializable]
    public class Tile
    {
        public TileType tileType;
        public bool isReachable = false;
        public int mapLocationX;
        public int mapLocationY;
    }
}

