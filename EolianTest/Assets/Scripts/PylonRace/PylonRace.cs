using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class PylonRace : MonoBehaviour
{

    #region PROVIDED_DO_NOT_EDIT
    public enum PassingPolicy { PassOnLeft, PassOnRight, PassOver }
    public Transform car;
    public float max_dist = 3f;
    public float instructors_time = 0f;
    int prev_cone = 0;
    int next_cone = 1;

    bool started = false;
    float startTime = 0f;
    float endTime = 0f;

    [Serializable]
    public class Cone
    {
        public Transform cone;
        public PassingPolicy policy;
    }

    public List<Cone> cones;

    [Serializable]
    private class PylonTestData
    {
        public PylonTestData()
        {
            result = new bool[5];
        }
        public Vector3 car_pos;
        public Vector3 prev_pylon_pos;
        public Vector3 next_pylon_pos;
        public bool[] result;
    }

    [Serializable]
    private class PylonTestDataSet
    {
        public PylonTestDataSet()
        {
            data = new List<PylonTestData>();
        }
        public List<PylonTestData> data;
    }

    void CreateUnitTests()
    {
        PylonTestDataSet test_data = new PylonTestDataSet();

        for (int i = 0; i < 100; i++)
        {
            PylonTestData d = new PylonTestData();

            d.car_pos = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0f, UnityEngine.Random.Range(-10f, 10f));
            d.prev_pylon_pos = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0f, UnityEngine.Random.Range(-10f, 10f));
            d.next_pylon_pos = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0f, UnityEngine.Random.Range(-10f, 10f));

            PassingPolicy pass_side = ((i % 2) == 0) ? PassingPolicy.PassOnLeft : PassingPolicy.PassOnRight;
            d.result[0] = HasStarted(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos);
            d.result[1] = WithinPath(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos);
            d.result[2] = HasReachedGoal(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos);
            d.result[3] = PassedOnCorrectSide(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos, pass_side);
            d.result[4] = RoundingCone(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos);

            test_data.data.Add(d);
        }

        string jsonData = JsonUtility.ToJson(test_data);
        string filePath = Path.Combine(Application.streamingAssetsPath, "PylonRaceUnitTest");
        File.WriteAllText(filePath, jsonData);
    }

    void RunUnitTests()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "PylonRaceUnitTest");
        if (File.Exists(filePath))
        {
            // read existing json
            string jsonData = File.ReadAllText(filePath);
            PylonTestDataSet test_data = JsonUtility.FromJson<PylonTestDataSet>(jsonData);


            int[] passed = new int[5];
            for (int i = 0; i < 5; i++)
                passed[i] = 0;

            for (int i = 0; i < 100; i++)
            {
                PylonTestData d = test_data.data[i];

                PassingPolicy pass_side = ((i % 2) == 0) ? PassingPolicy.PassOnLeft : PassingPolicy.PassOnRight;
                if (HasStarted(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos) == d.result[0])
                    passed[0] += 1;
                if (WithinPath(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos) == d.result[1])
                    passed[1] += 1;
                if (HasReachedGoal(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos) == d.result[2])
                    passed[2] += 1;
                if (PassedOnCorrectSide(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos, pass_side) == d.result[3])
                    passed[3] += 1;
                if (RoundingCone(d.car_pos, d.prev_pylon_pos, d.next_pylon_pos) == d.result[4])
                    passed[4] += 1;
            }
            Debug.Log("HasStarted:          " + ((passed[0] == 100) ? "Pass" : "Fail"));
            Debug.Log("WithinPath:          " + ((passed[1] == 100) ? "Pass" : "Fail"));
            Debug.Log("HasReachedGoal:      " + ((passed[2] == 100) ? "Pass" : "Fail"));
            Debug.Log("PassedOnCorrectSide: " + ((passed[3] == 100) ? "Pass" : "Fail"));
            Debug.Log("RoundingCone:        " + ((passed[4] == 100) ? "Pass" : "Fail"));
        }
    }

    #endregion

    #region STUDENT_CODE

    // Return true when the car crosses the starting line
    // Implement using positive or negative sign of dot product (property 3)
    bool HasStarted(Vector3 car_pos, Vector3 start_pylon_pos, Vector3 first_pylon_pos)
    {

        // true if dot product value > 0 = start;

        Vector3 temp = first_pylon_pos - start_pylon_pos;
        Vector3 temp2 = car_pos - start_pylon_pos;

        if (Vector3.Dot(temp2, temp) > 0)
        {
            return true;
        }

        return false;
    }

    // Return true if still within "max_dist" (see variable above) from the current line between prev and next cone.
    // Implement using dot projection (property 6)
    // Do not use the Vector3.Project function.
    bool WithinPath(Vector3 car_pos, Vector3 prev_pylon_pos, Vector3 next_pylon_pos)
    {

        float temp = max_dist * max_dist;

        Vector3 tempvec = next_pylon_pos - prev_pylon_pos;

        float len = tempvec.magnitude;

        tempvec = tempvec / len;

        Vector3 tempvec2 = car_pos - prev_pylon_pos;

        tempvec = tempvec2 - tempvec;

        float tempproj = tempvec.sqrMagnitude;

        if (tempproj < temp)
        {
            return true;
        }

        return false;
    }

    // Return true if car has traveled past the next_pylon_pos.
    // Implement using positive or negative sign of dot product (property 3)
    // Note: Same math approach as HasStarted.
    bool HasReachedGoal(Vector3 car_pos, Vector3 prev_pylon_pos, Vector3 next_pylon_pos)
    {
        // true if dot product value > 0 = start;

        Vector3 temp = next_pylon_pos - prev_pylon_pos;
        Vector3 temp2 = next_pylon_pos - car_pos;

        if (Vector3.Dot(temp2, temp) < 0)
        {
            return true;
        }

        return false;
    }

    // Return true if the car passed next_pylon_pos on the correct side.
    // Implement using cross product (property 3)
    bool PassedOnCorrectSide(Vector3 car_pos, Vector3 prev_pylon_pos, Vector3 next_pylon_pos, PassingPolicy which_side)

    {
        // true if cross product sign equals the same as passingpolicy which_side;

        Vector3 temp = next_pylon_pos - prev_pylon_pos;
        Vector3 temp2 = next_pylon_pos - car_pos;

        temp = Vector3.Cross(temp2, temp);

        if (temp.y < 0 && which_side == PassingPolicy.PassOnLeft)
        {
            return true;
        }
        if (temp.y > 0 && which_side == PassingPolicy.PassOnRight)
        {
            return true;
        }

        return false;
    }

    // Same as the HasStarted method (above) except you also must test to make certain that the car stays within max_dist 
    // radius of next_pylon_pos while rounding it and before starting toward next_pylon_pos.
    // Return true while the car is currently rounding the cone correctly.
    // Implement using sign of dot product (property 3) and dot of vector with itself (property 2)
    bool RoundingCone(Vector3 car_pos, Vector3 prev_pylon_pos, Vector3 next_pylon_pos)
    {

        //int sign = 0;

        //Vector3 temp = next_pylon_pos - prev_pylon_pos;
        //Vector3 temp2 = car_pos - prev_pylon_pos;

        //if (Vector3.Dot(temp2, temp) > 0)
        //{
        //    sign = 1;
        //}

        //temp = car_pos - prev_pylon_pos;

        //float var = Vector3.Dot(temp, temp);

        //if (var < (max_dist * max_dist) && sign > 0)
        //{
        //    return true;
        //}

        //if (var < (max_dist * max_dist))
        //{
        //    return true;
        //}

        return false;

        // true if dot is positive and distance is good.

        return false;
    }

    #endregion

    #region PROVIDED_DO_NOT_EDIT

    private void Start()
    {
        //CreateUnitTests();

        RunUnitTests();
    }

    void FinishRun(bool success)
    {
        if (success)
            Debug.Log("Good: Time:" + (endTime - startTime));
        else
            Debug.Log("Aborted:  Time:" + (endTime - startTime));

        float error = Mathf.Abs((endTime - startTime) - instructors_time);
        if (error < 0.01f)
            error = 0f;
        if (error < 0.5f)
            Debug.Log("Instructors time:" + instructors_time + " Error:" + error + " Passed!");
        else
            Debug.Log("Instructors time:" + instructors_time + " Error:" + error + " Failed!");

        DriverLogger logger = car.GetComponent<DriverLogger>();
        if (logger != null && logger.enabled)
        {
            logger.Save();
            logger.enabled = false;
        }

        Driver driver = car.GetComponent<Driver>();
        if (driver != null)
            driver.enabled = false;

        DriverAI driverai = car.GetComponent<DriverAI>();
        if (driverai != null)
            driverai.enabled = false;

        this.enabled = false;
    }

    bool rounding_cone = false;

    // Log data for each car
    void Update()
    {

        if (!started)
        {
            started = HasStarted(car.position, cones[prev_cone].cone.position, cones[next_cone].cone.position);
            if (started)
                startTime = Time.fixedTime;
            return;
        }

        if (rounding_cone)
        {
            if (!RoundingCone(car.position, cones[prev_cone].cone.position, cones[next_cone].cone.position))
            {
                if (!WithinPath(car.position, cones[prev_cone].cone.position, cones[next_cone].cone.position))
                {
                    endTime = Time.fixedTime;
                    FinishRun(false);
                }

                rounding_cone = false;
            }
        }

        if (!WithinPath(car.position, cones[prev_cone].cone.position, cones[next_cone].cone.position))
        {
            endTime = Time.fixedTime;
            FinishRun(false);
            return;
        }

        // Enroute
        if (!HasReachedGoal(car.position, cones[prev_cone].cone.position, cones[next_cone].cone.position))
            return;

        if (!PassedOnCorrectSide(car.position, cones[prev_cone].cone.position, cones[next_cone].cone.position, cones[next_cone].policy))
        {
            endTime = Time.fixedTime;
            FinishRun(false);
            return;
        }

        rounding_cone = true;
        prev_cone = next_cone;
        next_cone++;

        if (next_cone >= cones.Count)
        {
            endTime = Time.fixedTime;
            FinishRun(true);
        }
    }
    #endregion
}