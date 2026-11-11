using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject HomePanel;
    public GameObject ConnectionDemo;

    
    public GameObject Calibration;

    public GameObject ConnectorScreen;

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void start_game()
    {
        Logging.ScreenType = "Tutorial";
        Logging.ActivitySet = "";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        HomePanel.SetActive(false);
        load_connection_page();


    }

    public void load_connection_page()
    {
        Logging.ScreenType = "Tutorial";
        Logging.ActivitySet = "";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;

        ConnectionDemo.SetActive(true);
    }

    public void load_Calibration_page()
    {
        Calibration.SetActive(true);
    }

    public void load_actual_connection_page()
    {

        Logging.ScreenType = "Tutorial";
        Logging.ActivitySet = "";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;

        ConnectorScreen.SetActive(true);

        Logging.ScreenType = "Tutorial";
        Logging.ActivitySet = "";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
    }

   
    public void Next(GameObject currentobj)
    {
        if(currentobj == ConnectionDemo)
        {
            currentobj.SetActive(false);
            load_Calibration_page();
        }
        if (currentobj == Calibration)
        {
            currentobj.SetActive(false);
            load_actual_connection_page();
        }
    }

    public void Previous(GameObject currentobj)
    {
        if (currentobj == ConnectionDemo)
        {
            currentobj.SetActive(false);
            HomePanel.SetActive(true);
            Logging.ScreenType = "Tutorial";
            Logging.ActivitySet = "";
            Logging.FocusTransform = null;
            Logging.ActivityCorrectEnd = false;

        }
        if (currentobj == Calibration)
        {
            currentobj.SetActive(false);
            load_connection_page();
        }
    }
}
