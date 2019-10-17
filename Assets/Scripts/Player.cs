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
    [SerializeField] private float minTimer = 0f;
    [SerializeField] private float maxTimer = 2f;
    [SerializeField] private float movementSpeedModifier = 7;
    public Slider boostSlider;
    public Image boostSliderFillImage;

    private Rigidbody2D rb2D;
    public string state = "orbital";
    private bool starting = true;
    private Vector2 moveDirection;
    private GameObject nearPlanet;
    private GameObject lastPlanet;
    private float lastDistanceToPlanet;
    private float distanceToOrbitingPlanet;
    private Vector3 orbitOrigin;
    private float buttonTimer;
    private int superSlideCounter = 0;
    private ParticleSystem particleBoost;
    private Gradient boostGradientRed;
    private Gradient boostGradientBlue;
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
        boostSliderFillImage.color = Color.green;

        //set the colors for the boost particle system
        particleBoost = transform.GetChild(1).GetComponent<ParticleSystem>();
        boostGradientRed = new Gradient();
        boostGradientRed.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(1f, 0.63f, 0f), 0.0f), new GradientColorKey(new Color(0.94f, 0.82f, 0f), 20.0f), new GradientColorKey(new Color(0.89f, 1f, 0f), 50.0f) }, new GradientAlphaKey[] { });
        boostGradientBlue = new Gradient();
        boostGradientBlue.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(0.12f, 0f, 1f), 0.0f), new GradientColorKey(new Color(0.06f, 0.44f, 0.99f), 0.2f), new GradientColorKey(new Color(0.16f, 0.96f, 1f), 0.5f) }, new GradientAlphaKey[] {new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f)});
        Debug.Log(boostGradientBlue);
    }

    // Update is called once per frame
    void Update()
    {
        UsingSpaceKey();

        InGravityField();
    }

    private void FixedUpdate()
    {
        if (state == "orbital")
        {
            OrbitAround();
        }

        RotateToMovingDirection();

        if(state == "moving")
        {
            gameManager.GetComponent<GManager>().IncrementScore(Mathf.RoundToInt(rb2D.velocity.magnitude));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Planet")
        {
            EnteringGravityField(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Planet" && superSlideCounter == 1)
        {
            var col = particleBoost.colorOverLifetime;
            col.color = boostGradientRed;
        }
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
    }

    private void UsingSpaceKey()
    // dictate what the space key is doing according to the situation
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //charges the timer
            buttonTimer += Time.deltaTime;

            //updates the BoostSlider and changes the color if needed
            boostSlider.value = (Mathf.Clamp(buttonTimer - minTimer, 0, maxTimer) / maxTimer) * 100;
            if (buttonTimer > maxTimer / 2)
            {
                if(buttonTimer > maxTimer/2)
                {
                    boostSliderFillImage.color = Color.cyan;
                    var col = particleBoost.colorOverLifetime;
                    col.color = boostGradientBlue;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (state == "orbital")
            {
                //launches the ship, going into moving state
                state = "moving";
                rb2D.AddForce(transform.right * Mathf.Clamp(buttonTimer, minTimer, maxTimer) * Mathf.Abs(movementSpeedModifier), ForceMode2D.Impulse);

                //add counters to the super slide counter if the value is high enough
                if (buttonTimer > maxTimer / 2)
                {
                    superSlideCounter = 2;
                }
            }

            boostSlider.value = 0;
            boostSliderFillImage.color = Color.green;
            buttonTimer = minTimer;
        }
    }

    private void EnteringGravityField(GameObject other)
    {
        if (starting == true)
        {
            starting = false;
        }
        else
        {
            state = "in gravity field";
            lastPlanet = nearPlanet;
            nearPlanet = other;
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

            //adjust number of super slide counters if necessary and manages the score
            if (superSlideCounter > 1)
            {
                superSlideCounter--;
                gameManager.GetComponent<GManager>().IncrementScore(1000);
            }
            else if (superSlideCounter == 1)
            {
                superSlideCounter = 0;
                gameManager.GetComponent<GManager>().IncrementScore(4000);
            }
            else
            {
                gameManager.GetComponent<GManager>().IncrementScore(500);
            }
        }
    }

    private void InGravityField()
    {
        if (state == "in gravity field")
        // determine if the ship is going near or away the current planet
        {
            float distanceToPlanet = Vector2.Distance(transform.position, orbitOrigin);

            if (distanceToPlanet > lastDistanceToPlanet && distanceToPlanet > 2 && superSlideCounter <= 0)
            {
                //enter orbital state if the conditions are met
                rb2D.velocity = Vector2.zero;
                nearPlanet.GetComponent<PointEffector2D>().enabled = false;
                distanceToOrbitingPlanet = Vector2.Distance(orbitOrigin, transform.position);

                state = "orbital";
            }

            lastDistanceToPlanet = distanceToPlanet;
        }
    }
    #endregion
}
