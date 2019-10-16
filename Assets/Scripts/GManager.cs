#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    #region Variables
    public int numberOfPlanets = 3;
    public Text scoreText;
    public Text gameOverText;
    public GameObject planetsToSpawn;

    private float startingSizeOfPlanets;
    private float currentSizeOfPlanets;
    private Vector2 minRange = new Vector2(-14.5f, -7f);
    private Vector2 maxRange = new Vector2(14.5f, 7f);
    private int score = 0;
    private int displayScore = 0;
    #endregion

    #region Unity's Functions
    // Start is called before the first frame update
    void Start()
    {
        startingSizeOfPlanets = planetsToSpawn.GetComponent<CircleCollider2D>().radius;
        currentSizeOfPlanets = startingSizeOfPlanets;

        scoreText.text = "SCORE : " + score.ToString();
        StartCoroutine(ScoreUpdater());

        MaintainNumberOfPlanets(null, numberOfPlanets);
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
    //try to spawn a random planet at a random position, and if it overlaps with another one try to find a new pos for maxIter iterations, instantiate it if it free
    {
        Vector3 randomSpawnPosition = Vector3.zero;
        int i = 0;

        while (i < maxIter)
        {
            float xAxis = UnityEngine.Random.Range(minRange.x, maxRange.x);
            float yAxis = UnityEngine.Random.Range(minRange.y, maxRange.y);
            randomSpawnPosition = new Vector3(xAxis, yAxis, 0);

            if (Physics2D.OverlapCircle(randomSpawnPosition, planetsToSpawn.GetComponent<CircleCollider2D>().radius) == null)
            {
                GameObject spawnedPlanet = Instantiate(planetsToSpawn, randomSpawnPosition, Quaternion.identity);

                //set the planet at the right size and reduce the current size
                Debug.Log(currentSizeOfPlanets + " " + startingSizeOfPlanets);
                float ratio = currentSizeOfPlanets / startingSizeOfPlanets;
                spawnedPlanet.GetComponent<CircleCollider2D>().radius = currentSizeOfPlanets;
                spawnedPlanet.transform.GetChild(1).localScale = new Vector3(ratio, ratio, 1);
                spawnedPlanet.GetComponent<Planet>().currentSize = currentSizeOfPlanets;

                currentSizeOfPlanets -= 0.2f;
                break;
            }
            else
            {
                i++;
            }
        }

    }

    public void MaintainNumberOfPlanets(GameObject lastPlanet, int nbr)
    //remove the last planet and spawn another one if the count is low enough
    {
        if (lastPlanet != null)
        {
            Destroy(lastPlanet);
        }

        while (GameObject.FindGameObjectsWithTag("Planet").Length <= nbr)
        {
            SpawnPlanetAtRandomPosition();
        }
    }

    public void IncrementScore(int numberToAdd)
    //increment and updates the score
    {
        score += numberToAdd;
    }

    private IEnumerator ScoreUpdater()
    {
        while (true)
        {
            if(displayScore < score && (score - displayScore) > 300)
            {
                displayScore  += 100;
                scoreText.text = "SCORE : " + displayScore.ToString();
            }
            else if(displayScore < score)
            {
                displayScore += 10;
                scoreText.text = "SCORE : " + displayScore.ToString();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void GameOver()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            gameOverText.text = "GAME OVER\npress R to retry\nNEW HIGHSCORE : " + score.ToString() + "\n (old highscore = " + PlayerPrefs.GetInt("HighScore", 0).ToString() + ")";
            PlayerPrefs.SetInt("HighScore", score);
        }
        else
        {
            gameOverText.text = "GAME OVER\npress R to retry\nscore : " + score.ToString() + "\n highscore = " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }
    #endregion
}
