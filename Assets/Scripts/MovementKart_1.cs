using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;


public class MovementKart_1 : MonoBehaviour
{



    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;
    public bool accelerating = false;
    public bool isJumping = false;

    private float speed, currentSpeed; // set this to private later
    private float rotate, currentRotate;



    [Header("Parameters")]
    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;
    public float jumpStrength = 0; // need to set this in the jump function;
    public float wheelRotationAmount = 10;
    public float jumpCooldown;
    public float jumpHeight;

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
        //       transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);//moved this out of the update method because it was causing some weird shaking

        //Accelerate
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) { 
            speed = acceleration;
            accelerating = true;
        }
        else { accelerating = false; }

        //Steer
        if (Input.GetAxis("Horizontal") != 0) // need to find a way to jump and turn
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1; // if value is higher than 0 then set it to 1 or -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal"))); 
            Steer(dir, amount);
        }

        if (Input.GetKeyDown(KeyCode.Space)) { 
            isJumping = true;
            Invoke("Jump", jumpCooldown);
        }



        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f);
        speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        rotate = 0f;


        //b) Wheels
        //frontWheels.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * wheelRotationAmount), frontWheels.localEulerAngles.z);// moving this to fixed update to handle reverse code
        //test

    

        // frontWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude / 2);
        //backWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude / 2);
    }




    private void FixedUpdate()
    {
        //Follow Collider // basically attachs the car to the collider
       transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);//moved this out of the update method because it was causing some weird shaking

        //  sphere.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);
        if (accelerating && Input.GetKey(KeyCode.W)) //forward
        { 
             sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);//forward
             w1.Rotate(new Vector3(-1000 * Time.deltaTime, 0, 0));// rotation for individual wheels
             w2.Rotate(new Vector3(-1000 * Time.deltaTime, 0, 0));
             w3.Rotate(new Vector3(-1000 * Time.deltaTime, 0, 0));
             w4.Rotate(new Vector3(-1000 * Time.deltaTime, 0, 0));
             frontWheels.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * wheelRotationAmount), frontWheels.localEulerAngles.z);
        }

        if (accelerating && Input.GetKey(KeyCode.S)) // reverse;
        { 
           sphere.AddForce(-transform.forward * currentSpeed, ForceMode.Acceleration); //reverse
            w1.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));// rotation for individual wheels
            w2.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
            w3.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
            w4.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
            frontWheels.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * -wheelRotationAmount), frontWheels.localEulerAngles.z);
        }

  

        //Gravity
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering  // physically rotates vehicle
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

        kartModel.parent.DOComplete(); // using DOTween to make the kart jump
        kartModel.parent.DOPunchPosition(transform.up * jumpHeight, .3f, 5, 1);
        
        isJumping = false;

    }
}