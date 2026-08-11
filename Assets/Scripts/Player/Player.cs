using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Components
    private Rigidbody rb;


    // References
    [SerializeField] private GameObject flashlight;

    public static Player Instance { get; private set; }

    // Movement
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpForce = 20;

    [SerializeField] private float runningAcrescent = 5;
    [SerializeField] private float timeForRunAgain = 1.5f;
    [SerializeField] private float lastRunStopTime;
    private bool wasRunning;


    // Stamina
    [SerializeField] public float maxStamina = 100;
    [SerializeField] public float currentStamina;
    [SerializeField] private float staminaDrain = 15f;
    [SerializeField] private float staminaRecovery = 8f;
    public event Action<float, float> OnStaminaChanged;


    // Sede
    [SerializeField] public float maxThirst = 100;
    public float currentThirst;
    [SerializeField] private float minutesForDrainAllThirst = 10;
    public event Action<float, float> OnThristChanged;


    // Fome
    [SerializeField] public float maxHungry = 100;
    public float currentHungry;
    [SerializeField] private float minutesForDrainAllHungry = 10;
    public event Action<float, float> OnHungryChanged;


    // Vida
    [SerializeField] public float maxHealth = 100;
    public float currentHealth;
    public event Action<float, float> OnHealthChanged;


    // State
    private bool isRunning;
    private bool jumpRequested;
    private bool isGrounded;
    private bool flashlightIsOn;
    
    private Vector3 direction;

    void Awake()
    {
        // Inicializa sem bloquear a corrida
        lastRunStopTime = -timeForRunAgain;

        currentThirst = maxThirst;
        currentHungry = maxHungry;

        currentStamina = maxStamina;
        currentHealth = maxHealth;
        Instance = this;
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
        direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            direction += Vector3.forward;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            direction += Vector3.left;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            direction += Vector3.right;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            direction += Vector3.back;

        bool cooldownFinished =
            Time.time >= lastRunStopTime + timeForRunAgain;

        isRunning =
            direction != Vector3.zero &&
            Input.GetKey(KeyCode.LeftShift) &&
            currentStamina > 0 &&
            cooldownFinished;
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

        UpdateStamina(currentSpeed);

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
    
    void UpdateStamina(float currentSpeed)
    {
        // Acabou de parar de correr?
        if (wasRunning && !isRunning)
        {
            lastRunStopTime = Time.time;
        }

        if (isRunning)
        {
            currentSpeed += runningAcrescent;

            currentStamina -= staminaDrain * Time.fixedDeltaTime;
        }
        else
        {
            bool cooldownFinished =
                Time.time >= lastRunStopTime + timeForRunAgain;

            if (cooldownFinished)
            {
                currentStamina += staminaRecovery * Time.fixedDeltaTime;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        OnStaminaChanged?.Invoke(currentStamina, maxStamina);

        wasRunning = isRunning;
    }
    
    IEnumerator DrainThirst()
    {
        float quantityForDrain = maxThirst / (minutesForDrainAllThirst * 60);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currentThirst -= quantityForDrain;
            OnThristChanged?.Invoke(currentThirst, maxThirst);
        }
    }

    IEnumerator DrainHungry()
    {
        float quantityForDrain = maxHungry / (minutesForDrainAllHungry * 60);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currentHungry -= quantityForDrain;
            OnHungryChanged?.Invoke(currentHungry, maxHungry);
        }
    }

    public void ReceiveDamage(float damage)
    {
        currentHealth = Math.Clamp(currentHealth - damage, 0, maxHealth);
    }

    void AddHealth(float value)
    {
        currentHealth += value;
    }
    void AddSatiety(float value)
    {
        currentHungry += value;
    }
    void AddHydratation(float value)
    {
        currentThirst += value;
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
