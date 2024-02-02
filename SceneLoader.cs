using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour {
    [DllImport("__Internal")]
    private static extern void Refresh();

    public void LoadSimulation() {
        SceneManager.LoadScene("Simulation");
    }

    public void LoadMenu() {
        Application.Quit();
        Refresh();
    }

}