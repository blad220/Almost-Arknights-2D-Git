using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

//[CustomEditor(typeof(LevelCreateData))]
public class LevelInspector : Editor
{
    SerializedProperty list;
    //int Y;
    //int X;
    List<List<float>> massC = new List<List<float>>();
    //float[][] tempMass;
    private bool removeX;
    private bool removeY;

    private GUIStyle styleLabel;
    private GUIStyle styleButton;
    void OnEnable()
    {
        //    //massC = (target as LevelData).mapData3;
        //    //GetTarget = new SerializedObject(target as LevelData);
        list = serializedObject.FindProperty("mapData");
        //    //costCust = serializedObject.FindProperty("cost");
    }
    //void GUIInicialaize()
    //{
    //    styleLabel = GUI.skin.GetStyle("Label");
    //    styleLabel.alignment = TextAnchor.MiddleCenter;

    //    styleButton = GUI.skin.GetStyle("Button");
    //    //(target as LevelData).customGuiStyle = styleButton;
    //    styleButton.margin.left = 10;
    //    styleButton.margin.right = 10;
    //    styleButton.alignment = TextAnchor.MiddleCenter;
    //}
    Vector2 scrollPos;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        //GUIInicialaize();

        //EditorGUILayout.PropertyField(costCust);
        EditorGUILayout.PropertyField(list, true);

        GUILayout.Space(10);
        GUILayout.Label("Map Data");
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        //(target as LevelCreateData).MapDataAdd(new List<float>() { 0 });
        (target as LevelCreateData).MapDataZero();
        GUILayout.Space(5);
        GUILayout.Label("Map Data LIST");
        GUILayout.Space(5);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.BeginHorizontal();

    }
}

