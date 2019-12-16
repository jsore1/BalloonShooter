using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    public float smooth = 0.4f;
    public float sensitivity = 6;
    public float padding;
    public float speed;
    public GameObject baloonPop;

    private Vector3 currentAcceleration, initialAcceleration;
    private Vector3 min, max;
    private Rigidbody aim;
    private GameControllerScript gameController;
    private float nextShot;
    public Matrix4x4 calibrationMatrix;
    private Vector3 wantedDeadZone = Vector3.zero;
    private Vector3 _InputDir;

    public Quaternion calibrationQuaternion;

    void Start()
    {
        //currentAcceleration = Vector3.zero;
        min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        aim = GetComponent<Rigidbody>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
    }

    void Update()
    {
        if (!gameController.IsStarted())
        {
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        aim.velocity = new Vector3(moveHorizontal, moveVertical, 0) * speed;

        //_InputDir = getAccelerometer(Input.acceleration);
        _InputDir = FixAcceleration(Input.acceleration);

        //currentAcceleration = Vector3.Lerp(currentAcceleration, _InputDir, Time.deltaTime / smooth);
        currentAcceleration = Vector3.Lerp(currentAcceleration, _InputDir, Time.deltaTime / smooth);

        aim.transform.Translate(currentAcceleration * sensitivity);

        float xPosition = Mathf.Clamp(transform.position.x, min.x + padding, max.x - padding);
        float yPosition = Mathf.Clamp(transform.position.y, min.y + padding, max.y - padding);
        float zPosition = aim.position.z;

        aim.transform.position = new Vector3(xPosition, yPosition, zPosition);

        if (Time.time > nextShot && Input.GetButtonDown("Fire1"))
        {
            
            RaycastHit hit;
            Vector3 fwd = aim.transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(aim.transform.position, fwd, out hit, 100.0f))
            {
                
                if (hit.collider.GetComponent<Rigidbody>() != null)
                {
                    
                    nextShot = Time.time + 0.2f;
                    hit.collider.gameObject.GetComponent<Animator>().Play("Pop");
                    GameObject audioObjPop = Instantiate(baloonPop, aim.transform.position, Quaternion.identity);
                    Destroy(audioObjPop, 1);
                    gameController.IncreaseScore(10);
                }
            }
        }
    }

    //Метод для калибровки
    //public void CalibrateAccelerometer()
    //{
    //    wantedDeadZone = Input.acceleration;
    //    Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, 0f), wantedDeadZone);
    //    //create identity matrix ... rotate our matrix to match up with down vec
    //    Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));
    //    //get the inverse of the matrix
    //    calibrationMatrix = matrix.inverse;

    //}

    ////Метод для получения откалибраванного ввода
    //public Vector3 GetAccelerometer(Vector3 accelerator)
    //{
    //    Vector3 accel = calibrationMatrix.MultiplyVector(accelerator);
    //    return accel;
    //}

    public void CalibrateAccelerometer()
    {
        // получаем данные с датчика акселерометра и записываем
        // в переменную accelerationSnapshot
        Vector3 accelerationSnapshot = Input.acceleration;

        // Поворачиваем из положения лицом вверх в положение полученное от акселерометра
        // функция возвращает координаты положения телефона в пространстве типа кватернион
        // другими словами текущее положение телефона
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);

        // Инвентируем значение осей
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);

    }

    public Vector3 FixAcceleration(Vector3 acceleration)
    {
        // умножаем стартовое положение телефона на текущее
        // получаем текущее положение с учетом калибровки
        return calibrationQuaternion * acceleration;
    }
}
