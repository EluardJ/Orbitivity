#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    #region Variables
    private float rotationSpeed = 0;
    private Vector3 rotationVector;
    private GameObject GravityField;
    #endregion

    #region Unity's Functions
    // Start is called before the first frame update
    void Start()
    {
        GravityField = transform.GetChild(1).gameObject;

        while(rotationSpeed == 0)
        {
            rotationSpeed = Random.Range(-50, 50);
        }
        rotationVector = new Vector3(0, 0, rotationSpeed/100);

        GravityField.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        GravityField.transform.Rotate(rotationVector);
    }
    #endregion

    #region Functions
    #endregion
}