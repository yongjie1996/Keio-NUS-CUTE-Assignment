using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script used for Game Manager game object.
/// Used for various game methods that either cannot be controlled by specified game object it is used for
/// or methods that do not fit into any game object in particular.
/// Manages various states of the game such as player life count, score, level and pause state, as well as UI elements.
/// Able to save and load game progress.
/// </summary>
/// <see cref="SaveData"/> on what variables are saved from the game.
/// <seealso cref="SaveSystem"/> on how the variables are saved.
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

    /// <summary>
    /// Show player how to win the game and update text UI
    /// Disable text showing how to win the game after 2 seconds
    /// Get audio component to play sound whenever an asteroid gets destroyed
    /// </summary>
    /// <see cref="SetObjectiveTextInactive"/> to see how the win objective text is disabled
    private void Awake()
    {
        this.scoreText.text = "Score: " + score.ToString();
        this.livesText.text = "Lives: " + lives.ToString();
        Invoke(nameof(SetObjectiveTextInactive), 2.0f);
        this.levelText.text = "Level " + level.ToString();
        asteroidSound = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Check if player paused the game
    /// Player can pause the game by pressing the escape key
    /// </summary>
    /// <see cref="ResumeGame"/> for how the game will be resumed after pausing
    /// <seealso cref="PauseGame"/> for how the game will be paused
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

    /// <summary>
    /// Method to be called whenever an asteroid gets destroyed by a bullet
    /// Plays the SFX and particle effect for asteroid destruction
    /// Adds score according to the size of the asteroid
    /// Updates asteroid speed and player fire rate if updated score brings the level up
    /// Checks if player wins the game after destroying the asteroid
    /// </summary>
    /// 
    /// <param name="asteroid">Asteroid game object to be passed into the method for position and size</param>
    /// 
    /// <remarks>
    /// I did not put the checking of score for asteroid speed and fire rate in Update() as doing it this way is less intensive
    /// for the game as they would only need to check whenever the score gets updated, which is when the asteroid is destroyed
    /// by a bullet. The same reason as for why this method would check if the Level is 11 or more for when player wins the game
    /// with 10000 score or more.
    /// </remarks>
    /// 
    /// <see cref="AsteroidSpawner.SetSpeed(int)"/> for how the speed is calculated according to the level
    /// <seealso cref="Player.SetNewFireRate(int)"/> for how the fire rate is calculated according to the level
    /// <seealso cref="WinGame"/> for what happens when the player wins the game
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

        this.scoreText.text = "Score: " + score.ToString();
        this.level = score / 1000 + 1;

        asteroidSpawner.SetSpeed(level);
        player.SetNewFireRate(level);

        if (this.level >= 11) {
            WinGame();
        }
        else
        {
            this.levelText.text = "Level " + level.ToString();
        }        
    }

    /// <summary>
    /// Method to be called whenever the player dies
    /// Plays particle effect at player death position
    /// Minus 1 live count
    /// Invoke Respawn method if there are lives remaining, otherwise call GameOver method
    /// </summary>
    /// <see cref="GameOver"/> to see what happens when the player runs out of lives
    /// <seealso cref="Respawn"/> to see what happens when the player respawns
    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;

        livesText.text = "Lives: " + lives.ToString();

        if (this.lives <= 0)
        {
            GameOver();
        } 
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
    }

    /// <summary>
    /// Resets the player position back to the center of the game
    /// Set player game object to active
    /// </summary>
    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
    }

    /// <summary>
    /// Destroy every Asteroid game object in the scene
    /// </summary>
    private void DestroyAllAsteroids()
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

    /// <summary>
    /// Destroy every Bullet game object in the scene
    /// </summary>
    private void DestroyAllBullets()
    {
        GameObject[] bullets;

        bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }   
    }

    /// <summary>
    /// Shows text that player lost the game and show button to play the game again
    /// </summary>
    private void GameOver() 
    {
        this.asteroidSpawner.gameObject.SetActive(false);

        DestroyAllAsteroids();

        this.gameOverText.gameObject.SetActive(true);
        this.replayButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Sets text showing win objective to not be not active
    /// </summary>
    private void SetObjectiveTextInactive()
    {
        this.winObjectiveText.gameObject.SetActive(false);
    }

    /// <summary>
    ///  Shows text that player won the game and show button to play the game again
    /// </summary>
    private void WinGame () 
    {
        this.asteroidSpawner.gameObject.SetActive(false);

        DestroyAllAsteroids();

        this.player.gameObject.SetActive(false);
        this.playerWinsText.gameObject.SetActive(true);
        this.replayButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Allows the player to play the game again with score, level and lives reset
    /// </summary>
    public void ReplayButtonOnPress()
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
        this.player.SetNewFireRate(this.level);

        Respawn();
    }

    /// <summary>
    /// Saves the game progress
    /// </summary>
    /// <see cref="SaveData"/> for what variables are saved from the game
    /// <seealso cref="SaveSystem"/> for how the variables are saved
    public void SaveData()
    {
        SaveSystem.SaveProgress(this);
    }

    /// <summary>
    /// Loads the game progress, updating the level, lives, score and player position from the previously saved progress
    /// Also destroys all asteroids and bullets in the scene
    /// </summary>
    /// <see cref="SaveData"/> for what variables are saved from the game
    /// <seealso cref="SaveSystem"/> for how the variables are saved
    public void LoadData()
    {
        DestroyAllAsteroids();
        DestroyAllBullets();

        SaveData data = SaveSystem.LoadData();

        this.level = data.level;
        this.lives = data.lives;
        this.score = data.score;

        this.scoreText.text = "Score: " + score.ToString();
        this.livesText.text = "Lives: " + lives.ToString();
        this.levelText.text = "Level " + level.ToString();

        this.asteroidSpawner.SetSpeed(this.level);
        this.player.SetNewFireRate(this.level);

        Vector3 playerPosition;
        playerPosition.x = data.playerPosition[0];
        playerPosition.y = data.playerPosition[1];
        playerPosition.z = data.playerPosition[2];
        this.player.transform.position = playerPosition;
    }

    /// <summary>
    /// Resumes the game if the game is paused
    /// </summary>
    public void ResumeGame()
    {
        pauseMenuUI.gameObject.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        player.GetComponent<Player>().enabled = true;
    }

    /// <summary>
    /// Pauses the game and brings up the pause menu
    /// Disable Player script to prevent player from shooting bullets while game is still paused
    /// </summary>
    public void PauseGame()
    {
        pauseMenuUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        player.GetComponent<Player>().enabled = false;
    }

    /// <summary>
    /// Closes the game application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
