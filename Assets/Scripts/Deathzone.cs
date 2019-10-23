#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathzone : MonoBehaviour
{
    #region Variables
    public GameObject player;
    public GameObject gameManager;
    #endregion

    #region Unity's Functions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            gameManager.GetComponent<GManager>().GameOver();
        }
    }
    #endregion

    #region Functions
    #endregion
}