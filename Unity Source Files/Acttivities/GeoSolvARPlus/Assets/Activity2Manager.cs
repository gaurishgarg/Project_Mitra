using System.Collections;
using TMPro;
using UnityEngine;

public class Activity2Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Activity_Home_Screen;
    public Transform activtiy2_Camera;

    public Transform Initial_Transform;

    public RotationReader rotreader;

    public FloeManager flow;

    public GameObject house;

    public GameObject aeroplane;
    public GameObject End_Screen;

    public GameObject classrom_1;
    public GameObject classroom_2;
    public GameObject board;

    public GameObject Activity_Canvas;

    public GameObject AeroPlane_Canvas;

    public GameObject Cubes_Canvas;

    public Transform TargetAnglesHouse;

    public Transform TargetAngleAeroplane;

    public Transform CubesReference;

    public GameObject Cubes;

    public TextMeshProUGUI endscreentext;

    float tolerance_x;
    float tolerance_y;
    float tolerance_z;

    public AudioSource GreatJob;
    public AudioSource TryAgain;

    public AudioSource Instruction;

    public GameObject emptyobject;
    string[] correct_count = new string[3];


    public void PlayInstruction()
    {
        stop_all_Audios();
        Instruction.Play();
    }

    public void InitiateActivity_House()
    {


        End_Screen.SetActive(false);
        Activity_Home_Screen.SetActive(false);
        Activity_Canvas.SetActive(true);
        AeroPlane_Canvas.SetActive(false);
        Cubes_Canvas.SetActive(false);

        classrom_1.SetActive(false);
        classroom_2.SetActive(false);
        board.SetActive(false);


        rotreader.custom_angular_threshold(90f, 1f, 1f, 1f, 150f, 150f, 150f);
        Camera.main.transform.position = activtiy2_Camera.transform.position;
        Camera.main.transform.rotation = activtiy2_Camera.transform.rotation;

        flow.toggle_object(house);



        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity2A";
        Logging.FocusTransform = house.transform;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";




    }


    public void ValidateHouse()
    {
        stop_all_Audios();

        tolerance_x = 7.5f;
        tolerance_y = 15f;
        tolerance_z = 7.5f;


        float x_Diff = Mathf.DeltaAngle(house.transform.eulerAngles.x, TargetAnglesHouse.eulerAngles.x);
        float y_Diff = Mathf.DeltaAngle(house.transform.eulerAngles.y, TargetAnglesHouse.eulerAngles.y);
        float z_Diff = Mathf.DeltaAngle(house.transform.eulerAngles.z, TargetAnglesHouse.eulerAngles.z);

        if(Mathf.Abs(x_Diff) < tolerance_x && Mathf.Abs(y_Diff) < tolerance_y && Mathf.Abs(z_Diff) < tolerance_z)
        {
            Debug.Log("Success");
            stop_all_Audios();
            GreatJob.Play();

            correct_count[0] = "1";

            Logging.ScreenType = "Activity";
            Logging.ActivitySet = "Activity2A";
            Logging.FocusTransform = house.transform;
            Logging.ActivityCorrectEnd = true;
            Logging.Status = "Connected";
            Logging.AnswerMarked = "Correct_Rotation";

            InitiateActivity_Aeroplane();



        }
        else
        {

            TryAgain.Play();
        }
        
    }


    void stop_all_Audios()
    {

        if (Instruction.isPlaying)
        {
            Instruction.Stop();
        }
        if (TryAgain.isPlaying)
        {
            TryAgain.Stop();
        }
        if (GreatJob.isPlaying)
        {
            GreatJob.Stop();
        }
    }

    public void ValidateAeroplane()
    {

        tolerance_x = 25f;
        tolerance_y = 30f;
        tolerance_z = 25f;


        float x_Diff = Mathf.DeltaAngle(aeroplane.transform.eulerAngles.x, TargetAngleAeroplane.eulerAngles.x);
        float y_Diff = Mathf.DeltaAngle(aeroplane.transform.eulerAngles.y, TargetAngleAeroplane.eulerAngles.y);
        float z_Diff = Mathf.DeltaAngle(aeroplane.transform.eulerAngles.z, TargetAngleAeroplane.eulerAngles.z);

        if (Mathf.Abs(x_Diff) <= tolerance_x && Mathf.Abs(y_Diff) <= tolerance_y && Mathf.Abs(z_Diff) <= tolerance_z)
        {
            Debug.Log("Success");
            stop_all_Audios();
            GreatJob.Play();


            correct_count[1] =  "1";

            Logging.ScreenType = "Activity";
            Logging.ActivitySet = "Activity2B";
            Logging.FocusTransform = aeroplane.transform;
            Logging.ActivityCorrectEnd = true;
            Logging.Status = "Connected";
            Logging.AnswerMarked = "Correct_Rotation";

            InitiateActivity_Cubese();
        }
        else
        {
            TryAgain.Play();
        }

    }


    public void InitiateActivity_Aeroplane()
    {

        if (Instruction.isPlaying)
        {
            Instruction.Stop();
        }


        End_Screen.SetActive(false);
        Activity_Home_Screen.SetActive(false);


        Activity_Canvas.SetActive(false);
        AeroPlane_Canvas.SetActive(true);
        Cubes_Canvas.SetActive(false);

        classrom_1.SetActive(false);
        classroom_2.SetActive(false);
        board.SetActive(false);


        rotreader.custom_angular_threshold(90f, 1f, 1f, 1f, 150f, 150f, 150f);
        Camera.main.transform.position = activtiy2_Camera.transform.position;
        Camera.main.transform.rotation = activtiy2_Camera.transform.rotation;

        flow.toggle_object(aeroplane);


        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity2B";
        Logging.FocusTransform = aeroplane.transform;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";

    }


    public void InitiateActivity_Cubese()
    {

        if (Instruction.isPlaying)
        {
            Instruction.Stop();
        }


        End_Screen.SetActive(false);
        Activity_Home_Screen.SetActive(false);



        Activity_Canvas.SetActive(false);
        AeroPlane_Canvas.SetActive(false);
        Cubes_Canvas.SetActive(true);


        classrom_1.SetActive(false);
        classroom_2.SetActive(false);
        board.SetActive(false);


        rotreader.custom_angular_threshold(90f, 1f, 1f, 1f, 150f, 150f, 150f);
        Camera.main.transform.position = activtiy2_Camera.transform.position;
        Camera.main.transform.rotation = activtiy2_Camera.transform.rotation;

        flow.toggle_object(Cubes);

        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity2C";
        Logging.FocusTransform = Cubes.transform;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";


    }




    void Start()
    {

        rotreader.original_threshold();
        Camera.main.transform.position = Initial_Transform.transform.position;
        Camera.main.transform.rotation = Initial_Transform.transform.rotation;
        Activity_Home_Screen.SetActive(true);
        classrom_1.SetActive(true);
        classroom_2.SetActive(false);
        board.SetActive(true);




        Logging.ScreenType = "Activity_Home";
        Logging.ActivitySet = "Activity2";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";

    }

    void Update()
    {

    }



    public void ValidateCubes()
    {

        tolerance_x = 15f;
        tolerance_y = 30f;
        tolerance_z = 15f;


        float x_Diff = Mathf.DeltaAngle(Cubes.transform.eulerAngles.x, CubesReference.eulerAngles.x);
        float y_Diff = Mathf.DeltaAngle(Cubes.transform.eulerAngles.y, CubesReference.eulerAngles.y);
        float z_Diff = Mathf.DeltaAngle(Cubes.transform.eulerAngles.z, CubesReference.eulerAngles.z);

        stop_all_Audios();

        if (Mathf.Abs(x_Diff) < tolerance_x && Mathf.Abs(y_Diff) < tolerance_y && Mathf.Abs(z_Diff) < tolerance_z)
        {
            correct_count[2] = "1";

            Logging.ScreenType = "Activity";
            Logging.ActivitySet = "Activity2C";
            Logging.FocusTransform = Cubes.transform;
            Logging.ActivityCorrectEnd = true;
            Logging.Status = "Connected";
            Logging.AnswerMarked = "Correct_Rotation";



            Debug.Log("Success");
            GreatJob.Play();
            EndActivtiy("You Did It!");


        }
        else
        {
            TryAgain.Play();
        }

    }

    public void EndActivtiy(string text_to_display)
    {

        string answer_string = "";
        foreach (string str in correct_count)
        {
            answer_string += str;
        }

        if (answer_string.Equals("111"))
        {

            Logging.ScreenType = "Activity_End";
            Logging.ActivitySet = "Activity2";
            Logging.FocusTransform = null;
            Logging.ActivityCorrectEnd = true;
            Logging.Status = "Connected";
            text_to_display = "You Nailed it!";
        }
        else
        {
            Logging.ScreenType = "Activity_End";
            Logging.ActivitySet = "Activity2";
            Logging.FocusTransform = null;
            Logging.ActivityCorrectEnd = false;
            Logging.Status = "Connected";
            text_to_display = "You Missed Some!";
        }


        Camera.main.transform.position = Initial_Transform.transform.position;
        Camera.main.transform.rotation = Initial_Transform.transform.rotation;


        classrom_1.SetActive(true);
        classroom_2.SetActive(false);
        board.SetActive(true);


        End_Screen.SetActive(true);
        Activity_Home_Screen.SetActive(false);

        Activity_Canvas.SetActive(false);
        AeroPlane_Canvas.SetActive(false);
        Cubes_Canvas.SetActive(false);

        endscreentext.text = text_to_display;
        rotreader.original_threshold();


      




      


    }

    public void BacktoStart()
    {
        stop_all_Audios();
        
        Camera.main.transform.position = Initial_Transform.transform.position;
        Camera.main.transform.rotation = Initial_Transform.transform.rotation;

        flow.toggle_object(emptyobject);

        classrom_1.SetActive(true);
        classroom_2.SetActive(false);
        board.SetActive(true);

        End_Screen.SetActive(false);
        Activity_Home_Screen.SetActive(true);
        Activity_Canvas.SetActive(false);

        Logging.ScreenType = "Activity_Home";
        Logging.ActivitySet = "Activity2";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = true;
        Logging.Status = "Connected";
    }


  

}
