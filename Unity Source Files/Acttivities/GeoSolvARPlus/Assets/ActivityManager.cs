using TMPro;
using UnityEngine;

public class ActivityManager : MonoBehaviour
{

    public GameObject Activity_Home_Screen;
    public Transform activtiy1_Camera;

    public Transform Initial_Transform;

    public RotationReader rotreader;

    public FloeManager flow;

    public GameObject object_in_focus;
    public GameObject End_Screen;

    public GameObject classrom_1;
    public GameObject classroom_2;
    public GameObject board;

    public GameObject Activity_Canvas;

    public Transform SphereInitialPosition;
    public GameObject Sphere;

    public TextMeshProUGUI endscreentext;


    public GameObject emptyobject;

    public AudioSource Instruction;


    public void playInstruction()
    {
        if (!Instruction.isPlaying)
        {
            Instruction.Play();
        }
    } 

    public void InitiateActivity()
    {

       

        Sphere.transform.position = SphereInitialPosition.position;

        End_Screen.SetActive(false);
        Activity_Home_Screen.SetActive(false);
        Activity_Canvas.SetActive(true);

        classrom_1.SetActive(false);
        classroom_2.SetActive(false);
        board.SetActive(false);


        rotreader.custom_angular_threshold(90f, 1f, 1f, 1f, 150f, 0f, 150f);
        Camera.main.transform.position = activtiy1_Camera.transform.position;
        Camera.main.transform.rotation = activtiy1_Camera.transform.rotation;

        flow.toggle_object(object_in_focus);

        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity1A";
        Logging.FocusTransform = object_in_focus.transform;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";

    }
    


    void Start()
    {
        logactivtity1started();
        rotreader.original_threshold();
        Camera.main.transform.position = Initial_Transform.transform.position;
        Camera.main.transform.rotation = Initial_Transform.transform.rotation;
        Activity_Home_Screen.SetActive(true);
        classrom_1.SetActive(true);
        classroom_2.SetActive(false);
        board.SetActive(true);

    }

    void Update()
    {
       
    }

   

    public void EndActivtiy(string text_to_display)
    {
        if (Instruction.isPlaying)
        {
            Instruction.Stop();
        }

        Camera.main.transform.position = Initial_Transform.transform.position;
        Camera.main.transform.rotation = Initial_Transform.transform.rotation;

        classrom_1.SetActive(true);
        classroom_2.SetActive(false);
        board.SetActive(true);


        End_Screen.SetActive(true);
        Activity_Home_Screen.SetActive(false);

        Activity_Canvas.SetActive(false);
        endscreentext.text = text_to_display;
        rotreader.original_threshold();

        if (text_to_display.Contains("Nailed"))
        {
            Logging.ScreenType = "Activity_End";
            Logging.ActivitySet = "Activity1A";
            Logging.FocusTransform = null;
            Logging.ActivityCorrectEnd = true;
            Logging.Status = "Connected";
        }
        else
        {
            Logging.ScreenType = "Activity_End";
            Logging.ActivitySet = "Activity1A";
            Logging.FocusTransform = null;
            Logging.ActivityCorrectEnd = false;
            Logging.Status = "Connected";
        }
      

    }

    public void BacktoStart()
    {

        if (Instruction.isPlaying)
        {
            Instruction.Stop();
        }

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
        Logging.ActivitySet = "Activity1";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";
    }


    public void logactivtity1started()
    {
        Logging.ScreenType = "Activity_Home";
        Logging.ActivitySet = "Activity1";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";
    }




}