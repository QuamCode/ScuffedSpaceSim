using UnityEngine;
using System.Collections;

public class ExampleShipControl : MonoBehaviour {

    public float acceleration_amount = 1f;
    public float rotation_speed = 1f;
    public GameObject turret;
    public float turret_rotation_speed = 3f;

    // New variables for hyperdrive

    public GameObject engine_trail_renderer;
    public GameObject engine_particle_emitter;
    public GameObject hyperdrive_particle_emitter;

    private bool isHyperdriveCharging = false;
    private bool isHyperdriveActive = false;
    private float hyperdriveChargeTimer = 0f;
    private float timeElapsedWhileHyperdriveActive = 0f;
    private const float hyperdrive_charge_time = 5f;
    private const float hyperdrive_acceleration_increase = 300f;
    private const float hyperdrive_rotation_decrease = 80f;
    private const float hyperdrive_speed_threshold = 50f;
    private float max_speed = 600f;
    private const float hyperdrive_time_limit = 20f;
    private float timeElapsedInHyperdrive = 0f;


    // Use this for initialization
    void Start () {
    hyperdrive_particle_emitter.GetComponent<ParticleSystem>().Stop();
    engine_particle_emitter.GetComponent<ParticleSystem>().Play();
    }
    
    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            Screen.lockCursor = !Screen.lockCursor;    
    
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isStrafing = Input.GetKey(KeyCode.LeftShift);

        if (!isHyperdriveCharging && !isHyperdriveActive && Input.GetKey(KeyCode.Space)) {
        // Begin charging the hyperdrive
        isHyperdriveCharging = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        hyperdrive_particle_emitter.GetComponent<ParticleSystem>().Play();
        Debug.Log("Hyperdrive charging...");
    }
    else if (isHyperdriveCharging && !Input.GetKey(KeyCode.Space)) {
        // Stop charging the hyperdrive if space bar is released
        isHyperdriveCharging = false;
        hyperdrive_particle_emitter.GetComponent<ParticleSystem>().Stop();
        Debug.Log("Hyperdrive charge canceled");
    }

    if (isHyperdriveCharging) {
        // Check if the hyperdrive is fully charged
        hyperdriveChargeTimer += Time.deltaTime;
        if (hyperdriveChargeTimer >= hyperdrive_charge_time) {
            Debug.Log("Hyperdrive charged!");

            // Change engine trail and particle emitter colors to blue
            engine_trail_renderer.GetComponent<TrailRenderer>().material.color = Color.blue;
            engine_particle_emitter.GetComponent<ParticleSystem>().startColor = Color.blue;

            // Play hyperdrive particle emitter
            hyperdrive_particle_emitter.GetComponent<ParticleSystem>().Play();

            // Increase acceleration and decrease rotation for hyperdrive
            acceleration_amount += hyperdrive_acceleration_increase;
            rotation_speed -= hyperdrive_rotation_decrease;

            // Reset charge timer and set flag to indicate hyperdrive is active
            hyperdriveChargeTimer = 0f;
            isHyperdriveActive = true;
            isHyperdriveCharging = false;
            Debug.Log("Hyperdrive started");
        }
            else {
        // Stop all movement while the hyperdrive is charging
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
    }
    }

    if (isHyperdriveActive) {
        // Move the ship at full speed while hyperdrive is active
        GetComponent<Rigidbody2D>().AddForce(transform.up * acceleration_amount * Time.deltaTime);
        timeElapsedWhileHyperdriveActive += Time.deltaTime;
        engine_trail_renderer.GetComponent<TrailRenderer>().material.color = Color.blue;

        if (timeElapsedInHyperdrive >= hyperdrive_time_limit || timeElapsedWhileHyperdriveActive >= hyperdrive_charge_time && GetComponent<Rigidbody2D>().velocity.magnitude < hyperdrive_speed_threshold) {
            // Stop hyperdrive and reset engine trail and particle emitter colors to normal
            isHyperdriveActive = false;
            engine_trail_renderer.GetComponent<TrailRenderer>().material.color = Color.white;
            engine_particle_emitter.GetComponent<ParticleSystem>().startColor = Color.white;
            hyperdrive_particle_emitter.GetComponent<ParticleSystem>().Stop();

            // Reset acceleration and rotation to normal values
            acceleration_amount -= hyperdrive_acceleration_increase;
            rotation_speed += hyperdrive_rotation_decrease;

            timeElapsedInHyperdrive = 0f;
            timeElapsedWhileHyperdriveActive = 0f;
            max_speed = 50f;
            Debug.Log("Hyperdrive stopped");
        }
    }

        if (!isHyperdriveCharging && !isHyperdriveActive && verticalInput > 0f) {
            GetComponent<Rigidbody2D>().AddForce(transform.up * acceleration_amount * Time.deltaTime);
        }
        if (!isHyperdriveCharging && !isHyperdriveActive &&verticalInput < 0f) {
            GetComponent<Rigidbody2D>().AddForce((-transform.up) * acceleration_amount * Time.deltaTime);
        }
    
        if (!isHyperdriveCharging && !isHyperdriveActive && isStrafing && horizontalInput > 0f) {
            GetComponent<Rigidbody2D>().AddForce((-transform.right) * acceleration_amount * 0.6f * Time.deltaTime);
        }
        if (!isHyperdriveCharging && !isHyperdriveActive && isStrafing && horizontalInput < 0f) {
            GetComponent<Rigidbody2D>().AddForce((transform.right) * acceleration_amount * 0.6f * Time.deltaTime);
        }

        if (!isHyperdriveCharging && !isHyperdriveActive && !isStrafing && horizontalInput < 0f) {
            GetComponent<Rigidbody2D>().AddTorque(rotation_speed * Time.deltaTime);
        }
        if (!isHyperdriveCharging && !isHyperdriveActive && !isStrafing && horizontalInput > 0f) {
            GetComponent<Rigidbody2D>().AddTorque(-rotation_speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.H)) {
            transform.position = new Vector3(0,0,0);
        }
    }
}