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
    public float currentSize;
    public float startingSize = 3.5f;

    [SerializeField] private Sprite[] spritesCollection;

    private GameObject gravityField;
    private SpriteRenderer gravityFieldSprite;
    private GameObject planetSprite;
    private bool isSpawning = true;
    private bool isBeingDestroyed = false;
    private bool blinkingAnimation = true;
    private float lerpPlanet = 0.0f;
    private float lerpGravField;
    #endregion

    #region Unity's Functions
    void Start()
    {
        gravityField = transform.GetChild(1).gameObject;
        gravityFieldSprite = gravityField.GetComponent<SpriteRenderer>();
        planetSprite = transform.GetChild(0).gameObject;

        gravityField.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        planetSprite.GetComponent<SpriteRenderer>().sprite = spritesCollection[Random.Range(0, spritesCollection.Length)];
    }

    void Update()
    {
        SpawnAnimation();

        GravityFieldBlinkingAnimate();

        DestroyAnimation();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AdjustGravityFieldScale(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        float ratio = currentSize / startingSize;
        transform.GetChild(1).transform.localScale = new Vector3(ratio, ratio, 1);
    }
    #endregion

    #region Functions
    private void AdjustGravityFieldScale(GameObject player)
    //change the radius of the sprite of the gravity field
    {
        float ratio = currentSize / startingSize;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        float distRatio = Mathf.Clamp((distance / currentSize) * ratio, 0, ratio);
        Transform gravFieldNearPlanet = transform.GetChild(1);
        gravFieldNearPlanet.transform.localScale = new Vector3(distRatio, distRatio);
    }

    private void SpawnAnimation()
    {
        if (isSpawning == true)
        {
            float ratio = currentSize / startingSize;
            if (gravityField.transform.localScale.x != ratio)
            {
                lerpPlanet += 3.0f * Time.deltaTime;
                float lerpedScale = Mathf.Lerp(0.0f, ratio, lerpPlanet);
                planetSprite.transform.localScale = new Vector3(lerpedScale, lerpedScale, 1);

                lerpGravField += 2.0f * Time.deltaTime;
                lerpedScale = Mathf.Lerp(0.0f, ratio, lerpGravField);
                gravityField.transform.localScale = new Vector3(lerpedScale, lerpedScale, 1);
            }
            else
            {
                isSpawning = false;
            }
        }
    }

    public void DestroyPlanet()
    {
        isBeingDestroyed = true;
        lerpPlanet = 0.0f;
    }

    private void DestroyAnimation()
    {
        if (isBeingDestroyed == true)
        {
            float ratio = currentSize / startingSize;
            lerpPlanet += 3.0f * Time.deltaTime;
            float lerpedScale = Mathf.Lerp(ratio, 0.0f, lerpPlanet);
            planetSprite.transform.localScale = new Vector3(lerpedScale, lerpedScale, 1);
            gravityField.transform.localScale = new Vector3(lerpedScale, lerpedScale, 1);

            if (gravityField.transform.localScale.x < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void GravityFieldBlinkingAnimate()
    {
        if(blinkingAnimation == true)
        {
            gravityFieldSprite.color = new Color(gravityFieldSprite.color.r, gravityFieldSprite.color.g, gravityFieldSprite.color.b, gravityFieldSprite.color.a - 0.5f * Time.deltaTime);

            if(gravityFieldSprite.color.a < 0.3f)
            {
                blinkingAnimation = false;
            }
        }
        else
        {
            gravityFieldSprite.color = new Color(gravityFieldSprite.color.r, gravityFieldSprite.color.g, gravityFieldSprite.color.b, gravityFieldSprite.color.a + 0.5f * Time.deltaTime);

            if (gravityFieldSprite.color.a >= 1.0f)
            {
                blinkingAnimation = true;
            }
        }

    }
    #endregion
}