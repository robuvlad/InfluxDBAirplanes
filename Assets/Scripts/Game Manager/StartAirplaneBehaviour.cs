using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAirplaneBehaviour : MonoBehaviour
{
    [SerializeField] private EndAirplaneBehaviour endAirplane;

    public EndAirplaneBehaviour EndAirplane => endAirplane;
}
