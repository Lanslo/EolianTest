using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverAI : MonoBehaviour
{
    public string log_file; // Name of the file to be saved in the streaming folder.
    DriverLogger.LocationList trail;

    int curr;
    float elapsed = 0f;
    float duration = 0f;

    void NextPath()
    {
        if (trail.locations.Count > curr)
        {
            transform.localPosition = new Vector3(trail.locations[curr].pos.x, transform.localPosition.y, trail.locations[curr].pos.y);
            Vector3 lookpt = transform.position + new Vector3(trail.locations[curr].heading.x, 0f, trail.locations[curr].heading.y);        
            transform.LookAt(lookpt, Vector3.up);
            duration = trail.locations[curr].elapsed - elapsed;
            curr++;
        } else
        {
            this.enabled = false;
        }
    }

    private void Start()
    {
        trail = DriverLogger.Load(log_file);
        if (trail == null)
            this.enabled = false;

        NextPath();
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        duration -= Time.deltaTime;
        if (duration <= 0f )
            NextPath();
    }
}
