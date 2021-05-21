using Assets.Scripts.Domain;
using Assets.Scripts.Game_Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteInitializerBehaviour : MonoBehaviour
{
    void Start()
    {
        GameController.Instance.DropAllMeasurements();
    }
}
