using UnityEngine;

/// <summary>
/// Script used for Spawner game object.
/// Spawns asteroids repeatedly at the set rate outside the boundaries of the game.
/// Randomizes the rotation, size and range of direction of the spawned asteroid towards the player.
/// </summary>
/// <see cref="Asteroid"/> for behavior of Asteroid game object after it spawns.
/// <seealso cref="GameManager"/> for Game Manager object related functions used in this script.

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float trajectoryVariance = 15.0f;
    public float spawnRate = 2.5f;
    public float spawnDistance = 15.0f;
    public int spawnAmount = 1;
    private float asteroidOriginalSpeed;

    /// <summary>
    /// Spawns asteroids at a fixed rate
    /// </summary>
    /// <see cref="Spawn"/> for how the asteroids are spawned
    private void Start()
    {
        asteroidOriginalSpeed = asteroidPrefab.speed;
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate); 
    }

    /// <summary>
    /// Method to spawn an asteroid at a random point from a radius outside the game boundaries
    /// Asteroids spawned will have a random rotation, size, and speed relative to the size
    /// Asteroids after spawning will move in the general direction towards the player
    /// Called in Start method repeatedly through InvokeRepeating
    /// </summary>
    private void Spawn()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance; /// the radius of player initial position
            Vector3 spawnPoint = this.transform.position + spawnDirection; /// random spawn area outside of above mentioned radius

            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance); /// range of direction to generally point towards player after spawning
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward); 

            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize); /// randomize asteroid size
            asteroid.SetTrajectory(rotation * -spawnDirection); /// set the asteroid to head to the player's general direction
        }
    }

    /// <summary>
    /// Sets the speed of the spawned asteroid adjusted to the level of the game
    /// </summary>
    /// <param name="level">Current level of the game, to be used to determine the speed of the asteroids</param>
    public void SetSpeed(int level) /// set speed of asteroid according to the level
    {
        asteroidPrefab.speed = asteroidOriginalSpeed + ((level - 1) * 2.0f);
    }
}
