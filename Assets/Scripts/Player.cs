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
    private Rigidbody2D rb2D;
    private float buttonTimer;
    [SerializeField] private float maxTimer = 2;
    [SerializeField] private float movementSpeedModifier = 3;
    #endregion

    #region Unity's functions
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
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
            if(buttonTimer > maxTimer)
            {
                buttonTimer = maxTimer;
            }
            Debug.Log(buttonTimer);
            rb2D.AddForce(transform.right * buttonTimer * movementSpeedModifier, ForceMode2D.Impulse);

            buttonTimer = 0f;
        }

    }
    #endregion

    #region Functions

    #endregion
}
