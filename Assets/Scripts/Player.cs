#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    #region Variables
    [SerializeField] private GameObject gameManager;
    [SerializeField] private float minTimer = 1;
    [SerializeField] private float maxTimer = 2;
    [SerializeField] private float movementSpeedModifier = 3;
    public Slider boostSlider;

    private Rigidbody2D rb2D;
    public string mode = "orbital";
    private bool starting = true;
    private Vector2 moveDirection;
    private GameObject nearPlanet;
    private GameObject lastPlanet;
    private float lastDistanceToPlanet;
    private float distanceToOrbitingPlanet;
    private Vector3 orbitOrigin;
    private float buttonTimer;
    private float currentSpeed = 7;
    #endregion

    #region Unity's functions
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        nearPlanet = GameObject.Find("StartPlanet");
        orbitOrigin = nearPlanet.transform.position;
        distanceToOrbitingPlanet = Vector2.Distance(orbitOrigin, transform.position);
        buttonTimer = minTimer;
        gameManager.GetComponent<GManager>().MaintainNumberOfPlanets(lastPlanet, gameManager.GetComponent<GManager>().numberOfPlanets);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rb2D.velocity.magnitude);

        UsingSpaceKey();

        InGravityField();
    }

    private void FixedUpdate()
    {
        if (mode == "orbital")
        {
            OrbitAround();
        }

        RotateToMovingDirection();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (starting == true)
        {
            starting = false;
        }
        else
        {
            mode = "in gravity field";
            lastPlanet = nearPlanet;
            nearPlanet = other.gameObject;
            orbitOrigin = nearPlanet.transform.position;
            lastDistanceToPlanet = Vector2.Distance(transform.position, orbitOrigin);

            //decide the direction of the orbiting rotation around the planet, based on the position of the ship relative to the planet
            if (Vector2.SignedAngle(rb2D.velocity, (transform.position - orbitOrigin)) > 0)
            {
                movementSpeedModifier = -Mathf.Abs(movementSpeedModifier);
            }
            else
            {
                movementSpeedModifier = Mathf.Abs(movementSpeedModifier);
            }

            //manages the number of planets by destroying the last one and spawning other ones if necessary
            gameManager.GetComponent<GManager>().MaintainNumberOfPlanets(lastPlanet, gameManager.GetComponent<GManager>().numberOfPlanets);

            //manges the score
            gameManager.GetComponent<GManager>().IncrementScore();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        AdjustGravityFieldScale();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<PointEffector2D>().enabled = true;
        other.transform.GetChild(1).transform.localScale = Vector3.one;
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
    // makes the player orbit around a Planet
    {
        Vector3 dir = (orbitOrigin - transform.position);
        Vector3 cross = Vector3.Cross(dir, Vector3.forward).normalized;
        rb2D.velocity = cross * movementSpeedModifier;

        //to correct the imprecision of the orbiting code
        float diff = Vector2.Distance(orbitOrigin, transform.position) - distanceToOrbitingPlanet;
        transform.position += dir.normalized * diff;

        //Debug.Log("distance : " + Vector2.Distance(orbitOrigin, transform.position));
    }

    private void UsingSpaceKey()
    // dictate what the space key is doing according to the situation
    {
        if (Input.GetKey(KeyCode.Space) && mode == "orbital")
        {
            //charges the timer
            buttonTimer += Time.deltaTime;

            //updates the BoostSlider
            boostSlider.value = (Mathf.Clamp(buttonTimer - minTimer, 0, maxTimer) / maxTimer) * 100;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (mode == "orbital")
            {
                //launches the ship, going into moving mode
                mode = "moving";
                boostSlider.value = 0;
                rb2D.AddForce(transform.right * Mathf.Clamp(buttonTimer, minTimer, maxTimer) * Mathf.Abs(movementSpeedModifier), ForceMode2D.Impulse);
                Debug.Log(Mathf.Clamp(buttonTimer, minTimer, maxTimer));
            }

            buttonTimer = minTimer;
        }
    }

    private void InGravityField()
    {
        if (mode == "in gravity field")
        // determine if the ship is going near or away the current planet
        {
            float distanceToPlanet = Vector2.Distance(transform.position, orbitOrigin);

            if (distanceToPlanet > lastDistanceToPlanet)
            {
                //enter orbital mode if the conditions are met
                currentSpeed = rb2D.velocity.magnitude;
                rb2D.velocity = Vector2.zero;
                nearPlanet.GetComponent<PointEffector2D>().enabled = false;
                distanceToOrbitingPlanet = Vector2.Distance(orbitOrigin, transform.position);

                mode = "orbital";
            }

            lastDistanceToPlanet = distanceToPlanet;

            AdjustGravityFieldScale();
        }
    }

    private void AdjustGravityFieldScale()
    //change the radius of the sprite of the gravity field
    {
        float distRatio = Mathf.Clamp(Vector2.Distance(transform.position, orbitOrigin) / nearPlanet.GetComponent<CircleCollider2D>().radius, 0, 1);
        Transform gravFieldNearPlanet = nearPlanet.transform.GetChild(1);
        gravFieldNearPlanet.transform.localScale = new Vector3(distRatio, distRatio);
    }

    #endregion
}
