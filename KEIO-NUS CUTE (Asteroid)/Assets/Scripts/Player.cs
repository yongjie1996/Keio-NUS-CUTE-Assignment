using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used for Player game object.
/// Allows player object to move using keyboard inputs.
/// Able to shoot bullets to destroy asteroids.
/// Dies if an asteroid collides into player game object.
/// Respawns after dying and gets 2 seconds of invulnerability.
/// </summary>
/// <see cref="GameManager"/> for methods related on how Game Manager handles player respawn after death.
public class Player : MonoBehaviour
{
    public GameManager _gameManager;
    public float fireRate = 0.2f; // used in update to countdown before new bullet can be fired
    public Bullet bulletPrefab;
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    public float invulnerabilityTimer = 3.0f;
    private Animator animator;
    private Rigidbody2D _rigidbody;
    private bool _thrusting;
    private bool _reversing;
    private float _turnDirection;
    private float setFireRate; // current fire rate in game
    private float originalFireRate; // store base fire rate for method setNewFireRate as the other two variables are used constantly in Update

    /// <summary>
    /// Gets animation and rigidbody2D components to store as variables, used for other methods.
    /// Sets a variable for fire rate to be reset to and the original value of the set fire rate configurized to be used as base fire rate
    /// </summary>
    /// <see cref="DisableBlink"/> for how the sprite animation is handled to be disabled
    /// <seealso cref="FixedUpdate"/> for how controls are mechanically handled
    /// <seealso cref="SetNewFireRate(int)"/> for how fire rate is determined per level progression
    private void Awake()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        setFireRate = fireRate;
        originalFireRate = fireRate;
    }

    /// <summary>
    /// Allow sprite blinking animation to play and give player invulnerability for 2 seconds on spawn.
    /// Turn collision back on for player after 2 seconds.
    /// </summary>
    /// <see cref="TurnOnCollisions"/> for how player collision is turned on for player after invulnerability period is up
    /// <seealso cref="DisableBlink"/> for how the sprite animation is handled to be disabled
    private void OnEnable()
    {
        animator.enabled = true;
        this.gameObject.layer = LayerMask.NameToLayer("RespawnedPlayer"); // this layer prevents collisions for a set amount of time to prevent player from dying immediately on respawn
        Invoke(nameof(TurnOnCollisions), invulnerabilityTimer); // switch back to original player layer after invulnerbility timer is up
        Invoke(nameof(DisableBlink), invulnerabilityTimer);
    }

    /// <summary>
    /// Tracks player inputs for controls and timer for fire rate.
    /// Player can shoot whenever fire rate timer is 0.0f or lower.
    /// </summary>
    /// <see cref="Shoot"/> for method allowing player to shoot
    private void Update()
    {
        if (fireRate > 0.0f) // countdown for player fire rate
        {
            fireRate -= Time.deltaTime;
        }

        _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _reversing = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1.0f;
        } 
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _turnDirection = -1.0f;
        } 
        else
        {
            _turnDirection = 0.0f;
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if (fireRate < 0.0f) // limit player fire rate
            {
                Shoot();
                fireRate = setFireRate;
            }           
        }
    }
    /// <summary>
    /// Used for physics as making the related game physics attribute to regular Update method would make it dependent on framerate
    /// </summary>
    private void FixedUpdate()
    {
        if (_thrusting)
        {
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed); // move forward
        }

        if (_reversing)
        {
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed * -1.0f); // move backward
        }

        if (_turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(_turnDirection * this.turnSpeed); // apply turning direction
        }
    }

    /// <summary>
    /// Allows player object to shoot bullets by calling Project after instantiating a bullet
    /// </summary>
    /// <see cref="Bullet"/> for other bullet related methods
    /// <seealso cref="Bullet.Project(Vector2)"/> for how the bullet is made to move
    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    /// <summary>
    /// If the player collides with an asteroid, they die and respawn
    /// </summary>
    /// <param name="collision">Collided object to be compared with tag</param>
    /// <see cref="GameManager.PlayerDied"/> to see how player death and respawn is handled
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid")) /// if collided to an asteroid
        {
            _rigidbody.velocity = Vector3.zero; /// reset player velocity
            _rigidbody.angularVelocity = 0.0f; /// reset torques applied to player

            this.gameObject.SetActive(false);

            _gameManager.PlayerDied();
        }
    }

    /// <summary>
    /// Change the layer of player game object that allows collision after invulnerability is up
    /// Respawned players get invulnerability for 2 seconds by being on a layer that does not allow collisions
    /// </summary>
    /// <see cref="GameManager.PlayerDied"/> to see how player respawn is handled
    private void TurnOnCollisions()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    /// <summary>
    /// Disables animation for blinking sprite
    /// </summary>
    private void DisableBlink()
    {
        animator.enabled = false;
    }

    /// <summary>
    /// Calculates fire rate based on current level of the game
    /// </summary>
    /// <param name="level">Current level of the game, to be used to determine the in-game fire rate</param>
    /// <see cref="GameManager"/> for how game level is determined
    public void SetNewFireRate(int level)
    {
        this.setFireRate = originalFireRate - ((level - 1) * 0.01f);
    }
}
