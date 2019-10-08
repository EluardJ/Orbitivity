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
    private bool orbitalMode = true;
    private GameObject orbitalPlanet;
    private Vector3 orbitOrigin;
    private Vector3 orbitAxis;
    private float buttonTimer;
    [SerializeField] private float maxTimer = 2;
    [SerializeField] private float movementSpeedModifier = 3;
    #endregion

    #region Unity's functions
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        orbitalPlanet = GameObject.Find("OrbitPlanet");
        orbitOrigin = orbitalPlanet.transform.position;
        orbitAxis = new Vector3(0, 0, 1);
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
            if(orbitalMode == true)
            {
                orbitalMode = false;
            }

            if (buttonTimer > maxTimer)
            {
                buttonTimer = maxTimer;
            }
            Debug.Log(buttonTimer);
            rb2D.AddForce(transform.right * buttonTimer * movementSpeedModifier, ForceMode2D.Impulse);

            buttonTimer = 0f;
        }

        if (orbitalMode == true)
        {
            transform.RotateAround(orbitOrigin, orbitAxis, Time.deltaTime * movementSpeedModifier * 10);
        }

        Debug.Log(rb2D.velocity);
    }
    #endregion

    #region Functions

    #endregion
}
