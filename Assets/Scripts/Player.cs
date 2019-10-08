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
    private GameObject nearPlanet;
    private float lastDistanceToPlanet;
    private Vector3 orbitOrigin;
    private Vector3 orbitAxis = new Vector3(0, 0, 1);
    private float buttonTimer;
    [SerializeField] private float maxTimer = 2;
    [SerializeField] private float movementSpeedModifier = 3;
    #endregion

    #region Unity's functions
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        nearPlanet = GameObject.Find("OrbitPlanet");
        orbitOrigin = nearPlanet.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == "orbital")
        {
            OrbitAround();
        }

        RotateToMovingDirection();

        UsingSpaceKey();

        if(mode == "in gravity field")
        {
            float distanceToPlanet = Vector2.Distance(transform.position, orbitOrigin);
            Debug.Log("distance : " + distanceToPlanet + "   last dist : " + lastDistanceToPlanet);

            if(distanceToPlanet <= lastDistanceToPlanet)
            {
                Debug.Log("approaching");
            }
            else
            {
                Debug.Log("going away");
                rb2D.velocity = Vector2.zero;
                nearPlanet.GetComponent<PointEffector2D>().enabled = false;
                mode = "orbital";
            }

            lastDistanceToPlanet = distanceToPlanet;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        mode = "in gravity field";
        nearPlanet = other.gameObject;
        orbitOrigin = nearPlanet.transform.position;
        lastDistanceToPlanet = Vector2.Distance(transform.position, orbitOrigin);
        Debug.Log(mode);
    }
    #endregion

    #region Functions

    private void RotateToMovingDirection()
    //Rotate the player in the direction it is moving to
    {
        moveDirection = rb2D.velocity;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OrbitAround()
    // makes the player orbit around an OrbitpLanet
    {
            transform.RotateAround(orbitOrigin, orbitAxis, Time.deltaTime * movementSpeedModifier * 10);
    }

    private void UsingSpaceKey()
    // dictate what the space key is doing according to the situation
    {
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
    }

    #endregion
}
