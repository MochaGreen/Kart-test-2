using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class MovementKart_1 : MonoBehaviour
{



    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;


    public float speed, currentSpeed; // set this to private later
    public float rotate, currentRotate;


    [Header("Parameters")]

    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;

    [Header("Model Parts")]

    public Transform frontWheels;
    public Transform backWheels;


    void Start()
    {


    }

    void Update()
    {


        //Follow Collider
        transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);

        //Accelerate
        if (Input.GetKey(KeyCode.W)) { 
            speed = acceleration;
        Debug.Log("up");
        }

        //Steer
        if (Input.GetAxis("Horizontal") != 0)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            Steer(dir, amount);
        }





        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f); speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;
    }


  

    private void FixedUpdate()
    {
        //  sphere.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);

     sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        //Gravity
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f, layerMask);

        //Normal Rotation
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

}
