using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void MainScenePlay()
    {
        SceneManager.LoadScene("Game Scene");
    }
    
    public void MainMenuLoad()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

    //[MenuItem("SonorantStudios/Scenes/Main Menu")]
    //static void LoadMainMenuScene()
    //{
    //    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
    //    {
    //        EditorSceneManager.OpenScene($"Assets/Scenes/Main Menu.unity", OpenSceneMode.Single);
    //    }
    //}

    //[MenuItem("SonorantStudios/Scenes/Game Scene")]
    //static void LoadBuildScene()
    //{
    //    if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
    //    {
    //        EditorSceneManager.OpenScene($"Assets/Scenes/Game Scene.unity", OpenSceneMode.Single);
    //    }
    //}




}
