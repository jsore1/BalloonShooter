using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonScript : MonoBehaviour
{
    public void BalloonDestroy()
    {
        Destroy(gameObject, 0.2f);
    }
}
