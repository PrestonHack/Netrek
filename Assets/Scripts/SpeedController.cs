using UnityEngine;
using System.Text.RegularExpressions;
using System;


public class SpeedController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private float tickTimer;
    [SerializeField]
    private float nextTime;
    [SerializeField]
    public float warpSpeed;
    public float moveSpeed;
    public float rotationSpeed;
    public float speedPercent;
    public float currentSpeed;
    public float desiredSpeed;
    public float allowedSpeed;
    public float accelRate;
    public float deccelRate;
    public float turnRate;
    public float maxSpeed;
    private float maxSpeedAllowed;

    private void Start()
    {
        tickTimer = 0.1f;
        currentSpeed = 0;
        rotationSpeed = turnRate;
        playerController = GetComponentInParent<PlayerController>();
        maxSpeedAllowed = maxSpeed;
    }

    private void Update()
    {
        speedPercent = (currentSpeed / maxSpeed);

        if (Regex.IsMatch(Input.inputString, "[0-9)!@]"))
        {
            if (Input.inputString == "@")
            {
                desiredSpeed = 12;
            }
            else if (Input.inputString == "!")
            {
                desiredSpeed = 11;
            }
            else if (Input.inputString == ")")
            {
                desiredSpeed = 10;
            }
            else
            {
                desiredSpeed = float.Parse(Input.inputString);
            }

            if (desiredSpeed > maxSpeed)
            {
                desiredSpeed = maxSpeed;
            }
        }

        if (Time.time > nextTime)
        {
            nextTime = Time.time + tickTimer;
            if (desiredSpeed > currentSpeed)
            {
                currentSpeed += accelRate / 1000;
            }
            else if (desiredSpeed < currentSpeed)
            {
                currentSpeed -= deccelRate / 1000;
            }
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        currentSpeed = (float)Math.Round(currentSpeed, 1);
        moveSpeed = currentSpeed * warpSpeed;
        rotationSpeed = turnRate / (currentSpeed * currentSpeed);

        if (playerController.fuelController.currentFuel < playerController.warpFuelUse)
        {
            if (playerController.fuelController.rechargeRate / playerController.warpCost < desiredSpeed)
            {
                allowedSpeed = playerController.fuelController.rechargeRate / playerController.warpCost;
                allowedSpeed = (float)Math.Truncate((decimal)allowedSpeed);
                desiredSpeed = allowedSpeed;
            }
        }
        else
        {
            allowedSpeed = maxSpeedAllowed;
        }

        if (playerController.hullController.hullHealth < playerController.hullController.hullMaxHealth)
        {
            maxSpeedAllowed = maxSpeed * playerController.hullController.hullHealth / playerController.hullController.hullMaxHealth;
            maxSpeedAllowed = (float)Math.Truncate((decimal)maxSpeedAllowed);
        }
        else
        {
            maxSpeedAllowed = maxSpeed;
        }

        if (desiredSpeed > maxSpeedAllowed)
        {
            desiredSpeed = maxSpeedAllowed;
        }

        playerController.maxWarp = maxSpeedAllowed;

    }

}
