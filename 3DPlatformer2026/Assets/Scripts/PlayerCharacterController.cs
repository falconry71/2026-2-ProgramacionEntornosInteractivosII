using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{

    CharacterController cc;

    private Vector2 playerHorizontalInput;
    private Vector3 horizontalVelocity;
    public float movementSpeed = 5f;

    public float jumpForce = 10f;

    public float jumpCounter = 0f; 
    private float jumpMult1 = 1.1f;
    private float jumpMult2 = 1.2f;
    
    public float jumpTimeLimit = 0.3f;

    public bool enSuelo = false;

    
    







    private Vector3 moveDirection;

    private float gravity = -9.8f;

    public float gravityScale = 1f;


    public Transform meshTransform;


    private Vector3 verticalVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
        verticalVelocity = new Vector3(0f, 0f, 0f);
    }


    private void Update()
    {
        if (cc.isGrounded == true && !enSuelo)
        {
            enSuelo = true;
            Debug.Log("en suelo");
        }
        else if (cc.isGrounded == false && enSuelo == true)
        {
            enSuelo = false;
            Debug.Log("no en suelo");
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Componente XZ (plano del suelo)
        moveDirection = transform.TransformDirection(new Vector3(playerHorizontalInput.x, 0f, playerHorizontalInput.y));
        horizontalVelocity = moveDirection * movementSpeed;

        //Componente Y (gravedad)
        verticalVelocity.y += gravity * gravityScale * Time.deltaTime; 
        
        //Los juntamos en un componente XYZ
        Vector3 newVelocity = new Vector3(horizontalVelocity.x, verticalVelocity.y, horizontalVelocity.z);
        
        //Movemos en ese componente XYZ
        cc.Move(newVelocity * Time.deltaTime);




        if (enSuelo == true)
        {
            jumpTimeLimit -= Time.deltaTime;


            

            

            

        }

        else if (enSuelo == false) 
        {
            jumpTimeLimit = 0.3f;

        }

        

        if(jumpCounter > 2f || jumpTimeLimit < 0f)
        {
            jumpCounter = 0;
        }

        

        
    }


    public void OnMove (InputAction.CallbackContext context)
    {
        playerHorizontalInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed && cc.isGrounded)
        {
            verticalVelocity.y = jumpForce;

            jumpCounter++;
        }

        if(jumpCounter == 1 && cc.isGrounded)
        {
            verticalVelocity.y = jumpForce * jumpMult1;
        }
        else if(jumpCounter == 2 && cc.isGrounded)
        {
            verticalVelocity.y = jumpForce * jumpMult2;
        }

       // funny setting xdxd 
        if(context.canceled && jumpCounter == 0)
        {
            verticalVelocity.y = 0f; 
        }
       



    }

    public void LateUpdate()
    {
        Vector3 lookDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);

        if (meshTransform != null && lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetMeshRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            meshTransform.rotation = targetMeshRotation;
        }
    }



    




}
