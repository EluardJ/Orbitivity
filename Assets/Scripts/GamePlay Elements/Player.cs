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
    [Header("Movement Variables")]
    [SerializeField] private float minTimer = 0f;
    [SerializeField] private float maxTimer = 2f;
    [SerializeField] private float movementSpeedModifier = 7;

    private enum State
    {
        Orbiting,
        Moving,
        InGravityField,
        Dead
    }

    private Rigidbody2D rigidBody2D;
    private State currentState = State.Orbiting;
    private bool starting = true;
    private GameObject nearPlanet;
    private GameObject lastPlanet;
    private float lastDistanceToPlanet;
    private float distanceToOrbitingPlanet;
    private Vector3 orbitOrigin;
    private float boostButtonTimer;
    private int superSlideCounter = 0;
    private ParticleSystem particleBoost;
    private Gradient boostGradientRed;
    private Gradient boostGradientBlue;
    #endregion

    #region Unity's functions
    void Start()
    {
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        nearPlanet = GameObject.Find("StartPlanet");
        orbitOrigin = nearPlanet.transform.position;
        distanceToOrbitingPlanet = Vector2.Distance(orbitOrigin, transform.position);
        boostButtonTimer = minTimer;

        SetColoursForBoostParticlesGradients();

        GameEvents.current.onGameOver += AnimateDeath;
    }

    void Update()
    {
        ChargeBoost();
        LaunchShip();

        if (currentState == State.InGravityField)
        {
            InGravityField();
        }
    }

    private void FixedUpdate()
    {
        if (currentState == State.Orbiting)
        {
            OrbitAround();
        }

        RotateToMovingDirection();

        if (currentState == State.Moving || currentState == State.InGravityField)
        {
            GameEvents.current.ScoreIncrease(Mathf.RoundToInt(rigidBody2D.velocity.magnitude));
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
        if (other.gameObject.tag == "Planet" && superSlideCounter == 1)
        {
            var col = particleBoost.colorOverLifetime;
            col.color = boostGradientRed;
            currentState = State.Moving;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "DeathZone" && currentState == State.Moving)
        {
            GameEvents.current.GameOver();
            currentState = State.Dead;
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onGameOver -= AnimateDeath;
    }
    #endregion

    #region Functions

    private void RotateToMovingDirection()
    {
        Vector2 moveDirection = rigidBody2D.velocity;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OrbitAround()
    {
        Vector3 dir = (orbitOrigin - transform.position);
        Vector3 cross = Vector3.Cross(dir, Vector3.forward).normalized;
        rigidBody2D.velocity = cross * movementSpeedModifier;

        CorrectOrbitImprecision(dir);
    }

    private void CorrectOrbitImprecision(Vector3 dir)
    {
        float diff = Vector2.Distance(orbitOrigin, transform.position) - distanceToOrbitingPlanet;
        transform.position += dir.normalized * diff;
    }

    private void ChargeBoost()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            boostButtonTimer += Time.deltaTime;

            GameEvents.current.SliderValueChange((Mathf.Clamp(boostButtonTimer - minTimer, 0, maxTimer) / maxTimer) * 100);
            if (boostButtonTimer > maxTimer / 2)
            {
                if (boostButtonTimer > maxTimer / 2)
                {
                    GameEvents.current.SliderImageColorChange(Color.cyan);
                    var col = particleBoost.colorOverLifetime;
                    col.color = boostGradientBlue;
                }
            }
        }
    }

    private void LaunchShip()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (currentState == State.Orbiting)
            {
                currentState = State.Moving;
                rigidBody2D.AddForce(transform.right * Mathf.Clamp(boostButtonTimer, minTimer, maxTimer) * Mathf.Abs(movementSpeedModifier), ForceMode2D.Impulse);

                if (boostButtonTimer > maxTimer / 2)
                {
                    superSlideCounter = 2;
                }
            }

            GameEvents.current.SliderValueChange(0);
            GameEvents.current.SliderImageColorChange(Color.yellow);
            boostButtonTimer = minTimer;
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
            currentState = State.InGravityField;
            lastPlanet = nearPlanet;
            nearPlanet = other;
            orbitOrigin = nearPlanet.transform.position;
            lastDistanceToPlanet = Vector2.Distance(transform.position, orbitOrigin);

            GameEvents.current.EnteringNewPlanet(lastPlanet);

            ManageSlideCountersAndScore();
        }
    }

    private void ManageSlideCountersAndScore()
    {
        if (superSlideCounter > 1)
        {
            superSlideCounter--;
            GameEvents.current.ScoreIncrease(1000);
            GameEvents.current.InstantiateScorePopup(nearPlanet.transform, 1000, 1.5f);
        }
        else if (superSlideCounter == 1)
        {
            superSlideCounter = 0;
            GameEvents.current.ScoreIncrease(5000);
            GameEvents.current.InstantiateScorePopup(nearPlanet.transform, 5000, 2.0f);
            DecideOrbitDirection();
        }
        else
        {
            GameEvents.current.ScoreIncrease(1000);
            GameEvents.current.InstantiateScorePopup(nearPlanet.transform, 1000, 1.5f);
            DecideOrbitDirection();
        }
    }

    private void DecideOrbitDirection()
    //decide the direction of the orbiting rotation around the planet, based on the position of the ship relative to the planet
    {
        if (Vector2.SignedAngle(rigidBody2D.velocity, (transform.position - orbitOrigin)) > 0)
        {
            movementSpeedModifier = -Mathf.Abs(movementSpeedModifier);
        }
        else
        {
            movementSpeedModifier = Mathf.Abs(movementSpeedModifier);
        }
    }

    private void InGravityField()
    {
        float distanceToPlanet = Vector2.Distance(transform.position, orbitOrigin);

        if (distanceToPlanet > lastDistanceToPlanet && distanceToPlanet > 2 && superSlideCounter <= 0)
        {
            rigidBody2D.velocity = Vector2.zero;
            nearPlanet.GetComponent<PointEffector2D>().enabled = false;
            distanceToOrbitingPlanet = Vector2.Distance(orbitOrigin, transform.position);

            currentState = State.Orbiting;
        }

        lastDistanceToPlanet = distanceToPlanet;
    }

    private void SetColoursForBoostParticlesGradients()
    {
        particleBoost = transform.GetChild(1).GetComponent<ParticleSystem>();
        boostGradientRed = new Gradient();
        boostGradientRed.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(1f, 0.63f, 0f), 0.0f), new GradientColorKey(new Color(0.94f, 0.82f, 0f), 20.0f), new GradientColorKey(new Color(0.89f, 1f, 0f), 50.0f) }, new GradientAlphaKey[] { });
        boostGradientBlue = new Gradient();
        boostGradientBlue.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(0.12f, 0f, 1f), 0.0f), new GradientColorKey(new Color(0.06f, 0.44f, 0.99f), 0.2f), new GradientColorKey(new Color(0.16f, 0.96f, 1f), 0.5f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });
    }

    private void AnimateDeath()
    {
        var boostParticlesEmission = particleBoost.emission;
        boostParticlesEmission.enabled = false;

        ParticleSystem explosionParticleSystem = transform.GetChild(2).GetComponent<ParticleSystem>();
        explosionParticleSystem.Play();

        SpriteRenderer playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        playerSprite.enabled = false;

        rigidBody2D.velocity = Vector2.zero;
    }
    #endregion
}
