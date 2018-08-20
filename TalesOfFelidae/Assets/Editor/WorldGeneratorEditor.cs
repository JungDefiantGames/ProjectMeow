using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DungeonGenerator
{
    [CustomEditor(typeof(OverworldManager))]
    public class WorldGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            OverworldManager manager = (OverworldManager)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Generate New Overworld"))
            {
                manager.GenerateOverworld();
            }

            if (manager.thisOverworld != null && GUILayout.Button("Generate Cells"))
            {
                manager.GenerateCellObjects(manager.thisOverworld);
            }

            if (manager.thisOverworld != null && GUILayout.Button("Generate Lakes & Clean Up"))
            {
                manager.CleanUpOverworld(manager.thisOverworld);
            }

            if (manager.thisOverworld != null && GUILayout.Button("Assign Doors"))
            {
                manager.AssignDoors(manager.thisOverworld);
            }

            if (manager.thisOverworld != null && GUILayout.Button("Print Map"))
            {
                manager.PrintMap();
            }

            if (manager.thisOverworld != null && GUILayout.Button("Generate Physical Map"))
            {
                manager.GenerateTileObjects(manager.thisOverworld);
            }

            if (manager.thisOverworld != null && GUILayout.Button("Wipe Overworld"))
            {
                manager.WipeOverworld();
            }

        }
    }
}

