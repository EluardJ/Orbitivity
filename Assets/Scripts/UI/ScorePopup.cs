#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    #region Variables
    private TextMeshPro text;
    #endregion

    #region Unity's Functions
    private void Start()
    {
        text = transform.GetComponent<TextMeshPro>();
    }

    void Update()
    {
        AnimatePopup();
    }
    #endregion

    #region Functions
    private void AnimatePopup()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 5, 0);
        text.alpha -= 2 * Time.deltaTime;

        if(text.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
