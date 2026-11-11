using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloeManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform initial_orientation_camera;
    public Transform target_orientation_camera;

    public MicrobitSerialReader microbitflow;
    public GameObject connection_ok;
    public GameObject connection_not_ok;

    public GameObject Familiarisation_Screen;
    public GameObject Laptop_Connecting_Screen;




    public bool activity_started = false;

    public GameObject object_parent;
    public List<GameObject> objects = new List<GameObject>();

    public List<GameObject> activities = new List<GameObject>();

    public GameObject Original_Classroom;
    public GameObject Familiarisation_Room;
    public AudioSource connection_audio;
    void Start()
    {


        Logging.ScreenType = "Menu";
        Logging.ActivitySet = "";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
   

    

   public void set_activity_started()
    {
        activity_started = true;
    }

    public void stop_Audio()
    {
        if (connection_audio.isPlaying)
        {
            connection_audio.Stop();
        }
    }


    public void toggle_object(GameObject object_To_focus)
    {

        foreach (GameObject go in objects)
        {
            if (go != object_To_focus)
            {
                go.SetActive(false);
            }
        }
        object_To_focus.SetActive(true);
        microbitflow.calibrator = object_To_focus;
    }

    public void toggle_activity(GameObject activity_To_focus)
    {
        foreach (GameObject act in activities)
        {
            if (act != activity_To_focus)
            {
                act.SetActive(false);
            }
        }
        activity_To_focus.SetActive(true);
    }

    

    public void connect()
    {
        Logging.ScreenType = "Tutorial";
        Logging.ActivitySet = "";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connection Initiated";

        if (microbitflow.ConnectToMicrobit())
        {
            Logging.ScreenType = "Tutorial";
            Logging.ActivitySet = "";
            Logging.FocusTransform = null;
            Logging.ActivityCorrectEnd = false;
            Logging.Status = "Connected";

            connection_ok.SetActive(true);
            connection_not_ok.SetActive(false);
            connection_audio.Play();
            
            StartCoroutine(connection_progress());
            

        }
        else
        {
            connection_ok.SetActive(false);
            connection_not_ok.SetActive(true);

            Logging.ScreenType = "Tutorial";
            Logging.ActivitySet = "";
            Logging.FocusTransform = null;
            Logging.ActivityCorrectEnd = false;
            Logging.Status = "Could Not Connect";
        }

    }

    IEnumerator connection_progress()
    {
            

        yield return new WaitForSeconds(0.5f);
        initiate_sending_data();
        yield return new WaitForSeconds(0.5f);
        startmicrobitinitialrotationstate();
        yield return new WaitForSeconds(0.5f);
        Laptop_Connecting_Screen.SetActive(false);
        Familiarisation_Screen.SetActive(true);
        Original_Classroom.SetActive(false);
        Familiarisation_Room.SetActive(true);
        float t = 0f ;
        while(t <= 1)
        {
            Camera.main.transform.position = Vector3.Lerp(initial_orientation_camera.position, target_orientation_camera.position, t);
            Camera.main.transform.rotation = Quaternion.Lerp(initial_orientation_camera.rotation, target_orientation_camera.rotation, t);
            t = t + 0.05f;
            yield return new WaitForSeconds(0.05f);
            
        }

      
        object_parent.SetActive(true);

        Logging.ScreenType = "Demonstration";
        Logging.ActivitySet = "Familiarisation";
        Logging.FocusTransform = microbitflow.calibrator.transform;
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";

    }

    //the following is for getting the initial rotation after confirming i want to get data
    //i want to get data, i sent a confirm earlier, i am now parsing data you send, when satisfied, i'll ask for more data in another script

    public void startmicrobitinitialrotationstate()
    {
        microbitflow.getinitialstate();
    }


// i haven't got data yet, yes, i want to get first set of data;
    public void initiate_sending_data(){
        microbitflow.receive_data();
    }


    public void Quit()
    {
        Application.Quit();
    }
}
