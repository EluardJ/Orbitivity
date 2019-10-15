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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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