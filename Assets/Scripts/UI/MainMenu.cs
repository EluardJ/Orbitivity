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

public class MainMenu : MonoBehaviour
{
    #region Variables
    public Text HighScoreText;
    #endregion

    #region Unity's Functions
    private void Start()
    {
        HighScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    #endregion

    #region Functions
    public void LaunchGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        HighScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    #endregion
}