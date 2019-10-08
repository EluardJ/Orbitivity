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
    private string mode = "orbital";
    private Vector2 moveDirection;
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
        moveDirection = rb2D.velocity;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        if (Input.GetKey(KeyCode.Space))
        {
            buttonTimer += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (mode == "orbital")
            {
                mode = "moving";
                rb2D.AddForce(transform.right * Mathf.Clamp(buttonTimer, 0, maxTimer) * movementSpeedModifier, ForceMode2D.Impulse);
                Debug.Log(Mathf.Clamp(buttonTimer, 0, maxTimer));
            }

            buttonTimer = 0f;
        }

        if (mode == "orbital")
        {
            transform.RotateAround(orbitOrigin, orbitAxis, Time.deltaTime * movementSpeedModifier * 10);
        }

    }
    #endregion

    #region Functions

    #endregion
}
