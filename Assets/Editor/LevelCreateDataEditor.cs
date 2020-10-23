using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MainController.Level.Data;

namespace Level.Data
{
    [CustomEditor(typeof(LevelCreateData))]
    public class LevelCreateDataEditor : Editor
    {
        public GameObject map = null;

        private bool removeX;
        private bool removeY;

        private GUIStyle styleLabel;
        private GUIStyle styleButton;

        private LevelCreateData level;

        void OnEnable()
        {
            level = (target as LevelCreateData);
            map = level.map;

            EditorUtility.SetDirty(serializedObject.FindProperty("mapData").serializedObject.targetObject);
        }
        void GUIInicialaize()
        {
            styleLabel = GUI.skin.GetStyle("Label");
            styleLabel.alignment = TextAnchor.MiddleCenter;

            styleButton = GUI.skin.GetStyle("Button");

            styleButton.margin.left = 10;
            styleButton.margin.right = 10;
            styleButton.alignment = TextAnchor.MiddleCenter;
        }
        Vector2 scrollPos;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();

            GUIInicialaize();

            GUILayout.Space(10);

            if (level.getMapDataCount() <= 0) level.MapDataZero();

            GUILayout.Label("Map Data");
            GUILayout.Space(5);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginHorizontal();

            for (int x = -1; x < level.getMapDataCount(); x++)
            {

                EditorGUILayout.BeginVertical();

                for (int y = -1; y < level.getMapDataCount(0); y++)
                {
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        if (x == -1 && y == -1)
                        {
                            GUILayout.Label("", GUILayout.MaxWidth(50));
                            continue;
                        };
                        if (x == -1)
                        {
                            GUILayout.Label($"Y:{y}", GUILayout.MaxWidth(50));
                            continue;
                        }
                        if (y == -1)
                        {
                            GUILayout.Label($"X:{x}", GUILayout.MaxWidth(50));
                            continue;
                        };

                        level.setMapDataElementbyIndex(x, y, EditorGUILayout.FloatField(level.getMapDataElementbyIndex(x, y), GUILayout.MaxWidth(50)));
                    }
                }
                EditorGUILayout.EndVertical();

            }
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            GUILayout.Space(5);

            GUILayout.Space(5);

            GUIStyle customHorizontal = new GUIStyle();
            customHorizontal.padding.left = 10;
            customHorizontal.padding.right = 10;

            GUILayout.BeginHorizontal(customHorizontal);

            GUILayout.EndHorizontal();
            GUILayout.Space(5);


            using (var verticalScope = new GUILayout.VerticalScope("box"))
            {

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Add X", styleButton))
                {
                    List<float> tempList = new List<float>();
                    for (int x = 0; x < level.getMapDataCount(0); x++)
                    {
                        tempList.Add(0);
                    }
                    level.MapDataAdd(tempList);
                }
                if (GUILayout.Button("Add Y", styleButton))
                {
                    for (int x = 0; x < level.getMapDataCount(); x++)
                    {
                        level.MapDataAdd(x, 0);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                if (!removeX ^ removeY)
                {
                    if (GUILayout.Button("Remove last X"))
                    {
                        removeX = true;
                    }
                }
                if (removeX)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Delete THIS?", styleLabel, GUILayout.MaxWidth(100));
                    if (GUILayout.Button("Yes"))
                    {
                        level.MapDataRemoveLastX();
                        removeX = false;
                    }
                    if (GUILayout.Button("No"))
                    {
                        removeX = false;
                    }
                    GUILayout.EndHorizontal();
                }
                if (!removeY ^ removeX)
                {
                    if (GUILayout.Button("Remove last Y"))
                    {
                        removeY = true;
                    }
                }
                if (removeY)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Delete THIS?", styleLabel, GUILayout.MaxWidth(100));
                    if (GUILayout.Button("Yes"))
                    {
                        level.MapDataRemoveLastY();
                        removeY = false;
                    }
                    if (GUILayout.Button("No"))
                    {
                        removeY = false;
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

                if (GUILayout.Button("Clear"))
                {
                    level.MapDataClear();
                }
                GUILayout.Space(5);

            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}