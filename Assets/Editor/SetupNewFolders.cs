using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SetupNewFolders : EditorWindow
{
    private string newDirectory;
    private string newName;

    [MenuItem("Maz's Tools/Create New Folders")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SetupNewFolders));
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Create a new folder structure", EditorStyles.largeLabel);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField($"Root Folder Name: {newDirectory}");
        newName = EditorGUILayout.TextField("", newName);
        newDirectory = $"Project_{newName}";

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("GO!"))
        {
            AssetDatabase.CreateFolder("Assets", newDirectory);
            AssetDatabase.CreateFolder($"Assets/{newDirectory}", "Prefabs");
            AssetDatabase.CreateFolder($"Assets/{newDirectory}", "Scenes");
            AssetDatabase.CreateFolder($"Assets/{newDirectory}", "Scripts");
            AssetDatabase.CreateFolder($"Assets/{newDirectory}/Scripts", "Systems");

            var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);  
            //All current open Scenes are closed and the newly created Scene are opened.
            EditorSceneManager.SaveScene(newScene, $"Assets/{newDirectory}/Scenes/{newName}-Demo.unity");
        }

        if (GUILayout.Button("Cancel"))
        {
            Close();
        }

        EditorGUILayout.EndHorizontal();
    }
}