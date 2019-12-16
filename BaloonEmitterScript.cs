using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonEmitterScript : MonoBehaviour
{
    public float minDelay, maxDelay;
    public float size;
    public float speed;
    public GameObject balloonBlue;
    public GameObject balloonRed;
    public GameObject balloonPink;
    public GameObject balloonPurple;
    public GameObject balloonOrange;
    public GameObject balloonGreen;
    public GameObject balloonYellow;

    private float nextSpawn;
    private GameControllerScript gameController;
    private int intBalloon;
    private GameObject balloon;

    void Start()
    {
        gameController = 
            GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<GameControllerScript>(); // получаем доступ к скрипту gameController
    }

    void Update()
    {
        if (gameController.score >= 150)
        {
            speed = 2.5f;
            minDelay = 0.6f;
            maxDelay = 1.6f;
        } else if (gameController.score >= 300)
        {
            speed = 3f;
            minDelay = 0.5f;
            maxDelay = 1.5f;
        } else if (gameController.score >= 400)
        {
            speed = 3.5f;
            minDelay = 0.4f;
            maxDelay = 1.4f;
        } else if (gameController.score >= 500)
        {
            speed = 4f;
            minDelay = 0.3f;
            maxDelay = 1.3f;
        } else if (gameController.score >= 600)
        {
            speed = 4.5f;
            minDelay = 0.2f;
            maxDelay = 1.2f;
        } else if (gameController.score >= 650)
        {
            speed = 5f;
            minDelay = 0.1f;
            maxDelay = 1.1f;
        } else if (gameController.score >= 700)
        {
            speed = 5.5f;
            minDelay = 0.1f;
            maxDelay = 1.0f;
        } else if (gameController.score >= 750)
        {
            speed = 6f;
            minDelay = 0.1f;
            maxDelay = 0.9f;
        } else if (gameController.score >= 800)
        {
            speed = 6.5f;
            minDelay = 0.1f;
            maxDelay = 0.8f;
        } else if (gameController.score >= 850)
        {
            speed = 7f;
            minDelay = 0.1f;
            maxDelay = 0.7f;
        }

        if (!gameController.IsStarted())
        {
            return;
        }

        if (Time.time > nextSpawn)
        {
            float XPosition = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);

            Vector3 baloonPosition = new Vector3(XPosition, transform.position.y, transform.position.z);
            intBalloon = Random.Range(1, 8);
            switch (intBalloon)
            {
                case 1:
                    balloon = Instantiate(balloonBlue, baloonPosition, Quaternion.identity);
                    break;
                case 2:
                    balloon = Instantiate(balloonRed, baloonPosition, Quaternion.identity);
                    break;
                case 3:
                    balloon = Instantiate(balloonPink, baloonPosition, Quaternion.identity);
                    break;
                case 4:
                    balloon = Instantiate(balloonPurple, baloonPosition, Quaternion.identity);
                    break;
                case 5:
                    balloon = Instantiate(balloonOrange, baloonPosition, Quaternion.identity);
                    break;
                case 6:
                    balloon = Instantiate(balloonGreen, baloonPosition, Quaternion.identity);
                    break;
                default:
                    balloon = Instantiate(balloonYellow, baloonPosition, Quaternion.identity);
                    break;
            }

            balloon.GetComponent<Rigidbody>().velocity = Vector2.up * speed;
            nextSpawn = Time.time + Random.Range(minDelay, maxDelay);
        }
    }
}
