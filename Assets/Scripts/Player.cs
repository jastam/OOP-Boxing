using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    private Rigidbody playerRb;
    private GameObject mainCamera;
    private Vector3 previousPosition;
    private Weapon[] weapons;
    private int activeWeapon = -1;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        mainCamera = GameObject.Find("Main Camera");
        weapons = GetComponentsInChildren<Weapon>(true);
        if (weapons.Length > 0)
        {
            SwitchToWeapon(0);
        }

        previousPosition = transform.position;
    }

    //POLYMORPHISM
    private void SwitchToWeapon(int weaponIndex)
    {
        if (weaponIndex == activeWeapon || weapons.Length < 2)
        {
            return;
        }

        if (activeWeapon >= 0)
        {
            weapons[activeWeapon].Deactivate();
        }

        activeWeapon = weaponIndex;

        weapons[activeWeapon].Activate();
    }

    //POLYMORPHISM
    private void SwitchToWeapon(bool up)
    {
        int switchTo = activeWeapon;

        if (up)
        {
            switchTo++;
        }
        else
        {
            switchTo--;
        }

        if (switchTo < 0)
        {
            switchTo = weapons.Length - 1;
        }

        if (switchTo >= weapons.Length)
        {
            switchTo = 0;
        }

        SwitchToWeapon(switchTo);
    }

    private void Attack()
    {
        if (activeWeapon >= 0)
        {
            weapons[activeWeapon].Attack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();

        if (Input.mouseScrollDelta.y != 0)
        {
            SwitchToWeapon(Input.mouseScrollDelta.y > 0);
        }

        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    private void MovePlayer()
    {
        Vector3 CameraPlayerYpos = new Vector3(mainCamera.transform.position.x, this.transform.position.y, mainCamera.transform.position.z);
        Vector3 forwardDirection = (this.transform.position - CameraPlayerYpos).normalized;
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(forwardDirection * speed * verticalInput, ForceMode.Impulse);

        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(mainCamera.transform.right * speed * horizontalInput, ForceMode.Impulse);
    }

    private void RotatePlayer()
    {
        previousPosition.y = transform.position.y;
        if (transform.position == previousPosition)
        {
            return;
        }

        Vector3 targetDirection = (transform.position - previousPosition).normalized;
        Vector3 lookDirection = Vector3.RotateTowards(transform.position, targetDirection, 10, 0);
        transform.rotation = Quaternion.LookRotation(lookDirection);
        
        previousPosition = transform.position;
    }
}
