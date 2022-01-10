using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public int lives = 3;
    public int score = 0;

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        // TODO: increase score
    }

    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;

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
        // TODO
    }
}
