using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPause : MonoBehaviour
{
  public GameObject canvas;
  public PauseMenu pause;
  public AudioSource AS;
  public bool paused2 = false;
    // Start is called before the first frame update
    void Start()
    {
      AS = GetComponent<AudioSource>();
      canvas = GameObject.Find("PauseMenu Canvas");
      pause = canvas.GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
      bool paused = pause.GameIsPaused;
      if (AS.isPlaying && paused && !paused2)
      {
        AS.Pause();
        paused2 = true;
      }
      else if (!paused && paused2)
      {
        AS.Play();
        paused2 = false;
      }
    }
}
