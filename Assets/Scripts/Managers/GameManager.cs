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
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Objects References")]
    public TextMeshProUGUI scoreText;
    public Text gameOverText;
    public GameObject planetsToSpawn;
    public GameObject scorePopup;

    [Header("Values")]
    [SerializeField] private int numberOfPlanets = 3;

    private enum State
    {
        InGame,
        GameOver
    }
    private State currentState = State.InGame;

    private float startingSizeOfPlanets;
    private float currentSizeOfPlanets;
    private Vector2 minRangeToSpawn = new Vector2(-14.5f, -7f);
    private Vector2 maxRangeToSpawn = new Vector2(14.5f, 7f);
    private int score = 0;
    private int displayScore = 0;
    #endregion

    #region Unity's Functions
    void Start()
    {
        startingSizeOfPlanets = planetsToSpawn.GetComponent<CircleCollider2D>().radius;
        currentSizeOfPlanets = startingSizeOfPlanets;

        scoreText.text = "SCORE : " + score.ToString();
        StartCoroutine(ScoreUpdater());

        MaintainNumbersOfPlanets(null);

        GameEvents.current.onScoreIncrease += IncrementScore;
        GameEvents.current.onEnteringNewPlanet += MaintainNumbersOfPlanets;
        GameEvents.current.onGameOver += GameOver;
        GameEvents.current.onInstantiateScorePopup += InstantiateScorePopup;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == State.GameOver)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState == State.GameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onScoreIncrease -= IncrementScore;
        GameEvents.current.onEnteringNewPlanet -= MaintainNumbersOfPlanets;
        GameEvents.current.onGameOver -= GameOver;
        GameEvents.current.onInstantiateScorePopup -= InstantiateScorePopup;
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
            float xAxis = UnityEngine.Random.Range(minRangeToSpawn.x, maxRangeToSpawn.x);
            float yAxis = UnityEngine.Random.Range(minRangeToSpawn.y, maxRangeToSpawn.y);
            randomSpawnPosition = new Vector3(xAxis, yAxis, 0);

            if (Physics2D.OverlapCircle(randomSpawnPosition, 4.0f) == null)
            {
                GameObject spawnedPlanet = Instantiate(planetsToSpawn, randomSpawnPosition, Quaternion.identity);

                spawnedPlanet.GetComponent<CircleCollider2D>().radius = currentSizeOfPlanets;
                spawnedPlanet.GetComponent<Planet>().currentSize = currentSizeOfPlanets;

                currentSizeOfPlanets -= 0.05f;
                break;
            }
            else
            {
                i++;
            }
        }

    }

    public void MaintainNumberOfPlanets(GameObject lastPlanet, int totalNumberOfPlanets)
    //remove the last planet and spawn another one if the count is low enough
    {
        if (lastPlanet != null)
        {
            lastPlanet.GetComponent<Planet>().DestroyPlanet();
        }

        while (GameObject.FindGameObjectsWithTag("Planet").Length <= totalNumberOfPlanets)
        {
            SpawnPlanetAtRandomPosition();
        }
    }

    public void MaintainNumbersOfPlanets(GameObject lastPlanet)
    {
        MaintainNumberOfPlanets(lastPlanet, numberOfPlanets);
    }

    public void IncrementScore(int numberToAdd)
    {
        score += numberToAdd;
    }

    private IEnumerator ScoreUpdater()
    //makes the visible score increment visibly over time on screen to match the actual score
    {
        while (true)
        {
            if (displayScore < score && (score - displayScore) > 300)
            {
                displayScore += 100;
                scoreText.text = "SCORE : " + displayScore.ToString();
            }
            else if (displayScore < score && (score - displayScore) > 10)
            {
                displayScore += 10;
                scoreText.text = "SCORE : " + displayScore.ToString();
            }
            else if (displayScore < score)
            {
                displayScore += 1;
                scoreText.text = "SCORE : " + displayScore.ToString();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void InstantiateScorePopup(Transform position, int score, float size)
    {
        GameObject popup = Instantiate(scorePopup, position);
        popup.GetComponent<TextMeshPro>().text = "+" + score.ToString();
        popup.transform.localScale = new Vector3(size, size, 0);
    }

    public void GameOver()
    {
        currentState = State.GameOver;

        AudioManager.current.Playsound("Explosion");

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            gameOverText.text = "NEW HIGHSCORE : " + score.ToString() + "\n (old highscore = " + PlayerPrefs.GetInt("HighScore", 0).ToString() + ")";
            PlayerPrefs.SetInt("HighScore", score);
        }
        else
        {
            gameOverText.text = "score : " + score.ToString() + "\n highscore = " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }
    #endregion
}
