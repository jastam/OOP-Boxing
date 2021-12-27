using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera3rdPerson : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float distanceBehind;
    [SerializeField] float distanceTop;
    [SerializeField] float angle;

    private Vector3 previousPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        SetCameraToStartingPositionAndRotation();

        // save player position to calculate movement
        previousPlayerPosition = player.transform.position;
    }

    private void SetCameraToStartingPositionAndRotation()
    {
        transform.position = player.transform.position + GetRelativeCameraPosition();
        transform.rotation = GetCameraRotation();
    }

    private Quaternion GetCameraRotation()
    {
        Vector3 rotation = player.transform.rotation.eulerAngles;
        rotation.x = angle;
        return Quaternion.Euler(rotation);
    }

    private Vector3 GetRelativeCameraPosition()
    {
        float degrees = player.transform.rotation.eulerAngles.y;
        float radians = degrees * Mathf.Deg2Rad;
        float x = Mathf.Sin(radians);
        float z = Mathf.Cos(radians);
        Vector3 position = new Vector3(x, 0, z) * -distanceBehind;
        position.y = distanceTop;
        return position;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();

        var mouseX = Input.GetAxis("Mouse X");
        this.transform.RotateAround(player.transform.position, Vector3.up, mouseX * 2);

        // keep previous in case camere moves out of bounds
        var prevPosition = this.transform.position;
        var prevRotation = this.transform.rotation;

        var mouseY = Input.GetAxis("Mouse Y");
        this.transform.RotateAround(player.transform.position, this.transform.right, -mouseY * 2);

        float cameraAngle = Vector3.Angle(this.transform.forward, Physics.gravity);
        if (this.transform.position.y < 0.1 || cameraAngle < 20)
        {
            // if camera moves out of bounds reset position
            this.transform.position = prevPosition;
            this.transform.rotation = prevRotation;
        }
    }

    private void FollowPlayer()
    {
        // calculate player movement since last frame
        Vector3 playerMovement = player.transform.position - previousPlayerPosition;

        // move camera accordingly
        this.transform.position += playerMovement;

        // save player position to calculate movement in next frame
        previousPlayerPosition = player.transform.position;
        
    }
}
