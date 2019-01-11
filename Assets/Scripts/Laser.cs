using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private const float HORIZONTAL_BOUND = 10f;
    private const float VERTICAL_BOUND = 8f;
    private const string ASTEROID_TAG = "Asteroid";
    private const string GAME_MANAGER_ID = "GameManager";

    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private GameObject explosionAnimation;

    [SerializeField]
    private AudioClip laserBlastClip;

    [SerializeField]
    private AudioClip explosionClip;
    
    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameObject.Find(GAME_MANAGER_ID).GetComponent<GameManager>();
        
        AudioSource.PlayClipAtPoint(laserBlastClip, transform.position);
    }
    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        //check if laser is out of game area and destroy
        if (transform.position.x < (HORIZONTAL_BOUND * -1) || transform.position.x > HORIZONTAL_BOUND ||
             transform.position.y < (VERTICAL_BOUND * -1) || transform.position.y > VERTICAL_BOUND)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ASTEROID_TAG)
        {
            gameManager.AddScore();

            GameObject.Instantiate(explosionAnimation, collision.gameObject.transform.position, Quaternion.identity);
            
            //Destroy asteroid
            GameObject.Destroy(collision.gameObject);

            //Destroy laser
            GameObject.Destroy(gameObject);
        }
    }
}
