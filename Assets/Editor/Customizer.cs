using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Customizer : EditorWindow
{

    Color color;

    [MenuItem("Maz's Tools/Colorizer")]
    public static void ShowWindow()
    {
        /*This allows the menu yo show when it's called. It will be stored where I address it in the MenuItem.
         * 
         */
        GetWindow<Customizer>("Colorizer");
    }

    void OnGUI()
    {
        GUILayout.Label("Test label", EditorStyles.boldLabel);

        color = EditorGUILayout.ColorField("Color", color);

        if (GUILayout.Button("Apply"))
        {
            Colorize();
            Debug.Log("Pressed");
        }
    }

    void Colorize()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer render = obj.GetComponent<Renderer>();
            if (render != null)
            {
                render.sharedMaterial.color = color;
            }
        }
    }
}
