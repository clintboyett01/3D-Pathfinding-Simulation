using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript: MonoBehaviour {
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        Time.timeScale = 1;
        GameIsPaused = false;
        PauseMenuUI.SetActive(false);
    }

    public void Pause() {
        Time.timeScale = 0;
        GameIsPaused = true;
        PauseMenuUI.SetActive(true);
    }

}