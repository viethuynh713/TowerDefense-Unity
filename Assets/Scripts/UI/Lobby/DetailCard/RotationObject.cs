using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MythicEmpire.UI.Lobby
{
public class RotationObject : MonoBehaviour
{
    public float rotationSpeed = 1f;
    private Vector3 initialMousePosition;

    private void OnEnable()
    {
        // transform.rotation = Quaternion.Euler(0,180,0);
    }

    void OnMouseDrag()
    {

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, -mouseX * rotationSpeed);
    }
}}
