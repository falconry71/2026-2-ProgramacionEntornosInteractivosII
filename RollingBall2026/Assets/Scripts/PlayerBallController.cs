using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBallController : MonoBehaviour
{
    private Rigidbody rb;

    public Vector3 movementDirection;

    public float maxSpeed = 2f;


    public float movementSpeed = 2f;

    public float jumpForce = 10f;


    private float deathHeight = -200f;


    private bool isOnDash = false;

    public float dashTime = 1f;
    public float dashCooldown = 0.5f;
    public float dashForce = 3f;
    private bool isMaxSpeedLimitActive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isOnDash = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputMove = context.ReadValue<Vector2>();
        Debug.Log(inputMove);

        movementDirection = new Vector3(inputMove.x, 0f, inputMove.y);

        //rb.AddForce(movementDirection, ForceMode.Force);
    }

    public void OnLaunch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //(0, 1,0) * 100 -> (0, 100, 0)
            if (Mathf.Abs(rb.linearVelocity.y) - 0.1f < 0)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            else if (Mathf.Abs(rb.linearVelocity.y) - 0.1f > 0)
            {
                rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);


            }
        }
            //if(context.canceled)
            //{
            //}
        }


    public IEnumerator Dash()
    {
        isOnDash = true;
        isMaxSpeedLimitActive = false;

        Vector3 dashDirection = rb.linearVelocity.normalized;

        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        Debug.Log("Inicio dash!"); //Asignar modo dash

        yield return new WaitForSeconds(dashTime);

        Debug.Log("Termino dash!"); //Quitar modo dash

        isMaxSpeedLimitActive = true;

        yield return new WaitForSeconds(dashCooldown);

        isOnDash = false;


    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if(!isOnDash)
            {
                StartCoroutine(Dash());
            }

        }
        
    }




    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown("X"))
        

        if(transform.position.y < deathHeight)
        {
            Destroy(this.gameObject);
        }
    }


    private void FixedUpdate()
    {

        rb.AddForce(movementDirection * movementSpeed, ForceMode.Force);


        //limit = 2.
        //maxSpeed = 3;
        //rb.linearvelocity = (3, 6, 5);

        if (isMaxSpeedLimitActive)
        {
            Vector3 linearVelocityXZ = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            //(3, 0, 5)

            if (linearVelocityXZ.magnitude > maxSpeed)
            {
                Vector3 linearXZLimit = linearVelocityXZ.normalized * maxSpeed;
                //(3, 0, 5) -> (3/6, 0/6, 5/6) * 3 -> (1.5, 0, 1.67);

                rb.linearVelocity = new Vector3(linearVelocityXZ.x, rb.linearVelocity.y, linearVelocityXZ.z);
                //(1.5, 6, 1.67)
            }
        }

        //Vector3 force = new Vector3(1f, 0f, 0f);

        //rb.AddForce(force);
    }
}
