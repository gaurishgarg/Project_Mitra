using System;
using System.IO.Ports;
using UnityEngine;

public class RotationReader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float pitchThreshold = 5f;
    float yawThreshold = 5f;
    float rollThreshold = 5f;


    public GameObject extremedetection_Screen;

    public MicrobitSerialReader microbitvalues;

    Vector3 initial_microbit_state;
    Vector3 current_orientation; //INITIAL ORIENTATION

    Vector3 prev_microbit_angles;
    Vector3 prev_orientation;

    Vector3 current_microbit_angles;

    Vector3 true_difference;
    Vector3 adjusted_Difference;

    Vector3 actuated_Difference;

    float limitx = 180f;
    float limity = 180f;
    float limitz = 180f;


    float limitxt = 140f;
    float limityt = 179.5f;
    float limitzt = 140f;





    bool extremeMode = false;



    public bool check_Dominance = true;
    enum DominantAxis
    {
        None, PitchAxis, RollAxis, Yawaxis
    }
    DominantAxis dominantAxis;
    
    void Start()
    {
        
    }

    public void custom_angular_threshold(float angle, float p, float r, float y, float lx, float ly, float lz)
    {
        pitchThreshold = p;
        yawThreshold = y;
        rollThreshold = r;

        limitx = lx;
        limity = ly;
        limitz = lz;
    }

    public void original_threshold()
    {
        pitchThreshold = 5f;
        yawThreshold = 5f;
        rollThreshold = 5f;
    }



    //try real time updation

    //void Update()
    //{
    //    if (microbitvalues.initial_angles_received && !microbitvalues.confimreceivedata)
    //    {
    //        initial_microbit_state = microbitvalues.receive_initial_angles();
    //        current_orientation = microbitvalues.calibrator.transform.localEulerAngles;

    //        Debug.Log("Initial Angles Received: " + initial_microbit_state);
    //        Debug.Log("Calibrator Rotation: " + current_orientation);

    //        prev_microbit_angles = initial_microbit_state;
    //        prev_orientation = current_orientation;

    //        microbitvalues.confimreceivedata = true;
    //        microbitvalues.enablerotationdataread();
    //    }

    //    if (microbitvalues.confimreceivedata)
    //    {
    //        current_microbit_angles = microbitvalues.receive_values();

    //        true_difference = current_microbit_angles - prev_microbit_angles;

    //        adjusted_Difference = true_difference;
    //        adjusted_Difference.x = -adjusted_Difference.x;
    //        adjusted_Difference.z = -adjusted_Difference.z;
    //        adjusted_Difference.y = 0f;


    //        Vector3 yaw_vector = current_microbit_angles - initial_microbit_state;
    //        yaw_vector.x = 0f;
    //        yaw_vector.z = 0f;


    //        actuated_Difference = adjusted_Difference;


    //        Vector3 current_transform = microbitvalues.calibrator.transform.localEulerAngles;


    //        Vector3 expected_transform = new Vector3((prev_orientation + actuated_Difference).x, (current_orientation + yaw_vector).y, (prev_orientation + actuated_Difference).z);



    //        Vector3 expected_difference = expected_transform - current_transform;

    //        expected_difference.x = Mathf.DeltaAngle(0f, expected_difference.x);
    //        expected_difference.y = Mathf.DeltaAngle(0f, expected_difference.y);
    //        expected_difference.z = Mathf.DeltaAngle(0f, expected_difference.z);

    //        ///now here we compare with thresholds but not expected difference but total difference;
    //        ///
    //        Vector3 net_Transform = expected_transform - current_orientation;


    //        net_Transform.x = Mathf.DeltaAngle(0f, net_Transform.x);
    //        net_Transform.y = Mathf.DeltaAngle(0f, net_Transform.y);
    //        net_Transform.z = Mathf.DeltaAngle(0f, net_Transform.z);


    //        if(Mathf.Abs(net_Transform.x) < pitchThreshold)
    //        {
    //            expected_difference.x = 0f;
    //        }

    //        if (Mathf.Abs(net_Transform.y) < yawThreshold)
    //        {
    //            expected_difference.y = 0f;
    //        }
    //        if (Mathf.Abs(net_Transform.z) < rollThreshold)
    //        {
    //            expected_difference.z = 0f;
    //        }





    //        microbitvalues.calibrator.transform.localEulerAngles = current_transform + expected_difference;





    //        prev_orientation = microbitvalues.calibrator.transform.localEulerAngles;
    //        prev_microbit_angles = current_microbit_angles;

    //    }
    //}








    //following works

    void Update()
    {
        if (microbitvalues.initial_angles_received && !microbitvalues.confimreceivedata)
        {
            initial_microbit_state = microbitvalues.receive_initial_angles();
            current_orientation = microbitvalues.calibrator.transform.eulerAngles;

            Debug.Log("Initial Angles Received: " + initial_microbit_state);
            Debug.Log("Calibrator Rotation: " + current_orientation);

            microbitvalues.confimreceivedata = true;
            microbitvalues.enablerotationdataread();
        }

        if (microbitvalues.confimreceivedata)
        {
            current_microbit_angles = microbitvalues.receive_values();

            true_difference = current_microbit_angles - initial_microbit_state;

            adjusted_Difference = true_difference;
            adjusted_Difference.x = -adjusted_Difference.x;
            adjusted_Difference.z = -adjusted_Difference.z;




            adjusted_Difference.x = Mathf.DeltaAngle(0f, adjusted_Difference.x);
            adjusted_Difference.y = Mathf.DeltaAngle(0f, adjusted_Difference.y);
            adjusted_Difference.z = Mathf.DeltaAngle(0f, adjusted_Difference.z);





            actuated_Difference = adjusted_Difference;


            //do filtering here if required

            //getting_thresholds
            bool satisfy_pitch_threshold = (Mathf.Abs(adjusted_Difference.x) >= pitchThreshold);

            bool satisfy_yaw_threshold = (Mathf.Abs(adjusted_Difference.y) >= yawThreshold);
            bool satisfy_roll_threshold = (Mathf.Abs(adjusted_Difference.z) >= rollThreshold);


            bool exceeds_limitx = (Mathf.Abs(adjusted_Difference.x) >= limitx);

            bool exceeds_limity = (Mathf.Abs(adjusted_Difference.y) >= limity);
            bool exceeds_limitz = (Mathf.Abs(adjusted_Difference.z) >= limitz);






            if (!satisfy_pitch_threshold || exceeds_limitx)
            {
                actuated_Difference.x = 0f;
          
            }
            if (!satisfy_yaw_threshold || exceeds_limity)
            {
                actuated_Difference.y = 0f;
            }
            if (!satisfy_roll_threshold || exceeds_limitz)
            {

                actuated_Difference.z = 0f;
         
            }

            //extreme detection

                    bool extremeDetected =
            Mathf.Abs(adjusted_Difference.x) >= limitxt ||
            Mathf.Abs(adjusted_Difference.y) >= limityt ||
            Mathf.Abs(adjusted_Difference.z) >= limitzt;


            // ENTER EXTREME MODE
            if (!extremeMode && extremeDetected)
            {
                extremeMode = true;

                extremedetection_Screen.SetActive(true);
            }

            

            ///
            microbitvalues.calibrator.transform.eulerAngles = current_orientation + actuated_Difference; ;
        }
    }

    public void handle_Extreme_mode()
    {
        bool safeToContinue =
       Mathf.Abs(adjusted_Difference.x) < limitxt &&
       Mathf.Abs(adjusted_Difference.y) < limityt &&
       Mathf.Abs(adjusted_Difference.z) < limitzt;

        if (safeToContinue)
        {
            extremeMode = false;
            extremedetection_Screen.SetActive(false);
        }
    }

  


    //Backup of dominance.


    //void Update()
    //{
    //    if (microbitvalues.initial_angles_received && !microbitvalues.confimreceivedata)
    //    {
    //        initial_microbit_state = microbitvalues.receive_initial_angles();
    //        current_orientation = microbitvalues.calibrator.transform.eulerAngles;

    //        Debug.Log("Initial Angles Received: " + initial_microbit_state);
    //        Debug.Log("Calibrator Rotation: " + current_orientation);

    //        microbitvalues.confimreceivedata = true;
    //        microbitvalues.enablerotationdataread();
    //    }

    //    if (microbitvalues.confimreceivedata)
    //    {
    //        current_microbit_angles = microbitvalues.receive_values();

    //        true_difference = current_microbit_angles - initial_microbit_state;

    //        adjusted_Difference = true_difference;
    //        adjusted_Difference.x = -adjusted_Difference.x;
    //        adjusted_Difference.z = -adjusted_Difference.z;


    //        //getting_thresholds
    //        bool satisfy_pitch_threshold = (Mathf.Abs(adjusted_Difference.x) > pitchThreshold) && (Mathf.Abs(adjusted_Difference.x) <= angularThreshold);
    //        ;
    //        bool satisfy_yaw_threshold = (Mathf.Abs(adjusted_Difference.y) > yawThreshold);
    //        bool satisfy_roll_threshold = (Mathf.Abs(adjusted_Difference.z) > rollThreshold) && (Mathf.Abs(adjusted_Difference.z) <= angularThreshold);

    //        // getting dominant axis
    //        float max_value = Mathf.Max(Mathf.Abs(adjusted_Difference.x), Mathf.Abs(adjusted_Difference.y), Mathf.Abs(adjusted_Difference.z));

    //        bool satisfy_pitch_max = (Mathf.Abs(adjusted_Difference.y) <= max_value) && (Mathf.Abs(adjusted_Difference.z) <= max_value);
    //        bool satisfy_yaw_max = (Mathf.Abs(adjusted_Difference.x) <= max_value) && (Mathf.Abs(adjusted_Difference.z) <= max_value);
    //        bool satisfy_roll_max = (Mathf.Abs(adjusted_Difference.x) <= max_value) && (Mathf.Abs(adjusted_Difference.y) <= max_value);

    //        actuated_Difference = Vector3.zero;

    //        //you are eligible for dominance only if you have cleared threshold

    //        if (check_Dominance)
    //        {
    //            if (satisfy_pitch_threshold && satisfy_pitch_max)
    //            {
    //                dominantAxis = DominantAxis.PitchAxis;
    //                actuated_Difference.x = adjusted_Difference.x;
    //            }
    //            else if (satisfy_yaw_threshold && satisfy_yaw_max)
    //            {
    //                dominantAxis = DominantAxis.Yawaxis;
    //                actuated_Difference.y = adjusted_Difference.y;
    //            }
    //            else if (satisfy_roll_threshold && satisfy_roll_max)
    //            {
    //                dominantAxis = DominantAxis.RollAxis;
    //                actuated_Difference.z = adjusted_Difference.z;

    //            }
    //        }


    //        microbitvalues.calibrator.transform.eulerAngles = current_orientation + actuated_Difference;
    //    }
    //}



}
