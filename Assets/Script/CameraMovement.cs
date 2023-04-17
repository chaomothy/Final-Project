using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    // OFFSET FOR THE CAMERA RELATED TO THE PLAYER
    private Vector3 playerOffset = new Vector3(0f, 0f, -10f);

    // TIMER THAT LETS CAMERA TRAIL BEHIND SMOOTHLY INSTEAD OF RIGIDLY FOLLOWING THE PLAYER AROUND
    private float smoothTime = 0.25f;

    
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;


    void Update()
    {
        // CREATES A VECTOR3 FOR THE POSITION OF THE TARGET, AND USES THE OFFSET TO MAKE THE CAMERA FOLLOW SMOOTHLY BEHIND THE TARGET
        Vector3 targetPosition = target.position + playerOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
