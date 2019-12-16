using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DectroyBoundaryScript : MonoBehaviour
{
    public GameObject percent0;
    public GameObject percent50;
    public GameObject percent100;

    private int baloonMiss;
    private GameControllerScript gameController;

    void Start()
    {
        baloonMiss = 0; // инициализируем нулем переменную для подсчета пропущенных шаров
        gameController = 
            GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<GameControllerScript>(); // получаем доступ к скрипту gameController'а
    }

    // Проверяем вышел ли шар за границу коллайдера
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject); // уничтожаем столкнувшийся с коллайдером шар
        baloonMiss += 1; // добавляем 1 к счетчику пропущенных шаров

        if (baloonMiss == 1)
        {
            percent100.SetActive(false);
            percent50.SetActive(true);
        } else if (baloonMiss == 2)
        {
            percent50.SetActive(false);
            percent0.SetActive(true);
        }

        // Если пропущенных шаров больше или равно 3
        if (baloonMiss >= 3)
        {
            baloonMiss = 0; // обнуляем счетсик пропущенных шаров
            percent0.SetActive(false);
            percent100.SetActive(true);
            gameController.stopGame(); // останавливаем игру
        }
    }
}
