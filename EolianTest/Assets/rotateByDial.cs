using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateByDial : MonoBehaviour
{
    public float incomingfloat = 0;

    public void rotate(float degree)
    {
        transform.localRotation = Quaternion.AngleAxis(degree, this.transform.up);
    }
}
