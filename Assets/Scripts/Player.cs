#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region Variables
    private float movementSpeed;
    private float buttonTimer;
    [SerializeField] private float maxTimer = 2;
    #endregion

    #region Unity's functions
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            buttonTimer += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(buttonTimer > 2f)
            {
                buttonTimer = 2f;
            }
            movementSpeed = buttonTimer;
            buttonTimer = 0f;
        }

        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }
    #endregion

    #region Functions

    #endregion
}
