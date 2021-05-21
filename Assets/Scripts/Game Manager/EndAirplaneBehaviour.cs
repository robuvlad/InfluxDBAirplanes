using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAirplaneBehaviour : MonoBehaviour
{
    [SerializeField] private StartAirplaneBehaviour startAirplane;

    public StartAirplaneBehaviour StartAirplane => startAirplane;

    private float epsilon = 0.75f;
    private float timeCheckProximity = 1.0f;

    private void Start()
    {
        StartCoroutine(CheckForAirplanesInProximity());
    }

    private IEnumerator CheckForAirplanesInProximity()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeCheckProximity);

            foreach(Transform airplane in startAirplane.transform)
            {
                if (Vector2.Distance(airplane.position, transform.position) <= epsilon) 
                {
                    Destroy(airplane.gameObject);
                }
            }
        }
    }
}
