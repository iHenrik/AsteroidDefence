using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const string GLOBE_ID = "Globe";
    private const float SPEED_RANGE_MIN = 1f;
    private const float SPEED_RANGE_MAX = 4f;
    private const string GAME_MANAGER_ID = "GameManager";
    
    private GameManager gameManager;

    [SerializeField]
    private AudioClip explosionClip;
    
    void Start()
    {
        gameManager = GameObject.Find(GAME_MANAGER_ID).GetComponent<GameManager>();

        //Randomize scale
        float scale = Random.Range(0.5f, 2f);
        transform.localScale = new Vector3(scale, scale, scale);
    }
    
    void Update()
    {
        if (gameManager.CurrentGameState != GameManager.GameState.Game)
        {
            GameObject.Destroy(this.gameObject);
        }

        float speed = Random.Range(SPEED_RANGE_MIN, SPEED_RANGE_MAX);

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, 0f, 0f), step);

        float rotationSpeed = Random.Range(50f, 700f);
        transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(explosionClip, transform.position);
    }
}
