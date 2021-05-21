using Assets.Scripts.Domain;
using Assets.Scripts.Game_Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneManager : MonoBehaviour
{
    [SerializeField] private Transform planePrefab;
    private StartAirplaneBehaviour startAirplane;
    private static int id = 0;

    private void Start()
    {
        startAirplane = transform.parent.GetComponent<StartAirplaneBehaviour>();

        if (startAirplane == null)
            Debug.LogWarning($"Start Airplane is null: {startAirplane}");
    }

    private void OnMouseUp()
    {
        float rotationZ = GetZRotation(startAirplane);
        var airplaneInstantiated = Instantiate(
            planePrefab,
            startAirplane.transform.position,
            Quaternion.Euler(0.0f, 0.0f, rotationZ),
            startAirplane.transform);

        var airplaneBehaviour = airplaneInstantiated.GetComponent<AirplaneBehaviour>();
        if (airplaneBehaviour == null)
        {
            Debug.LogWarning("Airplane Behaviour is null.");
            return;
        }

        airplaneBehaviour.Airplane = new Airplane()
        {
            Timestamp = DateTime.UtcNow,
            Id = id,
            Name = airplaneInstantiated.name,
            StartLocation = startAirplane.name,
            EndLocation = startAirplane.EndAirplane.name
        };

        id += 1;
    }

    private float GetZRotation(StartAirplaneBehaviour startAirplane)
    {
        Vector3 start = startAirplane.transform.position;
        Vector3 end = startAirplane.EndAirplane.transform.position;
        Vector3 direction = start - end;
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * (Mathf.Rad2Deg);

        return rotationZ;
    }
}
