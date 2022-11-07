using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    GameManager gm;
    public GameObject gameM;

    public Slider mastervolume;
    public Slider gameeffectsvolume;
    public Slider musicvolume;

    public float Mvol;
    public float gevol;
    public float mvol;

    public float maxgevol;
    public float maxmvol;
    public bool setgemax;
    public bool setmmax;

    private void Start()
    {
        gameM = GameObject.Find("GameManager");
        gm = gameM.GetComponent<GameManager>();
        mastervolume = GameObject.Find("Master Volume").GetComponent<Slider>();
        gameeffectsvolume = GameObject.Find("Game Effects Volume").GetComponent<Slider>();
        musicvolume = GameObject.Find("Music Volume").GetComponent<Slider>();
        mastervolume.value = gm.mastervolume;
        gameeffectsvolume.value = gm.gameeffectsvolume;
        musicvolume.value = gm.musicvolume;
        SetMasterVolume(gm.mastervolume);
        SetGameEffectsVolume(gm.gameeffectsvolume);
        SetMusicVolume(gm.musicvolume);
    }

    void Update()
    {
      // UnityEngine.UI.Dropdown.DestroyObject(GameObject.Find("Blocker"));
      audioMixer.GetFloat("Master Volume", out Mvol);
      audioMixer.GetFloat("Game Effects Volume", out gevol);
      audioMixer.GetFloat("Music Volume", out mvol);
      SetMaxGEVolume(gevol);
      SetMaxMVolume(mvol);
      if (gevol > Mvol)
      {
        audioMixer.SetFloat("Game Effects Volume", Mvol);
        gameeffectsvolume.value = Mvol;
      }
      else if (gevol >= maxgevol && gevol == Mvol)
      {
        audioMixer.SetFloat("Game Effects Volume", Mvol);
        setgemax = false;
        gameeffectsvolume.value = Mvol;
      }
      if (mvol > Mvol)
      {
        audioMixer.SetFloat("Music Volume", Mvol);
        musicvolume.value = Mvol;
      }
      else if (mvol >= maxmvol && mvol == Mvol)
      {
        audioMixer.SetFloat("Music Volume", Mvol);
        musicvolume.value = Mvol;
      }
    }

    public void SetMaxGEVolume(float gevol)
    {
      if (!setgemax)
      {
        maxgevol = gevol;
        setgemax = true;
      }
    }
    public void SetMaxMVolume(float mvol)
    {
      if(!setmmax)
      {
        maxmvol = mvol;
        setmmax = true;
      }
    }
    public void SetMasterVolume(float volume)
    {
      if (volume == -30)
      {
        audioMixer.SetFloat("Master Volume", -80f);
      }
      else
      {
        audioMixer.SetFloat("Master Volume", volume);
        gm.mastervolume = volume;
      }
    }

    public void SetGameEffectsVolume(float volume)
    {
      if (volume == -30)
      {
        audioMixer.SetFloat("Game Effects Volume", -80f);
      }
      else
      {
        audioMixer.SetFloat("Game Effects Volume", volume);
        gm.gameeffectsvolume = volume;
      }
    }

    public void SetMusicVolume(float volume)
    {
      if (volume == -30)
      {
        audioMixer.SetFloat("Music Volume", -80f);
      }
      else
      {
        audioMixer.SetFloat("Music Volume", volume);
        gm.musicvolume = volume;
      }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        gm.fullscreen = isFullscreen;
    }

    public void GoBack()
    {
      SceneManager.UnloadSceneAsync(2);
    }

    public void GoControls()
    {
      SceneManager.LoadScene(6, LoadSceneMode.Additive);
    }
}
