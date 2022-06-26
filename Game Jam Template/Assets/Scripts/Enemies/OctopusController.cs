using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusController : MonoBehaviour
{
    [Header("Objects To Assign")]
    [SerializeField] private Transform Octo;
    [SerializeField] private Transform[] Locations;

    [Header("Settings")]
    [SerializeField] private int StartingLocation = 0;
    [SerializeField] private float EelSpeed = 1f;

    [Header("Public Fields")]
    public bool IsActive = true;

    private float startTime;
    private int currentLocationIndex = 0;
    private int targetLocationIndex = 0;
    private float journeyLenght;


    // Start is called before the first frame update
    void Start()
    {
        ResetToDefaultPosition();
    }

    /// Used to determin current location and destination
    /// </summary>
    private void UpdatePlatformTargets()
    {
        currentLocationIndex = 0;

        targetLocationIndex = 1;

        Octo.position = Locations[currentLocationIndex].position;

        startTime = Time.time;
        journeyLenght = Vector3.Distance(Locations[currentLocationIndex].position, Locations[targetLocationIndex].position);

        if (Locations[currentLocationIndex].position.x - Locations[targetLocationIndex].position.x > 0)
        {
            Octo.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            Octo.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            float distanceCovered = (Time.time - startTime) * EelSpeed;

            if (distanceCovered < journeyLenght)
            {
                float fractionOfJourney = distanceCovered / journeyLenght;

                Octo.position = Vector3.Lerp(Locations[currentLocationIndex].position, Locations[targetLocationIndex].position, fractionOfJourney);
            }
            else
            {
                UpdatePlatformTargets();
            }
        }
    }

    /// <summary>
    /// Resets the platform to it's default position.
    /// </summary>
    public void ResetToDefaultPosition(bool turnOff = false)
    {
        currentLocationIndex = StartingLocation - 1;  // this is stupid, but it prevents duplicate code
        targetLocationIndex = StartingLocation;
        UpdatePlatformTargets();
    }
}
