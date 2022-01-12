using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Script used for bullet game object. 
///  To be fired by player game object using left mouse button or spacebar. 
///  Destroys itself after hitting the game boundary or an asteroid.
/// </summary>
/// <see cref="Player"/> for other player controls


public class Bullet : MonoBehaviour
{
    public float speed = 500.0f;
    public float maxLifetime = 10.0f;
    private Rigidbody2D _rigidbody;

    /// <summary>
    /// Retrives rigidbody2D component of the bullet game object to be used for other methods
    /// </summary>
    private void Awake ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Adds force to the bullet so that it moves
    /// </summary>
    /// <param name="direction">The direction of the player is facing, determining the direction of the bullet</param>
    public void Project(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);

        Destroy(this.gameObject, this.maxLifetime); /// destroy this game object after set time so it doesn't linger in the game
    }

    /// <summary>
    /// Destroy bullet game object upon collision with anything that is not the player
    /// </summary>
    /// <param name="collision">Any game object with a collider</param>
    /// <remarks>
    /// The layer the bullet is on will not collide with the player's layer
    /// </remarks>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
