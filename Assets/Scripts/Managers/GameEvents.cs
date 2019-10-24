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
        onScoreIncrease?.Invoke(number);
    }

    public event Action<GameObject> onEnteringNewPlanet;
    public void EnteringNewPlanet(GameObject lastPlanet)
    {
        onEnteringNewPlanet?.Invoke(lastPlanet);
    }

    public event Action onGameOver;
    public void GameOver()
    {
        onGameOver?.Invoke();
    }

    public event Action<float> onSliderChange;
    public void SliderValueChange(float number)
    {
        onSliderChange?.Invoke(number);
    }

    public event Action<Color> onSliderImageColorChange;
    public void SliderImageColorChange(Color color)
    {
        onSliderImageColorChange?.Invoke(color);
    }

    public event Action<Transform, int, float> onInstantiateScorePopup;
    public void InstantiateScorePopup(Transform position, int score, float size)
    {
        onInstantiateScorePopup?.Invoke(position, score, size);
    }
    #endregion
}
