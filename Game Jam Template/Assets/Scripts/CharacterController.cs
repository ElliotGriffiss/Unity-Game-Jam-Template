using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CustomDataTypes;

public class CharacterController : MonoBehaviour
{
    public static event Action OnPlayerMove = delegate { };
    public static event Action OnPlayerDeathAnimationTriggered = delegate { };
    public static event Action OnPlayerDeath = delegate { };
    public static event Action OnNeutralTriggerActivated = delegate { };
    public static event Action OnPlayerCompleteLevel = delegate { };

    [Header("Player References")]
    [SerializeField] private Animator PlayerAnimator;
    protected Vector3Int CurrentPosition;

    private LevelData LevelData;
    private IEnumerator DeathSequence;
    private bool PlayerHasControl = false;

    public void UpdateCurrentLevel(LevelData levelData)
    {
        LevelData = levelData;

        CurrentPosition = LevelData.SpawnPoint;
        PlayerAnimator.SetBool("PlayerDead", false);
        PlayerHasControl = true;

        // This just makes sure everything is reset properly when a level loads.
        CharacterController.OnPlayerDeath();
    }

    private void Update()
    {
        if (PlayerHasControl)
        {
            CheckForPlayerInput();
        }
    }

    private void CheckForPlayerInput()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {

        }
        else if (Input.GetAxis("Horizontal") > 0)
        {

        }
        else if (Input.GetAxis("Vertical") > 0)
        {

        }
        else if (Input.GetAxis("Vertical") < 0)
        {

        }
    }

    private void TriggerPlayerDeath()
    {
        if (DeathSequence == null)
        {
            DeathSequence = PlayDeathSequence();
            StartCoroutine(DeathSequence);
        }
    }

    private IEnumerator PlayDeathSequence()
    {
        PlayerHasControl = false;
        CharacterController.OnPlayerDeathAnimationTriggered();
        yield return new WaitForSeconds(1f);

        CharacterController.OnPlayerDeath();
        CurrentPosition = LevelData.SpawnPoint;
        PlayerAnimator.SetBool("PlayerDead", false);
        DeathSequence = null;
        PlayerHasControl = true;
    }
}
