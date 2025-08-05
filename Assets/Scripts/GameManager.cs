using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    public bool gameIsProccessing;

    private RepeatBackground background;

    public float score;

    public TextMeshProUGUI scoreText;
    public GameObject gameOverText;

    public GameObject restartButton;



    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        background = GameObject.Find("Background").GetComponent<RepeatBackground>();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsProccessing)
        {
            score += (Time.deltaTime * background.speed) / 5;

            UpdateScore();
        }
    }

    public void EndGame()
    {
        gameIsProccessing = false;
        restartButton.SetActive(true);
        gameOverText.SetActive(true);
    }

    public void StartGame()
    {
        gameIsProccessing = true;
        score = 0f;
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + (int)score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
