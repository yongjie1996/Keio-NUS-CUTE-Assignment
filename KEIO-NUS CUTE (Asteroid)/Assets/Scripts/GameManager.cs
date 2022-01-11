using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public AsteroidSpawner asteroidSpawner;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public int lives = 3;
    public int score = 0;
    public int level = 1;
    public Canvas pauseMenuUI;
    public TMP_Text winObjectiveText;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text gameOverText;
    public TMP_Text levelText;
    public TMP_Text playerWinsText;
    public Button replayButton;
    private AudioSource asteroidSound;
    private bool gamePaused = false;

    private void Awake()
    {
        this.scoreText.text = "Score: " + score.ToString();
        this.livesText.text = "Lives: " + lives.ToString();
        Invoke(nameof(SetObjectiveTextInactive), 2.0f); // show objective to win game for 2 seconds
        this.levelText.text = "Level " + level.ToString();
        asteroidSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        asteroidSound.Play();

        if (asteroid.size < 0.75f)
        {
            this.score += 100;
        }
        else if (asteroid.size < 1.2f)
        {
            this.score += 50;
        }
        else
        {
            this.score += 25;
        }

        this.scoreText.text = "Score: " + score.ToString(); // update score text
        this.level = score / 1000 + 1;

        asteroidSpawner.SetSpeed(level);
        player.setNewFireRate(level);

        if (this.level >= 10) {
            WinGame();
        }
        else
        {
            this.levelText.text = "Level " + level.ToString();
        }        
    }

    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;

        livesText.text = "Lives: " + lives.ToString(); // update lives text

        if (this.lives <= 0) // end game if player lives is equal or less than zero
        {
            GameOver();
        } 
        else // otherwise respawn the player after dying, invoking respawn according to the respawn timer
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
    }

    private void Respawn() // set the player object back to active
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
    }

    private void DestroyAllAsteroids() // destroy every asteroid game object in the game
    {
        GameObject[] asteroids;

        asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject asteroid in asteroids)
        {
            this.explosion.transform.position = asteroid.transform.position;
            this.explosion.Play();
            Destroy(asteroid);
        }
    }

    private void GameOver()// shows text that player lost the game and show button to play the game again
    {
        this.asteroidSpawner.gameObject.SetActive(false);

        DestroyAllAsteroids();

        this.gameOverText.gameObject.SetActive(true);
        this.replayButton.gameObject.SetActive(true);
    }

    private void SetObjectiveTextInactive() // set text that shows game objective inactive
    {
        this.winObjectiveText.gameObject.SetActive(false);
    }

    private void WinGame ()  // shows text that player won the game and show button to play the game again
    {
        this.asteroidSpawner.gameObject.SetActive(false);

        DestroyAllAsteroids();

        this.player.gameObject.SetActive(false);
        this.playerWinsText.gameObject.SetActive(true);
        this.replayButton.gameObject.SetActive(true);
    }

    public void ReplayButtonOnPress() // restarts the game for the player
    {
        this.asteroidSpawner.gameObject.SetActive(true);
        this.gameOverText.gameObject.SetActive(false);
        this.playerWinsText.gameObject.SetActive(false);
        this.replayButton.gameObject.SetActive(false);

        this.lives = 3;
        this.score = 0;
        this.level = 1;

        this.scoreText.text = "Score: " + score.ToString();
        this.livesText.text = "Lives: " + lives.ToString();
        this.levelText.text = "Level " + level.ToString();

        this.asteroidSpawner.SetSpeed(this.level);
        this.player.setNewFireRate(this.level);

        Respawn();
    }

    public void SaveData() // save relevant data into a binary file
    {
        SaveSystem.SaveProgress(this);
    }

    public void LoadData() // load last saved data from binary file into game
    {
        DestroyAllAsteroids();

        SaveData data = SaveSystem.LoadData();

        this.level = data.level;
        this.lives = data.lives;
        this.score = data.score;

        this.scoreText.text = "Score: " + score.ToString();
        this.livesText.text = "Lives: " + lives.ToString();
        this.levelText.text = "Level " + level.ToString();

        this.asteroidSpawner.SetSpeed(this.level);
        this.player.setNewFireRate(this.level);

        Vector3 playerPosition;
        playerPosition.x = data.playerPosition[0];
        playerPosition.y = data.playerPosition[1];
        playerPosition.z = data.playerPosition[2];
        this.player.transform.position = playerPosition;
    }

    public void ResumeGame() // resumes game by having game time flow normally
    {
        pauseMenuUI.gameObject.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        player.GetComponent<Player>().enabled = true;
    }

    public void PauseGame() // stops game time in order to pause
    {
        pauseMenuUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        player.GetComponent<Player>().enabled = false;
    }

    public void QuitGame() // kill application
    {
        Application.Quit();
    }
}
