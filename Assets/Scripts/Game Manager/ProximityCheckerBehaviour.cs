using Assets.Scripts.Domain;
using Assets.Scripts.Game_Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityCheckerBehaviour : MonoBehaviour
{
    [SerializeField] private float repeteadTime;
    [SerializeField] private float epsilonDistance;
    private float timeElapsed;

    private void Start()
    {
        timeElapsed = 0.0f;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= repeteadTime)
        {
            CheckAirplanesInProximity();
            timeElapsed = 0.0f;
        }
    }

    private void CheckAirplanesInProximity()
    {
        var airplanes = FindObjectsOfType<AirplaneBehaviour>();

        foreach (var airplane in airplanes)
        {
            foreach (var otherAirplane in airplanes)
            {
                if (!airplane.Equals(otherAirplane))
                {
                    var distance = Vector2.Distance(airplane.transform.position, otherAirplane.transform.position);
                    if (distance <= epsilonDistance)
                    {
                        ProximityAirplane proximityAirplane = new ProximityAirplane()
                        {
                            Timestamp = DateTime.UtcNow,
                            BaseAirplaneName = airplane.name,
                            OtherAirplaneName = otherAirplane.name,
                            Distance = distance
                        };

                        //_ = GameController.Instance.WriteProximityAirplanes(new ProximityAirplane[] { proximityAirplane });

                        StartCoroutine(StartDrawingCircle(airplane, 200, 2.0f, airplane.transform.position));
                        StartCoroutine(StartDrawingCircle(otherAirplane, 200, 2.0f, otherAirplane.transform.position));
                    }
                }
            }
        }
    }

    private IEnumerator StartDrawingCircle(AirplaneBehaviour airplane, int vertexNumber, float radius, Vector3 centerPos)
    {
        DrawCircleAroundAirplane(airplane, vertexNumber, radius, centerPos);

        yield return new WaitForSeconds(1.5f);

        DestroyLineRenderer(airplane);
    }

    private void DrawCircleAroundAirplane(AirplaneBehaviour airplane, int vertexNumber, float radius, Vector3 centerPos)
    {
        var lineRenderer = airplane.GetComponent<LineRenderer>();
        var startWidth = 0.2f;
        var endWidth = 0.2f;

        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.loop = true;
        float angle = 2 * Mathf.PI / vertexNumber;
        lineRenderer.positionCount = vertexNumber;

        for (int i = 0; i < vertexNumber; i++)
        {
            Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                                     new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                       new Vector4(0, 0, 1, 0),
                                       new Vector4(0, 0, 0, 1));
            Vector3 initialRelativePosition = new Vector3(0, radius, 0);
            lineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));
        }
    }

    private void DestroyLineRenderer(AirplaneBehaviour airplane)
    {
        try
        {
            var lineRenderer = airplane.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }
        catch (MissingReferenceException _) { }
    }
}
