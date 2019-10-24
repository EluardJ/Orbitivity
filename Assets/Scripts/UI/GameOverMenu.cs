#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    #region Variables
    private bool spawning = false;
    private float lerpScale = 0.0f;
    #endregion

    #region Unity's Functions
    void Start()
    {
        GameEvents.current.onGameOver += GameOver;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        AnimateAllChildrenSpawning();
    }

    private void OnDestroy()
    {
        GameEvents.current.onGameOver -= GameOver;
    }
    #endregion

    #region Functions
    private void GameOver()
    {
        gameObject.SetActive(true);
        spawning = true;
    }

    private void AnimateAllChildrenSpawning()
    {
        if (spawning == true && lerpScale <= 1.0f)
        {
            float lerpedNumber = Mathf.Lerp(0.0f, 1.0f, lerpScale);

            foreach (Transform child in transform)
            {
                child.transform.localScale = new Vector3(lerpedNumber, lerpedNumber, 1);
            }

            lerpScale += 3 * Time.deltaTime;
        }
    }

    #endregion
}
