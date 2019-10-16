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

    private float rotationSpeed = 0;
    private Vector3 rotationVector;
    private GameObject GravityField;
    #endregion

    #region Unity's Functions
    // Start is called before the first frame update
    void Start()
    {
        GravityField = transform.GetChild(1).gameObject;

        while (rotationSpeed == 0)
        {
            rotationSpeed = Random.Range(-50, 50);
        }
        rotationVector = new Vector3(0, 0, rotationSpeed / 100);

        GravityField.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        GravityField.transform.Rotate(rotationVector);
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
    #endregion
}