using UnityEngine;

/// <summary>
/// Script used for Asteroid game object behavior.
/// Moves in the general direction towards the player in a straight line after spawning outside of the game boundaries.
/// Able to move in and out of game boundaries.
/// Splits in half if the size is bigger than 1.0f in value after getting destroyed by a bullet, otherwise it just gets destroyed.
/// Will destroy itself after 30 seconds to prevent overloading the game with too many asteroid game objects present in the game.
/// </summary>
/// <see cref="AsteroidSpawner"/> for the spawner object behavior on how it instantiates asteroid game objects.
/// <seealso cref="GameManager"/> for Game Manager object related functions used in this script.

public class Asteroid : MonoBehaviour
{
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 50.0f;
    public float maxLifetime = 30.0f;
    private Rigidbody2D _rigidbody;

    /// <summary>
    /// Retrives rigidbody2D component of the asteroid game object to be used for other methods
    /// </summary>
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Randomizes the sprite rotation and size of the asteroid game object
    /// Mass of game object is determined by game object size
    /// </summary>
    private void Start()
    {
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f); /// random sprite rotation for spawned asteroid
        this.transform.localScale = Vector3.one * this.size; /// random sprite scale

        _rigidbody.mass = this.size * 2.0f;
    }

    /// <summary>
    /// Sets the course of the asteroid game object towards the player's general direction
    /// </summary>
    /// <param name="direction">Randomized direction in general direction to the player</param>
    /// <see cref="AsteroidSpawner.Spawn"/> for how the direction of the asteroid is determined
    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);

        Destroy(this.gameObject, maxLifetime);
    }

    /// <summary>
    /// Check if collided game object is bullet and destroy the asteroid if it is
    /// If the size of the asteroid is big enough, it will split into two instead
    /// </summary>
    /// <param name="collision">Game object collided to be compared if the tag of the object is "Bullet"</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) /// if collided with bullet
        {
            if ((this.size * 0.5) >= this.minSize ) /// create 2 smaller asteroids if the asteroid is bigger than minSize if split
            {
                CreateSplit();
                CreateSplit();
            }

            FindObjectOfType<GameManager>().AsteroidDestroyed(this); 
            Destroy(this.gameObject);        
        }       
    }

    /// <summary>
    /// Make a half sized asteroid of the current existing one
    /// </summary>
    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }

}
