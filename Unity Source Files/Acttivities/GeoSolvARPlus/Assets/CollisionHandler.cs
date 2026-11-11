using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Ground;



    public ActivityManager activityManager;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == Ground)
        {
            Debug.Log("Collided");
            activityManager.EndActivtiy("You Nailed It!");
            return;

        }
    }

    

}
