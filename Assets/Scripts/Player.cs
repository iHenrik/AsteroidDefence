using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float LASER_INIT_Y_POSITION = 1.2f;
    private const string ASTEROID_TAG = "Asteroid";
    private const string GAME_MANAGER_ID = "GameManager";

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private GameObject Laser;

    [SerializeField]
    private GameObject explosionAnimation;

    [SerializeField]
    private GameObject barrelPoint;

    [SerializeField]
    private GameObject globe;

    private GameManager gameManager;
    private float fireRate = 0.25f;
    private float nextFire = 0f;
    
    private void Start()
    {
        gameManager = GameObject.Find(GAME_MANAGER_ID).GetComponent<GameManager>();
    }
    
    void Update()
    {
        Move();
        Shoot();
    }

    private void Shoot()
    {
        if (gameManager.CurrentGameState != GameManager.GameState.Game)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            Vector3 laserInitPosition = transform.position;
            laserInitPosition.y = LASER_INIT_Y_POSITION;
            Quaternion laserInitRotation = transform.rotation;

            Instantiate(Laser, barrelPoint.transform.position, laserInitRotation);
        }
    }

    //Moves player around the globe
    private void Move()
    {
        if (gameManager.CurrentGameState != GameManager.GameState.Game)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");

        transform.Rotate(horizontalInput * rotationSpeed * Vector3.back * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ASTEROID_TAG)
        {
            //Call GameOver();
            gameManager.SetGameOver();

            GameObject.Instantiate(explosionAnimation, collision.gameObject.transform.position, Quaternion.identity);
            GameObject.Instantiate(explosionAnimation, gameObject.transform.position, Quaternion.identity);

            //Destroy asteroid
            GameObject.Destroy(collision.gameObject);
            
            //Hide weapon
            gameObject.SetActive(false);

            //Hide globe
            globe.SetActive(false);
        }
    }
}
