using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour {

    public float speed = 1f;
    public float angspeed = 1f;
    public float angacc = 30f;
    public float curr_ang_speed = 1f;

    // Update is called once per frame
    void Update ()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate( Vector3.up,  -curr_ang_speed * Time.deltaTime );

            if ( curr_ang_speed < angspeed)
                curr_ang_speed *= (1f+(angacc * Time.deltaTime));
        } else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate( Vector3.up, curr_ang_speed * Time.deltaTime );
            if (curr_ang_speed < angspeed)
                curr_ang_speed *= (1f+(angacc * Time.deltaTime));
        } else
        {
            curr_ang_speed = 1f;
        }

    }
}
