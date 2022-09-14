using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
  public bool GameIsPaused = false;
  public GameObject PauseMenuUI;

  GameManager gm;
  public GameObject gameM;

  void Start()
  {
    gameM = GameObject.Find("GameManager");
    gm = gameM.GetComponent<GameManager>();
  }
  void Update()
  {
    if(Input.GetKeyDown(gm.pausekey))
    {
      if(GameIsPaused)
      {
        Resume();
      }
      else
      {
        Pause();
      }
    }
  }

  public void Resume()
  {
    Debug.Log("Resumed");
    PauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    GameIsPaused = false;
    Cursor.visible = false;
  }

  void Pause()
  {

    PauseMenuUI.SetActive(true);
    Time.timeScale = 0f;
    GameIsPaused = true;
    Cursor.visible = true;
  }

  public void Settings()
  {
    Debug.Log("Settings");
    SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    Cursor.visible = true;
  }

  public void Menu()
  {
    SceneManager.LoadScene("Menu", LoadSceneMode.Single);
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
