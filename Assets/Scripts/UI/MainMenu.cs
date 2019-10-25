#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    #region Variables
    public Text HighScoreText;
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    #endregion

    #region Unity's Functions
    private void Start()
    {
        HighScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        SetVolume(PlayerPrefs.GetFloat("Volume", 0));
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0);
    }
    #endregion

    #region Functions
    public void LaunchGame()
    {
        AudioManager.current.Playsound("Click");
        SceneManager.LoadScene("Game");
    }

    public void ResetHighScore()
    {
        AudioManager.current.Playsound("Click");
        PlayerPrefs.SetInt("HighScore", 0);
        HighScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void QuitGame()
    {
        AudioManager.current.Playsound("Click");
        Application.Quit();
    }
    #endregion
}