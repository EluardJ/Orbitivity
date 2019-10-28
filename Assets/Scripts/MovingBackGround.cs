#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackGround : MonoBehaviour
{
    #region Variables
    [SerializeField] private float scrollingSpeed = 5.0f;
    #endregion

    #region Unity's Functions
    void Update()
    {
        transform.position = new Vector2(transform.position.x - scrollingSpeed * Time.deltaTime, 0);

        if (transform.position.x < -37)
        {
            transform.position = new Vector2(38, 0);
        }
    }
    #endregion
}
