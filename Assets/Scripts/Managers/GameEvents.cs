#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    #region Unity's Functions
    private void Awake()
    {
        current = this;
    }
    #endregion

    #region Events
    public event Action<int> onScoreIncrease;
    public void ScoreIncrease(int number)
    {
        if(onScoreIncrease != null)
        {
            onScoreIncrease(number);
        }
    }

    public event Action<GameObject> onNewPlanet;
    public void NewPlanet(GameObject lastPlanet)
    {
        if(onNewPlanet != null)
        {
            onNewPlanet(lastPlanet);
        }
    }

    public event Action onGameOver;
    public void GameOver()
    {
        if(onGameOver != null)
        {
            onGameOver();
        }
    }

    public event Action<float> onSliderChange;
    public void SliderValueChange(float number)
    {
        if(onSliderChange != null)
        {
            onSliderChange(number);
        }
    }

    public event Action<Color> onSliderImageColorChange;
    public void SliderImageColorChange(Color color)
    {
        if(onSliderImageColorChange != null)
        {
            onSliderImageColorChange(color);
        }
    }

    public event Action<Transform, int, float> onInstantiateScorePopup;
    public void InstantiateScorePopup(Transform position, int score, float size)
    {
        if(onInstantiateScorePopup != null)
        {
            onInstantiateScorePopup(position, score, size);
        }
    }
    #endregion
}
