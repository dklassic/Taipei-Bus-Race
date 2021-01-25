using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BusSimpleController : MonoBehaviour
{
    private PlayerControl controls;
    [Header("Control Raw Input")]
    [SerializeField] private float steer = 0f;
    [SerializeField] private float gas = 0f;
    [SerializeField] private float brake = 0f;
    [SerializeField] private float drift = 0f;
    [Header("Physical Property")]
    [SerializeField] private float acceleration = 400f;
    [SerializeField] private float steerSpeed = 20f;
    [SerializeField] private float extraSteerModifier = 1.5f;
    [SerializeField] private float gravity = 100f;
    [SerializeField] private float heightOffset = 2f;
    float rotate, currentRotate;
    float speed, currentSpeed;
    bool inDrift = false;
    [SerializeField] Rigidbody rb;
    Transform busModel;
    // Start is called before the first frame update
    void Awake()
    {
        InitialInputControl();
    }
    void Start()
    {
        busModel = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        speed += gas * acceleration;
        speed -= brake * acceleration;
        //Follow Collider
        transform.position = rb.transform.position - new Vector3(0, heightOffset, 0);

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f); speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;
        Steer(steer);

        if (drift > 0 && !inDrift)
        {
            busModel.DOComplete();
            busModel.DOPunchPosition(transform.up * 0.5f, .3f, 5, 1);
            inDrift = true;
        }
        if (drift > 0 && inDrift)
        {
            Steer(steer * extraSteerModifier);
        }
        if (drift == 0 && inDrift)
        {
            inDrift = false;
        }

    }
    void FixedUpdate()
    {
        if (rb == null)
            return;

        //Gravity
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        rb.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);
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
    }

    public void Steer(float amount)
    {
        rotate = steerSpeed * amount;
    }
}
