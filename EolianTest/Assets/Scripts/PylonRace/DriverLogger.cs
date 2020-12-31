using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DriverLogger : MonoBehaviour {

    public string log_file; // Name of the file to be saved in the streaming folder.
    public float ds_sq;        // SqDistance you must travel before dropping another point.

    [Serializable]
    public class Location
    {
        public Vector2 pos;
        public Vector2 heading;
        public float elapsed;
    }

    [System.Serializable]
    public class LocationList
    {
        public List<Location> locations = new List<Location>();
    }

    LocationList trail = new LocationList();
    float elapsed = 0f;

    public void Save()
    {
        // Save JSON and finish.
        string filePath = Path.Combine(Application.streamingAssetsPath, log_file);
        Debug.Log("Saving:" + filePath);

        string jsonData = JsonUtility.ToJson(trail);
        File.WriteAllText(filePath, jsonData);
    }

    static public LocationList Load( string log_file )
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, log_file);
        Debug.Log("Loading JSON Data from:" + filePath);

        if (File.Exists(filePath))
        {
            // read existing json
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<LocationList>(jsonData);
        }
        else
        {
            // create a new Gesture data struction and json file
            Debug.LogWarning("No LocationList found");
            return null;
        }
    }

    void DropPoint()
    {
        Location loc = new Location();
        loc.pos = new Vector3( transform.localPosition.x, transform.localPosition.z);
        loc.heading = new Vector2(transform.forward.x, transform.forward.z);
        loc.elapsed = elapsed;
        trail.locations.Add(loc);
        Debug.Log("Dropping:" + loc);
    }

    // Use this for initialization
    Vector3 last_pos;
    void Start () {
        last_pos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        elapsed += Time.deltaTime;
        Vector3 ds = transform.localPosition - last_pos;
        if (Vector3.Dot(ds, ds) > ds_sq)
        {
            last_pos = transform.localPosition;
            DropPoint();
        }

        if ( Input.GetKeyDown(KeyCode.S) )
        {
            Save();
            this.enabled = false;
        }
	}

}
