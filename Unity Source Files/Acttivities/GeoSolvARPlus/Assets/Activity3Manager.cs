using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Activity3Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public GameObject Activity_Home_Screen;
    public Transform activtiy2_Camera;

    public Transform Initial_Transform;

    public RotationReader rotreader;

    public FloeManager flow;


    public GameObject End_Screen;

    public GameObject classrom_1;
    public GameObject classroom_2;
    public GameObject board;

    public GameObject Activtiy1;

    public GameObject Activity_Canvas_Q1;

    public GameObject Studio_Panel_A1_Q1;

    public GameObject Explain_A1_Q1;
    public TextMeshProUGUI endscreentext;

  
    public AudioSource GreatJob;
    public AudioSource TryAgain;

    public GameObject House;

    public GameObject Car;
    public GameObject Studio;

    public GameObject Cubes;

    public GameObject emptyobject;

    public TMP_Dropdown Q1Dropdown;

    public TMP_Dropdown Q2Dropdown;

    public TMP_Dropdown Q3Dropdown;



    public GameObject Activtiy2;

    public GameObject Activity_Canvas_Q2;

    public GameObject Studio_Panel_A2_Q2;

    public GameObject Explain_A2_Q2;



    public GameObject Activtiy3;

    public GameObject Activity_Canvas_Q3;

    public GameObject Studio_Panel_A3_Q3;

    public GameObject Explain_A3_Q3;

    string[] answers = new string[3];


    public AudioSource Instruction;


    public AudioSource Observe;


    public TextMeshProUGUI studio_Text;
    public void Initiate()
    {

        if (Observe.isPlaying)
        {
            Observe.Stop();
        }

        End_Screen.SetActive(false);
        Activity_Home_Screen.SetActive(false);
        Activtiy1.SetActive(true);
        Activity_Canvas_Q1.SetActive(true);
        Studio_Panel_A1_Q1.SetActive(false);
        Explain_A1_Q1.SetActive(false);



        classrom_1.SetActive(false);
        classroom_2.SetActive(false);
        board.SetActive(false);


        Camera.main.transform.position = activtiy2_Camera.transform.position;
        Camera.main.transform.rotation = activtiy2_Camera.transform.rotation;

        flow.toggle_object(emptyobject);


        Activtiy2.SetActive(false);
        Activity_Canvas_Q2.SetActive(false);
        Studio_Panel_A2_Q2.SetActive(false);
        Explain_A2_Q2.SetActive(false);
        Studio.SetActive(false);


        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity3A_P";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";


    }


    public void PlayInstruction()
    {
        if (Instruction.isPlaying)
        {
            Instruction.Stop();
        }
        if (Observe.isPlaying)
        {
            Observe.Stop();
        }

        Instruction.Play();
    }

    public void ValidateQ1()
    {
        if (!Observe.isPlaying)
        {
            Observe.Play();
        }


        if (Instruction.isPlaying)
        {
            Instruction.Stop();
        }

        if (Q1Dropdown.value.ToString() == "0")
        {
            Debug.Log("None Selected");
            answers[0] = "";
            Logging.AnswerMarked = "";
            studio_Text.text = "You didn't answer!";
            studio_Text.color = Color.black;

        }
        else if(Q1Dropdown.value.ToString() == "1")
        {
            Debug.Log("A Selected");
            answers[0] = "A";
            Logging.AnswerMarked = "A";

            studio_Text.text = "Correct!";
            studio_Text.color = Color.green;

        }
        else if (Q1Dropdown.value.ToString() == "2")
        {
            Debug.Log("B Selected");
            answers[0] = "B";
            Logging.AnswerMarked = "B";

            studio_Text.text = "Incorrect!";
            studio_Text.color = Color.red;

        }
        else if (Q1Dropdown.value.ToString() == "3")
        {
            Debug.Log("C Selected");
            answers[0] = "C";
            Logging.AnswerMarked = "C";

            studio_Text.text = "Incorrect!";
            studio_Text.color = Color.red;

        }
        else if (Q1Dropdown.value.ToString() == "4")
        {
            Debug.Log("D Selected");
            Logging.AnswerMarked = "D";
            answers[0] = "D";

            studio_Text.text = "Incorrect!";
            studio_Text.color = Color.red;

        }
        Activity_Canvas_Q1.SetActive(false);
        Studio_Panel_A1_Q1.SetActive(true);
        Explain_A1_Q1.SetActive(false);
        rotreader.custom_angular_threshold(90f, 15f, 15f, 15f, 0f, 179f, 0f);
        flow.toggle_object(House);
        Studio.SetActive(true);

        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity3A_O";
        Logging.FocusTransform = House.transform;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";


    }

    public void move_A1_to_explain()
    {
        Activtiy1.SetActive(true);
        Activity_Canvas_Q1.SetActive(false);
        Studio_Panel_A1_Q1.SetActive(false);
        Explain_A1_Q1.SetActive(true);
        Studio.SetActive(false);
        flow.toggle_object(emptyobject);



        Activtiy2.SetActive(false);
        Activity_Canvas_Q2.SetActive(false);
        Studio_Panel_A2_Q2.SetActive(false);
        Explain_A2_Q2.SetActive(false);
        Studio.SetActive(false);

        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity3A_E";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";

    }

    public void startactivtiy2()
    {
        Activtiy1.SetActive(false);
        Activity_Canvas_Q1.SetActive(false);
        Studio_Panel_A1_Q1.SetActive(false);
        Explain_A1_Q1.SetActive(false);
        Studio.SetActive(false);

        Activtiy2.SetActive(true);
        Activity_Canvas_Q2.SetActive(true);
        Studio_Panel_A2_Q2.SetActive(false);
        Explain_A2_Q2.SetActive(false);
        Studio.SetActive(false);

        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity3B_P";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";


    }

    public void ValidateQ2()
    {
        if (Q2Dropdown.value.ToString() == "0")
        {
            Debug.Log("None Selected");
            answers[1] = "";
            Logging.AnswerMarked = "";

            studio_Text.text = "You didn't answer!";
            studio_Text.color = Color.black;
        }
        else if (Q2Dropdown.value.ToString() == "1")
        {
            Debug.Log("A Selected");
            answers[1] = "A";
            Logging.AnswerMarked = "A";

            studio_Text.text = "Incorrect!";
            studio_Text.color = Color.red;
        }
        else if (Q2Dropdown.value.ToString() == "2")
        {
            Debug.Log("B Selected");
            answers[1] = "B";
            Logging.AnswerMarked = "B";

            studio_Text.text = "Correct!";
            studio_Text.color = Color.green;

        }
        else if (Q2Dropdown.value.ToString() == "3")
        {
            Debug.Log("C Selected");
            answers[1] = "C";
            Logging.AnswerMarked = "C";
            studio_Text.text = "Incorrect!";
            studio_Text.color = Color.red;
        }
        else if (Q2Dropdown.value.ToString() == "4")
        {
            Debug.Log("D Selected");
            answers[1] = "D";
            Logging.AnswerMarked = "D";
            studio_Text.text = "Incorrect!";
            studio_Text.color = Color.red;
        }
        Activity_Canvas_Q2.SetActive(false);
        Studio_Panel_A2_Q2.SetActive(true);
        Explain_A2_Q2.SetActive(false);
        rotreader.custom_angular_threshold(90f, 15f, 15f, 15f, 0f, 179f, 0f);
        flow.toggle_object(Car);
        Studio.SetActive(true);
        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity3B_O";
        Logging.FocusTransform = Car.transform;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";

    }


    public void move_A2_to_explain()
    {
        Activtiy2.SetActive(true);
        Activity_Canvas_Q2.SetActive(false);
        Studio_Panel_A2_Q2.SetActive(false);
        Explain_A2_Q2.SetActive(true);
        Studio.SetActive(false);
        flow.toggle_object(emptyobject);


        Activtiy3.SetActive(false);
        Activity_Canvas_Q3.SetActive(false);
        Studio_Panel_A3_Q3.SetActive(false);
        Explain_A3_Q3.SetActive(false);
        Studio.SetActive(false);

        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity3B_E";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";

    }


    public void startactivtiy3()
    {
        Activtiy2.SetActive(false);
        Activity_Canvas_Q2.SetActive(false);
        Studio_Panel_A2_Q2.SetActive(false);
        Explain_A2_Q2.SetActive(false);
        Studio.SetActive(false);

        Activtiy3.SetActive(true);
        Activity_Canvas_Q3.SetActive(true);
        Studio_Panel_A3_Q3.SetActive(false);
        Explain_A3_Q3.SetActive(false);
        Studio.SetActive(false);

        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity3C_P";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";

    }

    public void ValidateQ3()
    {
        if (Q3Dropdown.value.ToString() == "0")
        {
            Debug.Log("None Selected");
            answers[2] = "";
            Logging.AnswerMarked = "";

            studio_Text.text = "You didn't answer!";
            studio_Text.color = Color.black;

        }
        else if (Q3Dropdown.value.ToString() == "1")
        {
            Debug.Log("A Selected");
            answers[2] = "A";
            Logging.AnswerMarked = "A";

            studio_Text.text = "Incorrect!";
            studio_Text.color = Color.red;

        }
        else if (Q3Dropdown.value.ToString() == "2")
        {
            Debug.Log("B Selected");
            answers[2] = "B";
            Logging.AnswerMarked = "B";

            studio_Text.text = "Incorrect!";
            studio_Text.color = Color.red;

        }
        else if (Q3Dropdown.value.ToString() == "3")
        {
            Debug.Log("C Selected");
            answers[2] = "C";
            Logging.AnswerMarked = "C";

            studio_Text.text = "Incorrect!";
            studio_Text.color = Color.red;
        }
        else if (Q3Dropdown.value.ToString() == "4")
        {
            Debug.Log("D Selected");
            answers[2] = "D";
            Logging.AnswerMarked = "D";

            studio_Text.text = "Correct!";
            studio_Text.color = Color.green;


        }
        Activity_Canvas_Q3.SetActive(false);
        Studio_Panel_A3_Q3.SetActive(true);
        Explain_A3_Q3.SetActive(false);
        rotreader.custom_angular_threshold(90f, 15f, 15f, 15f, 0f, 179f, 0f);
        flow.toggle_object(Cubes);
        Studio.SetActive(true);

        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity3C_O";
        Logging.FocusTransform = Cubes.transform;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";


    }


    public void move_A3_to_explain()
    {
        Activtiy3.SetActive(true);
        Activity_Canvas_Q3.SetActive(false);
        Studio_Panel_A3_Q3.SetActive(false);
        Explain_A3_Q3.SetActive(true);
        Studio.SetActive(false);
        flow.toggle_object(emptyobject);



        Logging.ScreenType = "Activity";
        Logging.ActivitySet = "Activity3C_E";
        Logging.FocusTransform = null;
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
        Logging.ActivitySet = "Activity3";
        Logging.FocusTransform = null;
        Logging.ActivityCorrectEnd = false;
        Logging.Status = "Connected";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void EndActivtiy(string text_to_display)
    {
        Activtiy2.SetActive(false);
        Activity_Canvas_Q2.SetActive(false);
        Studio_Panel_A2_Q2.SetActive(false);
        Explain_A2_Q2.SetActive(false);
        Studio.SetActive(false);


        Activtiy3.SetActive(false);
        Activity_Canvas_Q3.SetActive(false);
        Studio_Panel_A3_Q3.SetActive(false);
        Explain_A3_Q3.SetActive(false);
        Studio.SetActive(false);


        Activtiy1.SetActive(false);
        Activity_Canvas_Q1.SetActive(false);
        Studio_Panel_A1_Q1.SetActive(false);
        Explain_A1_Q1.SetActive(false);
        Studio.SetActive(false);




        Camera.main.transform.position = Initial_Transform.transform.position;
        Camera.main.transform.rotation = Initial_Transform.transform.rotation;


        classrom_1.SetActive(true);
        classroom_2.SetActive(false);
        board.SetActive(true);


        End_Screen.SetActive(true);
        Activity_Home_Screen.SetActive(false);

        Activity_Canvas_Q1.SetActive(false);
  

        endscreentext.text = text_to_display;
        rotreader.original_threshold();

        bool all_Correct = false;
        string combined_String = "";
        foreach(string str in answers)
        {
            combined_String += str;
        }
        if (combined_String.Equals("ABD"))
        {
            all_Correct = true;
        }

        if (all_Correct)
        {
            Logging.ScreenType = "Activity_End";
            Logging.ActivitySet = "Activity3";
            Logging.FocusTransform = null;
            Logging.ActivityCorrectEnd = true;
            Logging.Status = "Connected";

        }
        else
        {
            Logging.ScreenType = "Activity_End";
            Logging.ActivitySet = "Activity3";
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



        classrom_1.SetActive(true);
        classroom_2.SetActive(false);
        board.SetActive(true);

        End_Screen.SetActive(false);
        Activity_Home_Screen.SetActive(true);
        Activity_Canvas_Q1.SetActive(false);
    }


 


}
