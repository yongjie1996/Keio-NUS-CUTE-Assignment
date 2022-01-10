using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 500.0f;
    public float maxLifetime = 10.0f;
    private Rigidbody2D _rigidbody;
    
    private void Awake ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction) // shoots a bullet forward
    {
        _rigidbody.AddForce(direction * this.speed);

        Destroy(this.gameObject, this.maxLifetime); // destroy this game object after set time so it doesn't linger in the game
    }

    private void OnCollisionEnter2D(Collision2D collision) // destroy bullet after it collides with asteroid or boundary
    {
        Destroy(this.gameObject);
    }
}
