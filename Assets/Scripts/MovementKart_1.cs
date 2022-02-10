using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class MovementKart_1 : MonoBehaviour
{



    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;
    private bool accelerating = false;
    public bool isJumping = false;

    private float speed, currentSpeed; // set this to private later
    private float rotate, currentRotate;



    [Header("Parameters")]
    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;
    public float jumpStrength = 0;
    public float wheelRotationAmount = 10;

    [Header("Model Parts")]

    public Transform frontWheels;
    public Transform backWheels;

    public Transform w1;
    public Transform w2;
    public Transform w3;
    public Transform w4;


    void Start()
    {
        sphere.GetComponent<Rigidbody>();

    }

    void Update()
    {

        //Follow Collider // basically attachs the car to the collider
        transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);

        //Accelerate
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) { 
            speed = acceleration;
            accelerating = true;
        }

        //Steer
        if (Input.GetAxis("Horizontal") != 0)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            Steer(dir, amount);
        }

        if (Input.GetKeyDown(KeyCode.Space)) { 
            isJumping = true;
            Invoke("Jump", .5f);
        }




        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f);
        speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        rotate = 0f;


        //b) Wheels
        frontWheels.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * wheelRotationAmount), frontWheels.localEulerAngles.z);
        //test

    

        // frontWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude / 2);
        //backWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude / 2);
    }




    private void FixedUpdate()
    {
        //  sphere.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);
        if(accelerating && Input.GetKey(KeyCode.W)) //forward
        { 
           sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);
             w1.Rotate(new Vector3(-1000 * Time.deltaTime, 0, 0));
             w2.Rotate(new Vector3(-1000 * Time.deltaTime, 0, 0));
             w3.Rotate(new Vector3(-1000 * Time.deltaTime, 0, 0));
             w4.Rotate(new Vector3(-1000 * Time.deltaTime, 0, 0));
        }

        if (accelerating && Input.GetKey(KeyCode.S)) // reverse;
        { 
           sphere.AddForce(-transform.forward * currentSpeed, ForceMode.Acceleration);
            w1.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
            w2.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
            w3.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
            w4.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
        }

  

        //Gravity
        //sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        RaycastHit hitOn;
        RaycastHit hitNear;
                        //origin                                  //direction            //max distance//layer mask
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f, layerMask);

        //Normal Rotation                                          //basically the longer it takes to hit the more slanted a floor is.
       kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);  
       kartNormal.Rotate(0, transform.eulerAngles.y, 0);
    }



    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }



    private void Speed(float x)
    {
        currentSpeed = x;
    }


    public void Jump() {
      
            sphere.AddForce(Vector3.up * speed * Time.deltaTime);
        isJumping = false;
    }
}