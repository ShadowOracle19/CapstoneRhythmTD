using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void MainScenePlay()
    {
        SceneManager.LoadScene("Test Build Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    [MenuItem("SonorantStudios/Scenes/Main Menu")]
    static void LoadMainMenuScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/Main Menu.unity", OpenSceneMode.Single);
        }
    }

    [MenuItem("SonorantStudios/Scenes/Test Build Scene")]
    static void LoadBuildScene()
    {
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/Test Build Scene.unity", OpenSceneMode.Single);
        }
    }

    [MenuItem("SonorantStudios/Scenes/Dialogue test")]
    static void LoadDialogueTestScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/Dialogue Test Scene.unity", OpenSceneMode.Single);
        }
    }



}
