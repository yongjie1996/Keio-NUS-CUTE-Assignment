using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 50.0f;
    public float maxLifetime = 30.0f;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f); // random sprite rotation for spawned asteroid
        this.transform.localScale = Vector3.one * this.size; // random sprite scale

        _rigidbody.mass = this.size * 2.0f;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);

        Destroy(this.gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet") // if collided with bullet
        {
            if ((this.size * 0.5) >= this.minSize ) // create 2 smaller asteroids if the asteroid is bigger than minSize if split
            {
                CreateSplit();
                CreateSplit();
            }

            FindObjectOfType<GameManager>().AsteroidDestroyed(this); // play particles and add score
            Destroy(this.gameObject); // destroy asteroid            
        }       
    }

    private void CreateSplit() // create asteroid half of the size of the current one
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }

}
