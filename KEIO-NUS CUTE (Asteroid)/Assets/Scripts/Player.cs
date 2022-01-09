using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float fireRate = 0.2f;
    public Bullet bulletPrefab;
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody2D _rigidbody;
    private bool _thrusting;
    private bool _reversing;
    private float _turnDirection;
    private float setFireRate;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        setFireRate = fireRate;
    }

    private void Update() // check for player input for player movement
    {
        if (fireRate > 0.0f)
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
            if (fireRate < 0.0f)
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
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed);
        }

        if (_reversing)
        {
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed * -1.0f);
        }

        if (_turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(_turnDirection * this.turnSpeed);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }
}
