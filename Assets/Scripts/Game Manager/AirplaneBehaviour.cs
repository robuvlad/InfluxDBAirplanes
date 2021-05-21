using Assets.Scripts.Domain;
using Assets.Scripts.Game_Controller;
using System;
using UnityEngine;

public class AirplaneBehaviour : MonoBehaviour
{
    private Transform startLocation, endLocation;

    private const int MIN_SPEED = 600;
    private const int MAX_SPEED = 1200;
    private const float INTERPOLATED_MIN_SPEED = 0.15f;
    private const float INTERPOLATED_MAX_SPEED = 0.75f;

    private const int MIN_ALTITUDE = 7000;
    private const int MAX_ALTITUDE = 10000;

    private float unitySpeed;

    private float reloadTime = 0.5f;
    private float elapsedTime;

    private Airplane airplane;
    public Airplane Airplane
    {
        get { return airplane; }
        set
        {
            airplane = value;
            airplane.Speed = GetRandomSpeed;
            airplane.Altitude = GetRandomAltitude;

            unitySpeed = GetInterpolatedSpeed(airplane.Speed);

            _ = GameController.Instance.WriteAirplaneAsync(new Airplane[] { airplane });
        }
    }

    private void Start()
    {
        startLocation = GameObject.Find(airplane.StartLocation).transform;
        endLocation = GameObject.Find(airplane.EndLocation).transform;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= reloadTime)
        {
            InsertPositionsIntoStream();
            InsertProgressIntoStream();
            elapsedTime = 0.0f;
        }

        if (!startLocation || !endLocation) { return; }

        float step = unitySpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, endLocation.position, step);
    }

    private int GetRandomSpeed => UnityEngine.Random.Range(MIN_SPEED, MAX_SPEED);

    private int GetRandomAltitude => UnityEngine.Random.Range(MIN_ALTITUDE, MAX_ALTITUDE);

    private float GetInterpolatedSpeed(int value)
    {
        float normal = Mathf.InverseLerp(MIN_SPEED, MAX_SPEED, value);
        float mapped = Mathf.Lerp(INTERPOLATED_MIN_SPEED, INTERPOLATED_MAX_SPEED, normal);
        return mapped;
    }

    private void InsertPositionsIntoStream()
    {
        AirplanePosition airplanePosition = new AirplanePosition()
        {
            Timestamp = DateTime.UtcNow,
            Id = airplane.Id,
            Name = airplane.Name,
            X_Position = transform.position.x,
            Y_Position = transform.position.y
        };
        _ = GameController.Instance.WriteAirplanePositionAsync(new AirplanePosition[] { airplanePosition });
    }

    private void InsertProgressIntoStream()
    {
        AirplaneProgress airplaneProgress = new AirplaneProgress()
        {
            Timestamp = DateTime.UtcNow,
            Id = airplane.Id,
            Name = airplane.Name,
            Progress = GetCurrentProgress()
        };
        _ = GameController.Instance.WriteAirplaneProgressAsync(new AirplaneProgress[] { airplaneProgress });
    }

    private int GetCurrentProgress()
    {
        var totalDistance = Vector2.Distance(endLocation.transform.position, startLocation.transform.position);
        var currentDistance = Vector2.Distance(transform.position, startLocation.transform.position);

        if (currentDistance > totalDistance)
        {
            Debug.LogWarning($"Current Distance {currentDistance} is greather than Total Distance {totalDistance}");
        }

        int progress = (int)((currentDistance / totalDistance) * 100);

        return progress;
    }
}
