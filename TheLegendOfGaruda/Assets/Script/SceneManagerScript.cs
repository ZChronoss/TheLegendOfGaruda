using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public string nextSceneName;
    public void changeScene() {
        SceneManager.LoadScene(nextSceneName);
    }
}
