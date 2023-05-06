using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Movement : MonoBehaviour
{

    public Text highscoretext;
    public Text scoretext;
    public Text speedtext;
    public int score;
    public float speed;
    
   
   AudioSource startup;
    AudioSource song;
  
    public GameObject panel;
    public WheelCollider frontleftwheel, frontrightwheel, backleftwheel, backrightwheel;
    public Transform frontleftwheelt, frontrightwheelt, backleftwheelt, backrightwheelt;


    public float accelerationforce = 300f;
    public float breakingforce = 3000f;
    private float presentbreakforce = 0f;
    public float presentacceleration = 0f;


    public float wheelsTorque = 35;
    private float presentturnangle = 0f;

  


    private void Start()
    {

        PlayerPrefs.GetInt("score");


        scoretext.text = PlayerPrefs.GetInt("score").ToString();
        highscoretext.text = PlayerPrefs.GetInt("highscore").ToString();
        startup = GetComponent<AudioSource>();
        song = GameObject.FindGameObjectWithTag("startsound").GetComponent<AudioSource>();
       
        
    }


    void Update()
    {



        
        speed = Mathf.Clamp(speed, 0.2f, 300);


        if (presentacceleration > 1)
        {
            StartCoroutine(Acceleration());
            if(speed>200)
            {
                speed -=0.2f;
            }
            if(speed>250)
            {
                speed -= 0.2f;
            }
           
        }
        if (presentacceleration == 0)
        {
            speed -= 0.2f;
            
            

   
        }

        if(speed>1)
        {
            score++;
        }
        MoveCar();
        CarSteering();
       

      
      
        speedtext.text = speed.ToString();
       

        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
        }

        
        PlayerPrefs.SetInt("score", score);



    }


    private void MoveCar()
    {
        frontleftwheel.motorTorque = presentacceleration;
        frontrightwheel.motorTorque = presentacceleration;
        backleftwheel.motorTorque = presentacceleration;
        backrightwheel.motorTorque = presentacceleration;

        presentacceleration = accelerationforce * SimpleInput.GetAxis("Vertical");
    }


    private void CarSteering()
    {
        
        frontleftwheel.steerAngle = presentturnangle;
        frontrightwheel.steerAngle = presentturnangle;

        SteeringWheels(frontleftwheel, frontleftwheelt);
        SteeringWheels(frontrightwheel, frontrightwheelt);
        SteeringWheels(backleftwheel, backleftwheelt);
        SteeringWheels(backrightwheel, backrightwheelt);

        presentturnangle = wheelsTorque *  SimpleInput.GetAxis("Horizontal");
    }


    void SteeringWheels(WheelCollider WC,Transform WT)
    {
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);

        WT.position = position;
        WT.rotation = rotation;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "road")
        {
           
            SceneManager.LoadScene("SampleScene");
           
        }
        if (collision.gameObject.tag == "Obstacle")
        {

            SceneManager.LoadScene("SampleScene");

        }

        PlayerPrefs.SetInt("score", score);
    }

    public void StartGame()
    {
        
        panel.SetActive(false);
        startup.Play();
        song.Play();
      
    }


    public void ApplyBreaks()
    {
        StartCoroutine(carBreks());
        speed -= 150;
        
    }
   

   
  IEnumerator carBreks()
    {
        presentbreakforce = breakingforce;

        frontleftwheel.brakeTorque = presentbreakforce;
        frontrightwheel.brakeTorque = presentbreakforce;
        backleftwheel.brakeTorque = presentbreakforce;
        backrightwheel.brakeTorque = presentbreakforce;

        yield return new WaitForSeconds(2f);

        presentbreakforce = 0f;

        frontleftwheel.brakeTorque = presentbreakforce;
        frontrightwheel.brakeTorque = presentbreakforce;
        backleftwheel.brakeTorque = presentbreakforce;
        backrightwheel.brakeTorque = presentbreakforce;

        
    }


    IEnumerator Acceleration()
    {
        yield return new WaitForSeconds(0.2f);
        speed += 0.5f;
    }

}

