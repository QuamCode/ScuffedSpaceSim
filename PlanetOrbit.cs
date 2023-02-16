using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    public Transform orbitCenter;
    public float orbitRadius = 2.0f;
    public float orbitSpeed = 1.0f;
    public bool alwaysFaceCenter = false;

    private Vector3 orbitAxis;
    private Vector3 orbitPosition;
    private Vector3 rotationAxis;

    public Transform shadowMask;
    public bool alwaysFaceAwayFromCenter = false;

    // Start is called before the first frame update
    void Start()
    {
        orbitAxis = Vector3.forward;
        orbitPosition = transform.position - orbitCenter.position;
        rotationAxis = Vector3.up;
        
    }

    // Update is called once per frame
    void Update()
{
    // Calculate new position based on orbit
    orbitPosition = Quaternion.AngleAxis(orbitSpeed * Time.deltaTime, orbitAxis) * orbitPosition;
    transform.position = orbitCenter.position + orbitPosition.normalized * orbitRadius;

    // Rotate planet to face center
    if (alwaysFaceCenter)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, orbitPosition);
    }

    // Rotate shadow mask to face away from center
    if (shadowMask && alwaysFaceAwayFromCenter)
    {
        Vector3 shadowDirection = shadowMask.transform.position - orbitCenter.position;
        shadowDirection.z = 0f;
        float zRotation = Mathf.Atan2(shadowDirection.y, shadowDirection.x) * Mathf.Rad2Deg + 0f;
        shadowMask.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }
}
}