using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody rb;

    // References
    [SerializeField] private GameObject flashlight;

    // Movement
    [SerializeField] private float speed = 10;
    [SerializeField] private float runningAcrescent = 5;
    [SerializeField] private float jumpForce = 5;

    // Stamina
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaDrain = 15f;
    [SerializeField] private float staminaRecovery = 8f;

    // Sede
    [SerializeField] private float maxThirst = 100;
    [SerializeField] private float currentThirst;
    [SerializeField] private float minutesForDrainAllThirst = 25;

    // Fome
    [SerializeField] private float maxHungry = 100;
    [SerializeField] private float currentHungry;
    [SerializeField] private float minutesForDrainAllHungry = 25;


    // State
    private float currentStamina;
    private bool isRunning;
    private bool jumpRequested;
    private bool isGrounded;
    private bool flashlightIsOn;
    
    private Vector3 direction;

    void Awake()
    {
        currentThirst = maxThirst;

        currentStamina = maxStamina;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    
        //Se a lanterna estiver ligada por padrão segue
        flashlightIsOn = flashlight.activeSelf;

        StartCoroutine(DrainThirst());
        StartCoroutine(DrainHungry());
    }

    // Update is called once per frame
    void Update()
    {
        ReadMovement();
        ReadJump();
        ReadFlashlight();
        ReadThirst();
    }

    void FixedUpdate()
    {
        Move();

        if (jumpRequested){
            Jump();
        }

        jumpRequested = false;
    }

    void ReadMovement()
    {
        isRunning = false;
        direction = Vector3.zero;

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            direction += Vector3.forward;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            direction += Vector3.left;
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            direction += Vector3.right;            
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            direction += Vector3.back;            
        }

        if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift) && currentStamina > 0){
            isRunning = true;
        }
    }

    void ReadJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            jumpRequested = true;
        }   
    }

    void ReadFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlightIsOn = !flashlightIsOn;
            flashlight.SetActive(flashlightIsOn);
        }
    }

    void ReadThirst()
    {
        if(currentThirst <= 0)
        {
            //MORRE
        }
    }

    void Move()
    {
        float currentSpeed = speed;

        direction.Normalize();

        if (isRunning)
        {
            currentSpeed += runningAcrescent;
            currentStamina -= staminaDrain * Time.fixedDeltaTime;
        }
        else
        {
            currentStamina += staminaRecovery * Time.fixedDeltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        Vector3 moveDirection =
            transform.forward * direction.z +
            transform.right * direction.x;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDirection.x * currentSpeed;
        velocity.z = moveDirection.z * currentSpeed;

        rb.linearVelocity = velocity;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    
    IEnumerator DrainThirst()
    {
        float quantityForDrain = maxThirst / (minutesForDrainAllThirst * 60);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currentThirst -= quantityForDrain;
        }
    }

    IEnumerator DrainHungry()
    {
        float quantityForDrain = maxHungry / (minutesForDrainAllHungry * 60);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currentHungry -= quantityForDrain;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground")){ 
            isGrounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground")){
            isGrounded = false;
        }
    }
}
