#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    #region Variables
    private float rotationSpeed = 0;
    private Vector3 rotationVector;
    #endregion

    #region Unity's Functions
    // Start is called before the first frame update
    void Start()
    {
        while(rotationSpeed == 0)
        {
            rotationSpeed = Random.Range(-50, 50);
        }
        rotationVector = new Vector3(0, 0, rotationSpeed/100);

        gameObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationVector);
    }
    #endregion

    #region Functions
    #endregion
}