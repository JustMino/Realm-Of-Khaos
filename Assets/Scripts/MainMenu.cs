using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void Level1()
  {
    SceneManager.LoadScene("Level1", LoadSceneMode.Single);
  }
  public void Settings()
  {
    SceneManager.LoadScene(2, LoadSceneMode.Single);
  }
  public void Credits()
  {
    SceneManager.LoadScene(5, LoadSceneMode.Single);
  }
  public void doExitGame()
  {
    Application.Quit();
  }
}
