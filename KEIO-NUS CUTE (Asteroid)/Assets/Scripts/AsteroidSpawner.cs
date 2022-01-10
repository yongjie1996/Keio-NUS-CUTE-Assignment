using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float trajectoryVariance = 15.0f;
    public float spawnRate = 2.5f;
    public float spawnDistance = 15.0f;
    public int spawnAmount = 1;
    private float asteroidOriginalSpeed;
    private void Start()
    {
        asteroidOriginalSpeed = asteroidPrefab.speed;
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate); // repeat spawn of asteroid according to the given rate
    }

    private void Spawn()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance; // the radius of player initial position
            Vector3 spawnPoint = this.transform.position + spawnDirection; // random spawn area outside of above mentioned radius

            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance); // range of direction to generally point towards player after spawning
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward); 

            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation); // spawn asteroid
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize); // randomize asteroid size
            asteroid.SetTrajectory(rotation * -spawnDirection); // set the asteroid to head to the player's general direction
        }
    }

    public void SetSpeed(int level) // set speed of asteroid according to the level
    {
        asteroidPrefab.speed += asteroidOriginalSpeed + ((level - 1) * 5.0f);
    }
}
