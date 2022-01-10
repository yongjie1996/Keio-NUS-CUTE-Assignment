using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager _gameManager;
    public float fireRate = 0.2f;
    public Bullet bulletPrefab;
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    public float invulnerabilityTimer = 3.0f;
    private Rigidbody2D _rigidbody;
    private bool _thrusting;
    private bool _reversing;
    private float _turnDirection;
    private float setFireRate;

    private void Awake() // get Rigidbody2D component and get value for fire rate of shooting
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        setFireRate = fireRate;
    }

    private void OnEnable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("RespawnedPlayer"); // this layer prevents collisions for a set amount of time to prevent player from dying immediately on respawn
        Invoke(nameof(TurnOnCollisions), invulnerabilityTimer); // switch back to original player layer after invulnerbility timer is up
    }

    private void Update() // check for player input for player movement
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

    private void FixedUpdate() // used for physics as making the related game physics attribute to regular Upate function would make it dependent on framerate
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

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation); // create new bullet prefab
        bullet.Project(this.transform.up); // shoot bullet
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid") // if collided to an asteroid
        {
            _rigidbody.velocity = Vector3.zero; // reset player velocity
            _rigidbody.angularVelocity = 0.0f; // reset torques applied to player

            this.gameObject.SetActive(false);

            _gameManager.PlayerDied(); // respawn the player
        }
    }

    private void TurnOnCollisions() // set player game object layer back to "Player"
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
