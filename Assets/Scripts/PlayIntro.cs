using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayIntro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      GameObject camera = GameObject.Find("Main Camera");

      var videoPlayer = camera.GetComponent<UnityEngine.Video.VideoPlayer>();
      string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "splash.mov");
      videoPlayer.url = filePath;
      videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
      InvokeRepeating("checkOver", 0.1f, 0.1f);
    }

    private void checkOver()
    {
      long playerCurrentFrame = GetComponent<Camera>().GetComponent<UnityEngine.Video.VideoPlayer>().frame;
      long playerFrameCount = System.Convert.ToInt64(GetComponent<Camera>().GetComponent<UnityEngine.Video.VideoPlayer>().frameCount);

      if (playerCurrentFrame < playerFrameCount - 1)
      {
        return;
      }
      else
      {
        SceneManager.LoadScene(1);
        CancelInvoke("checkOver");
      }
    }
}
