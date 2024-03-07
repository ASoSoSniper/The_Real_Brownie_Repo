using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementStuff : MonoBehaviour
{
    public void SceneTransitioning(int sceneID)
    {
        SceneManager.LoadScene(sceneID, LoadSceneMode.Single);
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
