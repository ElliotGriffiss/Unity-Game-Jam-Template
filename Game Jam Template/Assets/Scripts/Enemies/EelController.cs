using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EelController : MonoBehaviour
{
    [Header("Objects To Assign")]
    [SerializeField] private Transform Eel;
    [SerializeField] private Transform[] Locations;

    [Header("Settings")]
    [SerializeField] private int StartingLocation = 0;
    [SerializeField] private float EelSpeed = 1f;

    [Header("Public Fields")]
    public bool IsActive = true;
    // Added this so we can sequence a bunch of platforms together at the same time  and ensure they stay in sync

    // maybe re-work this to use time instead of speed, might make it easier to manage
    private float startTime;
    private int currentLocationIndex = 0;
    private int targetLocationIndex = 0;
    private float journeyLenght;

    private void Start()
    {
        ResetToDefaultPosition();
    }

    /// <summary>
    /// Used to determin current location and destination
    /// </summary>
    private void UpdatePlatformTargets()
    {
        currentLocationIndex++;

        if (currentLocationIndex > Locations.Length - 1)
        {
            currentLocationIndex = 0;
        }

        targetLocationIndex++;

        if (targetLocationIndex > Locations.Length - 1)
        {
            targetLocationIndex = 0;
        }

        startTime = Time.time;
        journeyLenght = Vector3.Distance(Locations[currentLocationIndex].position, Locations[targetLocationIndex].position);

        if (Locations[currentLocationIndex].position.x - Locations[targetLocationIndex].position.x > 0)
        {
            Eel.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            Eel.transform.localScale = new Vector3(-1, 1, 1);
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

                Eel.position = Vector3.Lerp(Locations[currentLocationIndex].position, Locations[targetLocationIndex].position, fractionOfJourney);
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
