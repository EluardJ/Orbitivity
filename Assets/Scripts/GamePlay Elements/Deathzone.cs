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
    #endregion

    #region Unity's Functions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameEvents.current.GameOver();
        }
    }
    #endregion

    #region Functions
    #endregion
}