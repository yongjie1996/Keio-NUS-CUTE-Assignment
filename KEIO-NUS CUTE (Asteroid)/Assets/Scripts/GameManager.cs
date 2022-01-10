using UnityEngine;
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
    public TMP_Text winObjectiveText;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text gameOverText;
    public TMP_Text levelText;
    public TMP_Text playerWinsText;

    private void Awake()
    {
        this.scoreText.text = "Score: " + score.ToString();
        this.livesText.text = "Lives: " + lives.ToString();
        Invoke(nameof(SetObjectiveTextInactive), 2.0f); // show objective to win game for 2 seconds
        Invoke(nameof(SetLevelTextActive), 2.0f); // show level after objective is set inactive
        this.levelText.text = "Level " + level.ToString();
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

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

    private void GameOver()
    {
        this.gameOverText.gameObject.SetActive(true);
    }

    private void SetObjectiveTextInactive()
    {
        this.winObjectiveText.gameObject.SetActive(false);
    }

    private void SetLevelTextActive()
    {
        this.levelText.gameObject.SetActive(true);
    }

    private void WinGame ()
    {
        this.asteroidSpawner.gameObject.SetActive(false);

        GameObject[] asteroids;

        asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach(GameObject asteroid in asteroids)
        {
            this.explosion.transform.position = asteroid.transform.position;
            this.explosion.Play();
            Destroy(asteroid);
        }

        this.playerWinsText.gameObject.SetActive(true);
    }
}
