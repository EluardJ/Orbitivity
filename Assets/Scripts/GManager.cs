#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    #region Variables
    public GameObject planet;
    private Vector2 minRange = new Vector2(-14.5f, -7f);
    private Vector2 maxRange = new Vector2(14.5f, 7f);
    public int numberOfPlanets = 3;
    #endregion

    #region Unity's Functions
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        //restart the scene FOR TESTING PURPOSE
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.S))
        //spawns a planet at random position FOR TESTING PURPOSE
        {
            SpawnPlanetAtRandomPosition();
        }
    }
    #endregion

    #region Functions
    private void SpawnPlanetAtRandomPosition(int maxIter = 100)
    //try to spawn a planet at a random position, and if it overlaps with another one try to find a new pos for maxIter iterations
    {
        Vector3 randomSpawnPosition = Vector3.zero;
        int i = 0;

        while (i < maxIter)
        {
            float xAxis = UnityEngine.Random.Range(minRange.x, maxRange.x);
            float yAxis = UnityEngine.Random.Range(minRange.y, maxRange.y);
            randomSpawnPosition = new Vector3(xAxis, yAxis, 0);

            if (Physics2D.OverlapCircle(randomSpawnPosition, 5) == null)
            {
                Instantiate(planet, randomSpawnPosition, Quaternion.identity);
                Debug.Log("worked the first time");
                break;
            }
            else
            {
                i++;
                Debug.Log("tried " + i + " times");
            }
        }

    }

    public void MaintainNumberOfPlanets(GameObject lastPlanet, int nbr)
    //remove the last planet and spawn another one if the count is low enough
    {
        if(lastPlanet != null)
        {
            Destroy(lastPlanet);
        }

        while (GameObject.FindGameObjectsWithTag("Planet").Length <= nbr)
        {
            SpawnPlanetAtRandomPosition();
        }
    }
    #endregion
}
