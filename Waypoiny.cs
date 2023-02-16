using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoiny : MonoBehaviour
{
    public Transform planetSprite;
    public Transform playerShip;

    void Update()
    {
    Vector3 planetScreenPos = Camera.main.WorldToScreenPoint(planetSprite.position);
    Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2f;
    float angle = Mathf.Atan2(planetScreenPos.y - screenCenter.y, planetScreenPos.x - screenCenter.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
