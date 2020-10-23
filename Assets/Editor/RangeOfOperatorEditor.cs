using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MainController.Operator.Range;
using static MainController.Operator.Range.RangeOfOperator;

namespace Operator.Range
{
    [CustomEditor(typeof(RangeOfOperator))]
    public class RangeOfOperatorEditor : Editor
    {
        public RangeOfOperator rangeOfOperator;

        public List<Wrapper> RangeData = new List<Wrapper>();

        private Vector2 scrollPos;
        private bool removeX;
        private bool removeY;

        private GUIStyle styleLabel;
        private GUIStyle styleButton;
        private int newX = 0;
        private int newY = 0;
        private bool showPosition = false;
        
        void OnEnable()
        {
            rangeOfOperator = (target as RangeOfOperator);
            RangeData = rangeOfOperator.RangeData;

            EditorUtility.SetDirty(serializedObject.FindProperty("RangeData").serializedObject.targetObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            GUIInicialaize();

            if (rangeOfOperator.getRangeDataCount() <= 0) rangeOfOperator.RangeDataZero();

            rangeOfOperator.OperatorPositionX = Mathf.Clamp(rangeOfOperator.OperatorPositionX, 0, rangeOfOperator.getRangeDataCount() - 1);
            rangeOfOperator.OperatorPositionY = Mathf.Clamp(rangeOfOperator.OperatorPositionY, 0, rangeOfOperator.getRangeDataCount(0) - 1);

            GUILayout.Label("Range Data");
            GUILayout.Space(5);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginHorizontal();

            for (int x = -1; x < rangeOfOperator.getRangeDataCount(); x++)
            {

                EditorGUILayout.BeginVertical();

                for (int y = -1; y < rangeOfOperator.getRangeDataCount(0); y++)
                {
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        if (isTableFieldNames(x, y)) continue;
                        DrowTogle(x, y, 25, 20);
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
                    List<bool> tempList = new List<bool>();
                    for (int x = 0; x < rangeOfOperator.getRangeDataCount(0); x++)
                    {
                        tempList.Add(false);
                    }
                    rangeOfOperator.RangeDataAdd(tempList);
                }
                if (GUILayout.Button("Add Y", styleButton))
                {
                    for (int x = 0; x < rangeOfOperator.getRangeDataCount(); x++)
                    {
                        rangeOfOperator.RangeDataAdd(x, false);
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
                        rangeOfOperator.RangeDataRemoveLastX();
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
                        rangeOfOperator.RangeDataRemoveLastY();
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
                    rangeOfOperator.RangeDataClear();
                }
                GUILayout.Space(5);

            }

            showPosition = EditorGUILayout.Foldout(showPosition, "Transform ARRAY");
            if (showPosition)
            {
                GUILayout.Space(5);
                GUILayout.Label("Range Data Main ARRAY");
                GUILayout.Space(5);

                RangeOfOperator.Range indexOfOperatorMainRight = rangeOfOperator.ArrayTransformRightMain();
                DrawArray(indexOfOperatorMainRight);

                GUILayout.Space(5);
                GUILayout.Label("Range Data -90 ARRAY");
                GUILayout.Space(5);

                RangeOfOperator.Range indexOfOperatorTop = rangeOfOperator.ArrayTransformTop();
                DrawArray(indexOfOperatorTop);

                GUILayout.Space(5);
                GUILayout.Label("Range Data 90 ARRAY");
                GUILayout.Space(5);

                RangeOfOperator.Range indexOfOperatorBottom = rangeOfOperator.ArrayTransformBottom();
                DrawArray(indexOfOperatorBottom);

                GUILayout.Space(5);
                GUILayout.Label("Range Data 180");
                GUILayout.Space(5);

                RangeOfOperator.Range indexOfOperatorLeft = rangeOfOperator.ArrayTransformLeft();
                DrawArray(indexOfOperatorLeft);
            }

            serializedObject.ApplyModifiedProperties();
        }
        void GUIInicialaize() {

            styleLabel = GUI.skin.GetStyle("Label");
            styleLabel.alignment = TextAnchor.MiddleCenter;

            styleButton = GUI.skin.GetStyle("Button");

            styleButton.margin.left = 10;
            styleButton.margin.right = 10;
            styleButton.alignment = TextAnchor.MiddleCenter;

        }
        public void DrawArray(bool[,] array, float MaxWidth, float MaxHeight)
        {

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginHorizontal();

            for (int x = -1; x < array.GetLength(0); x++)
            {

                EditorGUILayout.BeginVertical();

                for (int y = -1; y < array.GetLength(1); y++)
                {
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        if (isTableFieldNames(x, y)) continue;
                        array[x, y] = EditorGUILayout.Toggle(array[x, y], GUILayout.MaxWidth(MaxWidth), GUILayout.MaxHeight(MaxHeight));
                    }
                }

                EditorGUILayout.EndVertical();

            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            GUILayout.Space(5);
        }
        public void DrawArray(RangeOfOperator.Range indexOfOperator)
        {
            GUILayout.Space(5);
            bool[,] arrayTransformTop = indexOfOperator.array;

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginHorizontal();

            for (int x = -1; x < arrayTransformTop.GetLength(0); x++)
            {

                EditorGUILayout.BeginVertical();

                for (int y = -1; y < arrayTransformTop.GetLength(1); y++)
                {
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        if (isTableFieldNames(x, y)) continue;
                        DrowTogle(arrayTransformTop, x, y, indexOfOperator, 25, 20);
                    }
                }

                EditorGUILayout.EndVertical();

            }
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            GUILayout.Space(5);
        }
        private void DrowTogle(int x, int y, float MaxWidth, float MaxHeight)
        {
            if (rangeOfOperator.OperatorPositionX == x && rangeOfOperator.OperatorPositionY == y)
            {
                EditorGUILayout.SelectableLabel("X", GUILayout.MaxWidth(MaxWidth), GUILayout.MaxHeight(MaxHeight));
            }
            else
            {
                rangeOfOperator.setRangeDataElementbyIndex(x, y, EditorGUILayout.Toggle(rangeOfOperator.getRangeDataElementbyIndex(x, y), GUILayout.MaxWidth(MaxWidth), GUILayout.MaxHeight(MaxHeight)));
            }
        }
        private bool DrowTogleOperator(int x, int y, float MaxWidth, float MaxHeight)
        {
            if (rangeOfOperator.OperatorPositionX == x && rangeOfOperator.OperatorPositionY == y)
            {
                EditorGUILayout.SelectableLabel("X", GUILayout.MaxWidth(MaxWidth), GUILayout.MaxHeight(MaxHeight));
                return true;
            }
            else
            {
                rangeOfOperator.setRangeDataElementbyIndex(x, y, EditorGUILayout.Toggle(rangeOfOperator.getRangeDataElementbyIndex(x, y), GUILayout.MaxWidth(MaxWidth), GUILayout.MaxHeight(MaxHeight)));
                return false;
            }
        }
        private void DrowTogle(bool[,] array, int x, int y, RangeOfOperator.Range index, float MaxWidth, float MaxHeight)
        {
            if (x == index.x && y == index.y)
            {
                EditorGUILayout.SelectableLabel("X", GUILayout.MaxWidth(MaxWidth), GUILayout.MaxHeight(MaxHeight));
                
            }
            else
            {
                array[x,y] = EditorGUILayout.Toggle(array[x,y], GUILayout.MaxWidth(MaxWidth), GUILayout.MaxHeight(MaxHeight));
                
            }
        }
        private bool isTableFieldNames(int x, int y)
        {
            if (x == -1 && y == -1)
            {
                GUILayout.Label("", GUILayout.MaxWidth(25), GUILayout.MaxHeight(20));
                return true;
            };
            if (x == -1)
            {
                GUILayout.Label($"Y:{y}", GUILayout.MaxWidth(25), GUILayout.MaxHeight(20));
                return true;
            }
            if (y == -1)
            {
                GUILayout.Label($"X:{x}", GUILayout.MaxWidth(25), GUILayout.MaxHeight(20));
                return true;
            };
            return false;
        }
        
    }
}