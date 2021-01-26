using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bus : MonoBehaviour
{
    public static Bus Instance = null;
    private PlayerControl controls;
    [Header("Control Raw Input")]
    [SerializeField] private float steer = 0f;
    [SerializeField] private float gas = 0f;
    [SerializeField] private float brake = 0f;
    [SerializeField] private float drift = 0f;
    [Header("Physical Property")]
    [SerializeField] private float acceleration = 300f;
    [SerializeField] private float steerSpeed = 10f;
    [SerializeField] private float extraSteerModifier = 2f;
    [SerializeField] private float gravity = 100f;
    [SerializeField] private float heightOffset = 2f;
    float rotate, currentRotate;
    float speed, currentSpeed;
    bool inDrift = false;
    float nitro = 0f;
    bool inNitro = false;
    [SerializeField] private float currentNitro = 0f;
    [SerializeField] private float maxNitro = 100f;
    [SerializeField] private float nitroDepletionRate = 25f;
    public Rigidbody rb;
    Transform busModel;
    [SerializeField] Transform lookAt;
    // Start is called before the first frame update
    void Awake()
    {
        // create the only instance
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        InitialInputControl();
    }
    void Start()
    {
        busModel = transform.GetChild(0);
        lookAt = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        speed += gas * acceleration;
        speed -= brake * acceleration * 0.5f;

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f); speed = 0f;
        Steer(steer);
        if (drift > 0 && !inDrift)
        {
            busModel.DOComplete();
            busModel.DOPunchPosition(transform.up * 0.5f, .3f, 5, 1);
            inDrift = true;
        }
        else if (drift > 0 && inDrift)
        {
            Steer(steer * extraSteerModifier);
        }
        else if (drift == 0 && inDrift)
        {
            inDrift = false;
        }
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;

        if (nitro == 1f && currentNitro > 0)
        {
            inNitro = true;
            currentNitro = Mathf.Max(currentNitro - nitroDepletionRate * Time.deltaTime, 0f);
        }
        else
        {
            inNitro = false;
            // To be removed after bus stop implemented
            // currentNitro = Mathf.Min(currentNitro + 10f * Time.deltaTime, maxNitro);
        }
    }
    void FixedUpdate()
    {
        if (rb == null)
            return;
        // Gravity
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        // Nitro Boost
        if (inNitro)
            rb.AddForce(transform.forward * 600f, ForceMode.Acceleration);
        // Drift Acceleration
        else if (inDrift)
            rb.AddForce(transform.forward * currentSpeed * 0.8f, ForceMode.Acceleration);
        // Normal Acceleration
        else
            rb.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);
        VisualUpdate();
    }
    void OnEnable()
    {
        if (controls != null)
            controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        if (controls != null)
            controls.Gameplay.Disable();
    }

    void InitialInputControl()
    {
        controls = new PlayerControl();
        controls.Gameplay.Steer.performed += ctx => steer = ctx.ReadValue<float>();
        controls.Gameplay.Steer.canceled += _ => steer = 0f;
        controls.Gameplay.Gas.performed += ctx => gas = ctx.ReadValue<float>();
        controls.Gameplay.Gas.canceled += _ => gas = 0f;
        controls.Gameplay.Drift.performed += ctx => drift = ctx.ReadValue<float>();
        controls.Gameplay.Drift.canceled += _ => drift = 0f;
        controls.Gameplay.Brake.performed += ctx => brake = ctx.ReadValue<float>();
        controls.Gameplay.Brake.canceled += _ => brake = 0f;
        controls.Gameplay.Nitro.performed += _ => nitro = 1f;
        controls.Gameplay.Nitro.canceled += _ => nitro = 0f;
    }

    void VisualUpdate()
    {
        // Visual Change
        // Follow Collider
        transform.position = rb.transform.position - new Vector3(0, heightOffset, 0);
        if (currentSpeed >= 0)
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);
        else
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y - currentRotate, 0), Time.deltaTime * 5f);
    }

    public void AddNitro(float value)
    {
        currentNitro = Mathf.Min(currentNitro + value, maxNitro);
    }

    private void Steer(float amount)
    {
        rotate = steerSpeed * amount;
        lookAt.localPosition = Vector3.Lerp(lookAt.localPosition, Vector3.right * rotate / steerSpeed, 0.5f * Time.deltaTime);
    }
    public bool IsNitroFull() => currentNitro.Equals(maxNitro);
}
