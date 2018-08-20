using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace DungeonGenerator
{

    public enum TileType { Floor, Wall, Lake, Bridge, Door, SpawnPoint, Chest }

    public class OverworldManager : MonoBehaviour
    {

        public List<GameObject> cellPrefabs;
        public Overworld overworldPrefab;
        public float cellOffset;    //this equals the size of the cells
        public float tileOffset;
        public int maxDungeons;
        List<Vector2> validMachineTiles;
        //[HideInInspector]
        public Overworld thisOverworld;

        public void WipeOverworld()
        {
            DestroyImmediate(thisOverworld.gameObject, true);
        }

        public Overworld GenerateOverworld()
        {
            validMachineTiles = new List<Vector2>();
            thisOverworld = Instantiate(overworldPrefab, transform);
            thisOverworld.InitializeWorld(8, 8);

            GenerateTown(thisOverworld);
            //PlaceShrines(thisOverworld);
            GenerateTerrainCells(thisOverworld); //Assign difficulty levels

            validMachineTiles = GenerateValidMachineTilesList(thisOverworld);
            int numDungeons = 0;
            if(validMachineTiles.Count > 0)
            while(numDungeons < maxDungeons) { //(thisOverworld.CellMap.GetLength(0) * thisOverworld.CellMap.GetLength(1)) / 32) {
                    numDungeons += GenerateDungeonCells(thisOverworld);
                    if (validMachineTiles.Count < 1) numDungeons = maxDungeons;
            }
            //AssignItemLocations(thisOverworld);   //These are assigned to wilderness cells; 1-star equipment item guarded by powerful creature; one entrance
            //AssignWitchHouse(thisOverworld);    //Her house moves every day at midnight; any 'empty' space
            //
            //GenerateCellObjects(thisOverworld);
            //Debug.Log("Cells Generated!");
            //AssignDoors(thisOverworld);
            //Debug.Log("Doors added and dungeon cleaned up!");
            //SaveOverworld();

            return thisOverworld;
        }

        public void GenerateCellObjects(Overworld world)
        {
            for (int Y = 0; Y < world.CellMap.GetLength(0); Y++)
            {
                for (int X = 0; X < world.CellMap.GetLength(1); X++)
                {
                    if (!world.CellMap[Y, X].GetComponent<Cell>().isMachine)
                    {
                        //DoorList = new List<Tile>();
                        world.CellMap[Y, X].InitializeTiles();
                        world.CellMap[Y, X] = RandomFillDungeon(world.CellMap[Y, X]);
                        world.CellMap[Y, X] = SimulateDungeonGrowth(world.CellMap[Y, X]);
                        //GenerateMachine(machinePrefab, exampleMap);
                        //PlaceDoors(cell);
                        //CleanUpDungeon(world.CellMap[Y, X]);
                        //PlaceBridges(exampleMap);
                        //PlaceChests(exampleMap);
                        //PlaceMonsters(exampleMap);
                        //PlaceTraps(exampleMap);

                        Debug.Log("Cell created!");
                    }
                }
            }

        }

        public void AssignDoors(Overworld world)
        {
            Debug.Log("Assigning Doors");

            for (int Y = 0; Y < world.CellMap.GetLength(0); Y++)
            {
                for (int X = 0; X < world.CellMap.GetLength(1); X++)
                {
                    if(!world.CellMap[Y, X].isMachine)
                    {
                        //check if out of bounds; turn off doors for cells pointing in that direction
                        if (Y - 1 < 0) world.CellMap[Y, X].Directions[0] = false;
                        else world.CellMap[Y, X].Directions[0] = world.CellMap[Y - 1, X].Directions[2];

                        if (Y + 1 > world.CellMap.GetLength(0) - 1) world.CellMap[Y, X].Directions[2] = false;
                        else world.CellMap[Y, X].Directions[2] = world.CellMap[Y + 1, X].Directions[0];

                        if (X - 1 < 0) world.CellMap[Y, X].Directions[1] = false;
                        else world.CellMap[Y, X].Directions[1] = world.CellMap[Y, X - 1].Directions[3];

                        if (X + 1 > world.CellMap.GetLength(1) - 1) world.CellMap[Y, X].Directions[3] = false;
                        else world.CellMap[Y, X].Directions[3] = world.CellMap[Y, X + 1].Directions[0];

                        world.CellMap[Y, X] = PlaceDoors(world.CellMap[Y, X]);
                        ClearPathBetweenDoors(world.CellMap[Y, X]);
                    }
                }
            }
        }

        public void GenerateLake(Cell cell)
        {

            float lakePercent = Random.Range(cell.lakePrefab.MaxPercentLake / 2, cell.lakePrefab.MaxPercentLake);
            //float lakePercent = lake.MaxPercentLake;

            for (int Y = 3; Y < cell.CellLength - 3; Y++)
            {
                for (int X = 3; X < cell.CellWidth - 3; X++)
                {
                    if (X > 2 && Y > 2 && X < cell.CellWidth - 2 && Y < cell.CellLength - 2 && cell.TileMap[Y, X].tileType != TileType.Door && cell.TileMap[Y + 1, X].tileType != TileType.Door &&
                        cell.TileMap[Y - 1, X].tileType != TileType.Door && cell.TileMap[Y, X + 1].tileType != TileType.Door && cell.TileMap[Y, X - 1].tileType != TileType.Door)
                        //cell.TileMap[lakeSeedY, lakeSeedX].tileType = TileType.Lake; 
                        cell.TileMap[Y, X].tileType = cell.lakePrefab.LakeRandomTile(lakePercent, cell.TileMap[Y, X].tileType);
                }
            }


            int convertcount = 0;
            for (int Y = 3; Y < cell.CellLength - 3; Y++)
            {
                for (int X = 3; X < cell.CellWidth - 3; X++)
                {
                    for (int s = 0; s < cell.lakePrefab.SimSteps; s++)
                    {
                        int neighbors = cell.lakePrefab.countLakeNeighbors(cell, X, Y);

                        if (cell.TileMap[Y, X].tileType == TileType.Lake)
                        {
                            //if tile is surrounded by number of Lake cells LESS THAN Death Limit, they die
                            if (neighbors < cell.lakePrefab.DeathLimit)
                            {
                                cell.TileMap[Y, X].tileType = TileType.Wall;
                            }
                            else
                            {
                                cell.TileMap[Y, X].tileType = TileType.Lake;
                                convertcount++;
                            }
                        }
                        else
                        {
                            //if tile is surrounded by number of Lake cells MORE THAN Birth Limit, they come alive
                            if (neighbors > cell.lakePrefab.BirthLimit)
                            {
                                cell.TileMap[Y, X].tileType = TileType.Lake;
                                convertcount++;
                            }
                            //else cell.TileMap[Y, X].tileType = RandomTile(cell.PercentWalls);
                        }
                    }
                }
            }

            Debug.Log("Conversion Count = " + convertcount++);
        }

        public void CleanUpOverworld(Overworld world)
        {
            for (int Y = 0; Y < world.CellMap.GetLength(0); Y++)
            {
                for (int X = 0; X < world.CellMap.GetLength(1); X++)
                {
                    if (!world.CellMap[Y, X].GetComponent<Cell>().isMachine)
                    {
                        GenerateLake(world.CellMap[Y, X]);
                        ConvertOrphanTiles(world.CellMap[Y, X]);
                        RemoveDiagonals(world.CellMap[Y, X], 10);
                        Debug.Log("Overworld cleaned up!");
                    }
                }
            }
            
        }

        public void GenerateTileObjects(Overworld world)
        {
            //PrintMap();

            for (int Y = 0; Y < world.CellMap.GetLength(0); Y++)
            {
                for (int X = 0; X < world.CellMap.GetLength(1); X++)
                {
                    GenerateTilesInCell(world.CellMap[Y, X].GetComponent<Cell>());
                }
            }

        }

        //.....Cell Generation Functions.....//

        void GenerateTown(Overworld world)
        {
            int townStartY = world.CellMap.GetLength(0) - 2;
            int townStartX = (world.CellMap.GetLength(1) / 2) - 1;

            world.CellMap[townStartY, townStartX] = Instantiate(GetCellPrefabByName("Town"), CellCoordToScene(townStartX, townStartY), Quaternion.identity, world.transform).GetComponent<Cell>();
            world.CellMap[townStartY, townStartX].gameObject.SetActive(false);
            world.CellMap[townStartY, townStartX].OverworldLocationX = townStartX;
            world.CellMap[townStartY, townStartX].OverworldLocationY = townStartY;
            world.CellMap[townStartY, townStartX + 1] = Instantiate(GetCellPrefabByName("Placeholder"), CellCoordToScene(townStartX+1, townStartY), Quaternion.identity, world.transform).GetComponent<Cell>();
            world.CellMap[townStartY, townStartX + 1].OverworldLocationX = townStartX + 1;
            world.CellMap[townStartY, townStartX + 1].OverworldLocationY = townStartY;
             world.CellMap[townStartY + 1, townStartX] = Instantiate(GetCellPrefabByName("Placeholder"), CellCoordToScene(townStartX, townStartY+1), Quaternion.identity, world.transform).GetComponent<Cell>();
            world.CellMap[townStartY + 1, townStartX].OverworldLocationX = townStartX;
            world.CellMap[townStartY + 1, townStartX].OverworldLocationY = townStartY + 1;
            world.CellMap[townStartY + 1, townStartX + 1] = Instantiate(GetCellPrefabByName("Placeholder"), CellCoordToScene(townStartX+1, townStartY+1), Quaternion.identity, world.transform).GetComponent<Cell>();
            world.CellMap[townStartY + 1, townStartX + 1].OverworldLocationX = townStartX + 1;
            world.CellMap[townStartY + 1, townStartX + 1].OverworldLocationY = townStartY + 1;
            world.CellMap[townStartY, townStartX + 1].thisCellType = CellType.Forest;
            world.CellMap[townStartY + 1, townStartX].thisCellType = CellType.Forest;
            world.CellMap[townStartY + 1, townStartX + 1].thisCellType = CellType.Forest;
        }

        void PlaceShrines(Overworld world)
        {
            int numShrines = (world.CellMap.GetLength(0) * world.CellMap.GetLength(1)) / 32;

            //if (numShrines <= 2)
            for (int s = 0; s < numShrines; s++)
            {
                world.CellMap[s, s] = Instantiate(GetCellPrefabByName("Shrine"), transform).GetComponent<Cell>();
                world.CellMap[s, s].GetComponent<Cell>().thisCellType = CellType.Forest;

                /*
                switch (s)
                {
                    case 0:
                            world.CellMap[3, 1] = Instantiate(GetCellPrefabByName("Shrine"), transform);
                            world.CellMap[3, 1].GetComponent<Cell>().thisCellType = CellType.Forest;
                            break;
                    case 1:
                            world.CellMap[3, 6] = Instantiate(GetCellPrefabByName("Shrine"), transform);
                            world.CellMap[3, 6].GetComponent<Cell>().thisCellType = CellType.Forest;
                            break;
                }*/
            }
        }

        void GenerateTerrainCells(Overworld world)
        {
            for (int Y = 0; Y < world.CellMap.GetLength(0); Y++)
            {
                for (int X = 0; X < world.CellMap.GetLength(1); X++)
                {
                    if (world.CellMap[Y, X] == null)
                    {
                        GameObject newCell = Instantiate(GetCellPrefabByName("Forest"), CellCoordToScene(X, Y), Quaternion.identity, world.transform);
                        world.CellMap[Y, X] = newCell.GetComponent<Cell>();
                        world.CellMap[Y, X].GetComponent<Cell>().DifficultyLevel = 0;
                        world.CellMap[Y, X].GetComponent<Cell>().OverworldLocationX = X;
                        world.CellMap[Y, X].GetComponent<Cell>().OverworldLocationY = Y;
                    }
                }
            }
        }

        int GenerateDungeonCells(Overworld world)
        {
            if (validMachineTiles.Count < 1)
            {
                Debug.Log("No valid machine tiles! " + System.DateTime.Now);
                return 0;
            }

            int randomInd = Random.Range(0, validMachineTiles.Count - 1);
            int randomTileX = Mathf.RoundToInt(validMachineTiles[randomInd].x);
            int randomTileY = Mathf.RoundToInt(validMachineTiles[randomInd].y);

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (randomTileY + i >= 0 && randomTileX + j >= 0 && randomTileY + i <= world.CellMap.GetLength(0) - 1 && randomTileX + j <= world.CellMap.GetLength(0) - 1)
                        if (world.CellMap[randomTileY + i, randomTileX + j].isMachine)
                        {
                            validMachineTiles.RemoveAt(randomInd);
                            Debug.Log("Not a valid machine tile! " + System.DateTime.Now);
                            return 0;
                        }
                }
            }

            CellType machineCellType = world.CellMap[randomTileY, randomTileX].GetComponent<Cell>().thisCellType;
            world.CellMap[randomTileY, randomTileX] = Instantiate(GetCellPrefabByName("Machine"), CellCoordToScene(randomTileX, randomTileY), Quaternion.identity, world.transform).GetComponent<Cell>();
            world.CellMap[randomTileY, randomTileX].gameObject.SetActive(false);
            //world.CellMap[randomTileY, randomTileX + 1] = Instantiate(GetCellPrefabByName("Placeholder"), CellCoordToScene(randomTileX, randomTileY), Quaternion.identity, transform);
            //world.CellMap[randomTileY + 1, randomTileX] = Instantiate(GetCellPrefabByName("Placeholder"), CellCoordToScene(randomTileX, randomTileY), Quaternion.identity, transform);
            //world.CellMap[randomTileY + 1, randomTileX + 1] = Instantiate(GetCellPrefabByName("Placeholder"), CellCoordToScene(randomTileX, randomTileY), Quaternion.identity, transform);
            world.CellMap[randomTileY, randomTileX].GetComponent<Cell>().thisCellType = machineCellType;
            //world.CellMap[randomTileY, randomTileX + 1].GetComponent<Cell>().thisCellType = machineCellType;
            //world.CellMap[randomTileY + 1, randomTileX].GetComponent<Cell>().thisCellType = machineCellType;
            //world.CellMap[randomTileY + 1, randomTileX + 1].GetComponent<Cell>().thisCellType = machineCellType;
            world.CellMap[randomTileY, randomTileX].GetComponent<Cell>().OverworldLocationX = randomTileX;
            world.CellMap[randomTileY, randomTileX].GetComponent<Cell>().OverworldLocationY = randomTileY;
            validMachineTiles.RemoveAt(randomInd);
            Debug.Log("New machine generated! " + System.DateTime.Now);
            return 1;
        }

        //.....Tile Generation Functions.....//

        Cell RandomFillDungeon(Cell cell)
        {
            for (int Y = 0; Y < cell.CellLength; Y++)
            {
                for (int X = 0; X < cell.CellWidth; X++)
                {

                    cell.TileMap[X, Y].tileType = RandomTile(cell.PercentWalls);

                    if (X == 0)
                    {
                        cell.TileMap[X, Y].tileType = TileType.Wall;
                    }
                    if (Y == 0)
                    {
                        cell.TileMap[X, Y].tileType = TileType.Wall;
                    }
                    if (X == cell.CellWidth - 1)
                    {
                        cell.TileMap[X, Y].tileType = TileType.Wall;
                    }
                    if (Y == cell.CellLength - 1)
                    {
                        cell.TileMap[X, Y].tileType = TileType.Wall;
                    }

                }
            }

            return cell;
        }

        Cell SimulateDungeonGrowth(Cell oldMap)
        {
            Cell newMap = oldMap;
            //newMap.InitializeTiles();

            for (int i = 0; i < oldMap.SimSteps; i++)
            {
                for (int Y = 1; Y < oldMap.CellLength - 1; Y++)
                {
                    for (int X = 1; X < oldMap.CellWidth - 1; X++)
                    {
                        int neighbors = countFloorNeighbors(oldMap, X, Y);

                        if (oldMap.TileMap[Y, X].tileType == TileType.Floor)
                        {
                            if (neighbors < oldMap.DeathLimit)
                            {
                                newMap.TileMap[Y, X].tileType = TileType.Wall;
                            }
                            else
                            {
                                newMap.TileMap[Y, X].tileType = TileType.Floor;
                            }
                        }
                        else if (oldMap.TileMap[Y, X].tileType == TileType.Wall)
                        {
                            if (neighbors > oldMap.BirthLimit)
                            {
                                newMap.TileMap[Y, X].tileType = TileType.Floor;
                            }
                            else newMap.TileMap[Y, X].tileType = TileType.Wall;
                        }
                    }
                }
            }

            return newMap;
        }

        Cell PlaceDoors(Cell cell)
        {
            if (cell.DoorList == null) cell.DoorList = new List<Tile>();

            if(!cell.isMachine)
            if (cell.Directions != null && cell.Directions.Length == 4)
            {
                if (cell.Directions[0])
                {
                    cell.TileMap[0, cell.CellWidth / 2].tileType = TileType.Door;
                    cell.TileMap[0, (cell.CellWidth / 2) + 1].tileType = TileType.Door;
                    cell.DoorList.Add(cell.TileMap[0, cell.CellWidth / 2]);
                    cell.DoorList.Add(cell.TileMap[0, (cell.CellWidth / 2) + 1]);
                    Debug.Log("Door assigned!");
                }
                if (cell.Directions[1])
                {
                    cell.TileMap[cell.CellLength / 2, 0].tileType = TileType.Door;
                    cell.TileMap[(cell.CellLength / 2) + 1, 0].tileType = TileType.Door;
                    cell.DoorList.Add(cell.TileMap[cell.CellLength / 2, 0]);
                    cell.DoorList.Add(cell.TileMap[(cell.CellLength / 2) + 1, 0]);
                    Debug.Log("Door assigned!");
                }
                if (cell.Directions[2])
                {
                    cell.TileMap[cell.CellLength - 1, cell.CellWidth / 2].tileType = TileType.Door;
                    cell.TileMap[cell.CellLength - 1, (cell.CellWidth / 2) + 1].tileType = TileType.Door;
                    cell.DoorList.Add(cell.TileMap[cell.CellLength - 1, cell.CellWidth / 2]);
                    cell.DoorList.Add(cell.TileMap[cell.CellLength - 1, (cell.CellWidth / 2) + 1]);
                    Debug.Log("Door assigned!");
                }
                if (cell.Directions[3])
                {
                    cell.TileMap[cell.CellLength / 2, cell.CellWidth - 1].tileType = TileType.Door;
                    cell.TileMap[(cell.CellLength / 2) + 1, cell.CellWidth - 1].tileType = TileType.Door;
                    cell.DoorList.Add(cell.TileMap[cell.CellLength / 2, cell.CellWidth - 1]);
                    cell.DoorList.Add(cell.TileMap[(cell.CellLength / 2) + 1, cell.CellWidth - 1]);
                    Debug.Log("Door assigned!");
                }
            }
            return cell;
        }

        public void ClearPathBetweenDoors(Cell cell)
        {
            Debug.Log("Clearing path to door!");

            /*
            Tile masterDoor = cell.DoorList[Random.Range(0, cell.DoorList.Count - 1)];
            int connectedDoors = 1;

            while (connectedDoors < cell.DoorList.Count)
            {
                foreach (Tile door in cell.DoorList)
                {
                    if (door.mapLocationY != masterDoor.mapLocationY && door.mapLocationX != masterDoor.mapLocationX)
                        if (ExpandToTargetTile(cell, masterDoor, door))
                        {
                            connectedDoors++;
                        }
                }
            }   */

            int centerX = cell.CellLength / 2;
            int centerY = cell.CellWidth / 2;

            foreach(Tile door in cell.DoorList)
            {
                Tile closestTile = door;
                float closestDistance = cell.CellLength * cell.CellWidth;
                bool destReached = false;

                while(!destReached)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            int neigh_x = closestTile.mapLocationX + i;
                            int neigh_y = closestTile.mapLocationY + j;

                            if (i == 0 && j == 0) { }
                             else if (i != 0 && j != 0) {
                                if (neigh_x > 0 && neigh_y > 0 && neigh_x < cell.CellWidth - 1 && neigh_y < cell.CellLength - 1)
                                    if(cell.TileMap[neigh_y, neigh_x].tileType == TileType.Wall) cell.TileMap[neigh_y, neigh_x].tileType = TileType.Floor;
                            }
                            else if (neigh_x > 0 && neigh_y > 0 && neigh_x < cell.CellWidth - 1 && neigh_y < cell.CellLength - 1)
                            {
                                if (cell.TileMap[neigh_y, neigh_x].tileType == TileType.Wall) cell.TileMap[neigh_y, neigh_x].tileType = TileType.Floor;

                                float tileDistance = CalculateDistance(cell.TileMap[neigh_y, neigh_x].mapLocationX, cell.TileMap[neigh_y, neigh_x].mapLocationY, centerX, centerY);
                                if(tileDistance <= closestDistance)
                                {
                                    closestDistance = tileDistance;
                                    closestTile = cell.TileMap[neigh_y, neigh_x];
                                    Debug.Log("Found closest tile");
                                }
                            }

                        }
                    }

                    //if(closestTile.tileType == TileType.Wall) closestTile.tileType = TileType.Floor;
                    Debug.Log(CalculateDistance(closestTile.mapLocationX, closestTile.mapLocationY, centerX, centerY));

                    if (closestDistance < 1) destReached = true;
                }
            }
        }

        void RemoveDiagonals(Cell cell, int steps)
        {

            //Check for diagonals and convert walls to floors
            for (int s = 0; s < steps; s++)
            {
                for (int Y = 1; Y < cell.CellLength-1; Y++)
                {
                    for (int X = 1; X < cell.CellWidth-1; X++)
                    {
                        if (cell.TileMap[Y, X].tileType == TileType.Floor)
                        {
                            if ((Y + 1) < cell.CellLength - 1 && (X + 1) < cell.CellWidth - 1)
                            {
                                if (cell.TileMap[Y + 1, X].tileType == TileType.Wall && cell.TileMap[Y, X + 1].tileType == TileType.Wall
                                    && cell.TileMap[Y + 1, X + 1].tileType == TileType.Floor)
                                {
                                    cell.TileMap[Y + 1, X].tileType = TileType.Floor;
                                    cell.TileMap[Y, X + 1].tileType = TileType.Floor;
                                }
                            }
                        }

                        if (cell.TileMap[Y, X].tileType == TileType.Wall)
                        {
                            if ((Y + 1) < cell.CellLength - 1 && (X + 1) < cell.CellWidth - 1)
                            {
                                if (cell.TileMap[Y + 1, X].tileType == TileType.Floor && cell.TileMap[Y, X + 1].tileType == TileType.Floor
                                    && cell.TileMap[Y + 1, X + 1].tileType == TileType.Wall)
                                {
                                    cell.TileMap[Y, X].tileType = TileType.Floor;
                                    cell.TileMap[Y + 1, X + 1].tileType = TileType.Floor;
                                }
                            }
                        }
                    }
                }
            }

            //Convert orphan walls into floors
            for (int Y = 1; Y < cell.CellLength - 1; Y++)
            {
                for (int X = 1; X < cell.CellWidth - 1; X++)
                {
                    if (cell.TileMap[Y, X].tileType == TileType.Wall && countFloorNeighbors(cell, X, Y) > 5)
                    {
                        cell.TileMap[Y, X].tileType = TileType.Floor;
                    }
                }
            }
        }

        void ConvertAdjacentTiles(Cell cell, int x, int y, TileType oldType, TileType newType)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int neigh_x = x + i;
                    int neigh_y = y + j;

                    if (i == 0 && j == 0) { }
                    else if (neigh_x > 0 && neigh_y > 0 && neigh_x < cell.CellWidth - 1 && neigh_y < cell.CellLength - 1)
                    {
                        if (cell.TileMap[neigh_y, neigh_x].tileType == oldType)
                        {
                            cell.TileMap[neigh_y, neigh_x].tileType = newType;
                        }
                    }

                }
            }
        }

        void GenerateTilesInCell(Cell cell)
        {
            for (int Y = 0; Y < cell.CellLength; Y++)
            {
                for (int X = 0; X < cell.CellWidth; X++)
                {
                    if(!cell.isMachine)
                    switch (cell.TileMap[Y, X].tileType)
                    {
                        case TileType.Floor:
                            Instantiate(cell.floorPrefab, TileCoordToScene(cell, X, Y), Quaternion.identity, cell.transform).SetActive(false);
                            break;
                        case TileType.Wall:
                            Instantiate(cell.wallPrefab, TileCoordToScene(cell, X, Y), Quaternion.identity, cell.transform).SetActive(false);
                            break;
                        case TileType.Lake:
                            //Instantiate(cell.lakePrefab.MainTilePrefab, TileCoordToScene(cell, X, Y), Quaternion.identity, transform);
                            break;
                        default: break;
                    }
                }
            }

            cell.gameObject.SetActive(false);
        }

        void ConvertOrphanTiles(Cell cell)
        {
            for (int Y = 0; Y < cell.CellLength; Y++)
            {
                for (int X = 0; X < cell.CellWidth; X++)
                {
                    if (Y > 1 && Y < cell.CellLength - 2 && X > 1 && X < cell.CellWidth - 2)
                    {
                        //if (!cell.TileMap[Y, X].isReachable)
                        //cell.TileMap[Y, X].tileType = TileType.Wall;

                        if (!cell.isMachine)
                        for (int steps = 0; steps < 2; steps++)
                        {
                            //Convert Orphan Floor/Wall Tiles near Lakes; try twice
                            if (cell.TileMap[Y, X].tileType == TileType.Floor || cell.TileMap[Y, X].tileType == TileType.Wall)
                            {
                                int lakeCount = 0;

                                for (int i = -1; i < 2; i++)
                                {
                                    for (int j = -1; j < 2; j++)
                                    {

                                        int neigh_x = cell.TileMap[Y, X].mapLocationX + i;
                                        int neigh_y = cell.TileMap[Y, X].mapLocationY + j;

                                        if (i == 0 && j == 0) { }
                                        else if (neigh_x < 2 || neigh_y < 2 || neigh_x >= cell.CellWidth - 2 || neigh_y >= cell.CellLength - 2) { }
                                        else if (cell.TileMap[neigh_y, neigh_x].tileType == TileType.Lake)
                                        {
                                            lakeCount++;
                                        }
                                    }
                                }

                                if (lakeCount > 1) cell.TileMap[Y, X].tileType = TileType.Lake;
                            }

                        }

                        // Convert Orphan Lake Tiles
                        for (int steps = 0; steps < 2; steps++)
                        {

                            if (cell.TileMap[Y, X].tileType == TileType.Lake)
                            {
                                int floorwallCount = 0;

                                for (int i = -1; i < 2; i++)
                                {
                                    for (int j = -1; j < 2; j++)
                                    {

                                        int neigh_x = cell.TileMap[Y, X].mapLocationX + i;
                                        int neigh_y = cell.TileMap[Y, X].mapLocationY + j;

                                        if (i == 0 && j == 0) { }   //(i != 0 || j != 0) ||
                                        else if (neigh_x < 2 || neigh_y < 2 || neigh_x >= cell.CellWidth - 2 || neigh_y >= cell.CellLength - 2) { }
                                        else if (cell.TileMap[neigh_y, neigh_x].tileType == TileType.Floor || cell.TileMap[neigh_y, neigh_x].tileType == TileType.Wall)
                                        {
                                            floorwallCount++;
                                        }
                                    }
                                }

                                if (floorwallCount > 5) cell.TileMap[Y, X].tileType = TileType.Floor;
                            }
                        }
                    }

                }
            }
        }

        /*bool ExpandToTargetTile(Cell cell, Tile origin, Tile destination)
        {
            Debug.Log("Expanding to target tile.");
            List<Tile> DiscoveredSet = new List<Tile>();
            List<Tile> FrontierSet = new List<Tile>();

            //perform breadth first search; if frontier set equals coordinates of door return true; if search is exhausted return false
            //find frontier tile closest to door and convert nearby wall tiles to floor tiles
            DiscoveredSet.Add(origin);
            FrontierSet.Add(origin);
            origin.isReachable = true;
            while (FrontierSet.Count > 0)
            {
                for (int t = 0; t < FrontierSet.Count - 1; t++)
                {
                    Tile currentTile = FrontierSet[t];
                    FrontierSet.RemoveAt(t);

                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            int neigh_x = currentTile.mapLocationX + i;
                            int neigh_y = currentTile.mapLocationY + j;

                            if (i == 0 && j == 0) { }
                            else if (neigh_x > 0 && neigh_y > 0 && neigh_x < cell.CellWidth - 1 && neigh_y < cell.CellLength - 1)
                            {
                                if (cell.TileMap[neigh_y, neigh_x].mapLocationY == destination.mapLocationY 
                                    && cell.TileMap[neigh_y, neigh_x].mapLocationX == destination.mapLocationX)
                                {
                                    Debug.Log("Cell found!");
                                    return true;
                                }
                                else if (cell.TileMap[neigh_y, neigh_x].tileType == TileType.Floor || cell.TileMap[neigh_y, neigh_x].tileType == TileType.Lake)
                                {
                                    FrontierSet.Add(cell.TileMap[neigh_y, neigh_x]);
                                    DiscoveredSet.Add(cell.TileMap[neigh_y, neigh_x]);
                                    cell.TileMap[neigh_y, neigh_x].isReachable = true;
                                }
                            }

                        }
                    }
                }

                if (FrontierSet.Count < 1)
                {
                    float closestDistance = CalculateDistance(cell.CellWidth, cell.CellLength, 0, 0);
                    Tile closestTile = null;

                    for (int t = 0; t < DiscoveredSet.Count - 1; t++)
                    {
                        if (closestTile.tileType == TileType.Floor)
                        {
                            float distance = CalculateDistance(DiscoveredSet[t].mapLocationX, DiscoveredSet[t].mapLocationY, destination.mapLocationX, destination.mapLocationY);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestTile = DiscoveredSet[t];
                            }
                        }      
                    }

                    if (closestTile != null)
                    {
                        ConvertAdjacentTiles(cell, closestTile.mapLocationX, closestTile.mapLocationY, TileType.Wall, TileType.Floor);
                        FrontierSet.Add(closestTile);
                    }
                    else return false;
                }
            }

            return false;
        }*/

        /*void PlaceDoorsRandom(Cell cell)
        {
            List<Tile> DoorCandidateList = new List<Tile>();

            //1. find coordinate of adjacent cell's door
            //2. place door at adjacent cell's door
            //3. if no adjacent cell exists, do not place door at that edge
            //4. if adjacent cell exists and adjacent cell does not have a door, place a door at random point on edge
            //4. apply special rules for doors depending on CellType

            //gather all stair candidates in a list, ensure they follow rules for stair placement, then pick randomly

            for(int checkY = 0, checkX = 0; checkY < cell.CellLength; checkY++)
            {
                for(checkX = 0; checkX < cell.CellWidth; checkX++)
                {
                    if ((checkX > 0 || checkX < cell.CellWidth - 1) && checkY == 0) DoorCandidateList.Add(cell.TileMap[checkY, checkX]);
                    else if ((checkX > 0 || checkX < cell.CellWidth - 1) && checkY == cell.CellLength) DoorCandidateList.Add(cell.TileMap[checkY, checkX]);
                    else if ((checkY > 0 || checkY < cell.CellLength - 1) && checkX == 0) DoorCandidateList.Add(cell.TileMap[checkY, checkX]);
                    else if ((checkY > 0 || checkY < cell.CellLength - 1) && checkX == cell.CellWidth) DoorCandidateList.Add(cell.TileMap[checkY, checkX]);
                }
            }

            if(DoorCandidateList.Count > 0)
            {
                int randomDoor = Random.Range(0, DoorCandidateList.Count - 1);
                int doorX = DoorCandidateList[randomDoor].mapLocationX;
                int doorY = DoorCandidateList[randomDoor].mapLocationY;
                Debug.Log("Tile chosen is a " + cell.TileMap[doorY, doorX].tileType);
                cell.TileMap[doorY, doorX].tileType = TileType.Door;
                DoorList.Add(cell.TileMap[doorY, doorX]);
                
                if ((doorX > 0 | doorX < cell.CellWidth - 1) & doorY == 0) cell.TileMap[doorY+1, doorX].tileType = TileType.Floor;
                else if ((doorX > 0 | doorX < cell.CellWidth - 1) & doorY == cell.CellLength) cell.TileMap[doorY - 1, doorX].tileType = TileType.Floor;
                else if ((doorY > 0 | doorY < cell.CellLength - 1) & doorX == 0) cell.TileMap[doorY, doorX + 1].tileType = TileType.Floor;
                else if ((doorY > 0 | doorY < cell.CellLength - 1) & doorX == cell.CellWidth) cell.TileMap[doorY, doorX - 1].tileType = TileType.Floor;
            }
        }*/

        /*void GenerateMachine(Machine mach, Dungeon dgn)
    {
        int randomTileX, randomTileY;
        randomTileX = Random.Range(2, dgn.MapWidth - mach.MachineWidth);
        randomTileY = Random.Range(2, dgn.MapLength - mach.MachineLength);

        for (int i = 0; i < mach.MachineLength; i++)
        {
            for (int j = 0; j < mach.MachineWidth; j++)
            {
                dgn.TileMap[randomTileY+i, randomTileX+j].tileType = TileType.Machine;
            }
        }
    }*/

        /*void FloodFillDungeon(Cell cell, Tile tileToFill)
{
if (!tileToFill.isReachable && (tileToFill.tileType == TileType.Floor || tileToFill.tileType == TileType.Door)) tileToFill.isReachable = true;

if (tileToFill.isReachable)
{
    if ((tileToFill.mapLocationX - 1) > 1 &&
    !cell.TileMap[tileToFill.mapLocationY, tileToFill.mapLocationX - 1].isReachable) FloodFillDungeon(cell, cell.TileMap[tileToFill.mapLocationY, tileToFill.mapLocationX - 1]);
    if ((tileToFill.mapLocationX + 1) < cell.CellWidth - 1 &&
        !cell.TileMap[tileToFill.mapLocationY, tileToFill.mapLocationX + 1].isReachable) FloodFillDungeon(cell, cell.TileMap[tileToFill.mapLocationY, tileToFill.mapLocationX + 1]);
    if ((tileToFill.mapLocationY - 1) > 1 &&
        !cell.TileMap[tileToFill.mapLocationY - 1, tileToFill.mapLocationX].isReachable) FloodFillDungeon(cell, cell.TileMap[tileToFill.mapLocationY - 1, tileToFill.mapLocationX]);
    if ((tileToFill.mapLocationY + 1) < cell.CellLength - 1 &&
        !cell.TileMap[tileToFill.mapLocationY + 1, tileToFill.mapLocationX].isReachable) FloodFillDungeon(cell, cell.TileMap[tileToFill.mapLocationY + 1, tileToFill.mapLocationX]);
}

/*
List<Tile> FloodFill = new List<Tile>();
FloodFill.Add(startingTile);

while (FloodFill.Count > 0)
{
    Tile n = FloodFill[FloodFill.Count - 1];
    FloodFill.RemoveAt(FloodFill.Count-1);

    if(n.mapLocationX > 0 && n.mapLocationX < cell.CellWidth && n.mapLocationY > 0 && n.mapLocationY < cell.CellLength)
    {
        if(cell.TileMap[n.mapLocationY, n.mapLocationX].tileType == TileType.Floor 
            || cell.TileMap[n.mapLocationY, n.mapLocationX].tileType == TileType.Door)
        {
            cell.TileMap[n.mapLocationY, n.mapLocationX].isReachable = true;
            if ((n.mapLocationX - 1) > 0) FloodFill.Add(cell.TileMap[n.mapLocationY, n.mapLocationX - 1]);
            if((n.mapLocationX + 1) < cell.CellWidth) FloodFill.Add(cell.TileMap[n.mapLocationY, n.mapLocationX + 1]);
            if((n.mapLocationY - 1) > 0) FloodFill.Add(cell.TileMap[n.mapLocationY - 1, n.mapLocationX]);
            if((n.mapLocationY + 1) < cell.CellLength) FloodFill.Add(cell.TileMap[n.mapLocationY + 1, n.mapLocationX]);
        }
    }
}

//FloodFill.Clear();
for (int Y = 0; Y < cell.CellLength; Y++)
{
    for (int X = 0; X < cell.CellWidth; X++)
    {
        if (cell.TileMap[Y, X].isReachable == false) cell.TileMap[Y, X].tileType = TileType.Wall;
    }
}
}*/

        //.....Utility Functions.....//

        Vector3 TileCoordToScene(Cell cell, int X, int Y)
        {
            return new Vector3(cell.transform.position.x + (X * tileOffset), 0, cell.transform.position.z + (Y * tileOffset));
        }

        float CalculateDistance(int X1, int Y1, int X2, int Y2)
        {
            return Mathf.Sqrt(Mathf.Pow(X2 - X1, 2) + Mathf.Pow(Y2 - Y1, 2));
        }

        Vector3 CellCoordToScene(int X, int Y)
        {
            return new Vector3(X * cellOffset * tileOffset, 0, Y * cellOffset * tileOffset);
        }

        GameObject GetCellPrefabByName(string name)
        {
            for(int i = 0; i < cellPrefabs.Count; i++)
            {
                if (cellPrefabs[i].name == name) return cellPrefabs[i];
            }

            Debug.Log("Cell prefab doesn't exist!");
            return null;
        }

        List<Vector2> GenerateValidMachineTilesList(Overworld world)
        {
            List<Vector2> checkTiles = new List<Vector2>();
            bool cellPass = true;
            Debug.Log("Generating new valid machine tiles list!");

            for (int Y = 0; Y < world.CellMap.GetLength(0)-1; Y++)
            {
                for (int X = 0; X < world.CellMap.GetLength(1)-1; X++)
                {
                    cellPass = true;

                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {

                            if ( Y + i > world.CellMap.GetLength(0) - 1|| X + j > world.CellMap.GetLength(0) - 1) cellPass = false;     // Y + i < -1 || X + j < 0 ||
                            else if (Y + i >= 0 && X + j >= 0 && Y + i <= world.CellMap.GetLength(0) - 1 && X + j <= world.CellMap.GetLength(0) - 1)
                                if (world.CellMap[Y + i, X + j].GetComponent<Cell>().isMachine) cellPass = false;
                            //if ((Y + i > world.CellMap.GetLength(0)-1) || (X + j > world.CellMap.GetLength(1) - 1)) cellPass = false;
                        }
                    }

                    if(cellPass)
                    checkTiles.Add(new Vector2(X, Y));
                }
            }
            if (checkTiles.Count < 1)
            {
                Debug.Log("No valid machine tiles!");
            }

            return checkTiles;
        }

        IEnumerator WaitForSeconds(float time)
        {
            yield return new WaitForSecondsRealtime(time);
        }

        int countFloorNeighbors(Cell cell, int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int neigh_x = x + i;
                    int neigh_y = y + j;

                    if (i == 0 && j == 0) { }
                    else if (neigh_x > 0 && neigh_y > 0 && neigh_x < cell.CellWidth - 1 && neigh_y < cell.CellLength - 1)
                    {
                        if (cell.TileMap[neigh_y, neigh_x].tileType == TileType.Floor)
                        {
                            count++;
                        }
                    }

                }
            }

            return count;
        }

        Cell EmptyMap(Cell cell)
        {
            for (int X = 0, Y = 0; Y < cell.CellLength; Y++)
            {
                for (X = 0; X < cell.CellWidth; X++)
                {
                    cell.TileMap[Y, X].tileType = 0;
                }
            }

            return cell;
        }

        TileType RandomTile(float percent)
        {
            int randomRange = Random.Range(1, 101);
            if (randomRange <= percent)
            {
                return TileType.Wall;
            }
            return TileType.Floor;
        }

        public void PrintMap()
        {
            Debug.Log(WorldToString(thisOverworld));
            foreach (Cell cell in thisOverworld.CellMap) Debug.Log(CellToString(cell.GetComponent<Cell>()));
        }

        string CellToString(Cell cell)
        {
            string returnString = string.Join(" ", new string[] { "Width:",
                                          cell.CellWidth.ToString(),
                                          "\tHeight:",
                                          cell.CellLength.ToString(),
                                          "\t% Walls:",
                                          cell.PercentWalls.ToString(),
                                          "\t% Cell X/Y:",
                                          cell.OverworldLocationX.ToString() +
                                          " " + cell.OverworldLocationY.ToString(),
                                          System.Environment.NewLine });

            List<string> mapSymbols = new List<string>();
            mapSymbols.Add("O");
            mapSymbols.Add("#");
            mapSymbols.Add("%");
            mapSymbols.Add("c");
            mapSymbols.Add("D");
            mapSymbols.Add("^");

            for (int Y = 0; Y < cell.CellLength; Y++)
            {
                for (int X = 0; X < cell.CellWidth; X++)
                {
                    if(!cell.isMachine)
                    returnString += mapSymbols[(int)cell.TileMap[Y, X].tileType];
                }
                returnString += System.Environment.NewLine;
            }
            return returnString;
        }

        string WorldToString(Overworld world)
        {
            string returnString = string.Join(" ", new string[] { "Width:",
                                          world.CellMap.GetLength(1).ToString(),
                                          "\tHeight:",
                                          world.CellMap.GetLength(0).ToString(),
                                          "\t% Walls:",
                                          System.Environment.NewLine });

            List<string> mapSymbols = new List<string>();
            mapSymbols.Add("#");    //Forest
            mapSymbols.Add("O");    //Beach
            mapSymbols.Add("@");    //Cave
            mapSymbols.Add("%");    //Swamp
            mapSymbols.Add("M");    //Mountain

            for (int Y = 0; Y < world.CellMap.GetLength(0); Y++)
            {
                for (int X = 0; X < world.CellMap.GetLength(1); X++)
                {
                    if (!world.CellMap[Y, X].GetComponent<Cell>().isMachine) returnString += mapSymbols[(int)world.CellMap[Y, X].GetComponent<Cell>().thisCellType];
                    else returnString += "$";   //Machine
                }
                returnString += System.Environment.NewLine;
            }
            return returnString;
        }

    }
}

