using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void RestartScene() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Exit() {
        Application.Quit();
    }
}
