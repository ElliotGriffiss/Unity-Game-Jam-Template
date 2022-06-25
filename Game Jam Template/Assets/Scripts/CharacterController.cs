using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using CustomDataTypes;

public class CharacterController : MonoBehaviour
{
    public static event Action OnPlayerMove = delegate { };
    public static event Action OnPlayerDeathAnimationTriggered = delegate { };
    public static event Action OnPlayerDeath = delegate { };
    public static event Action OnNeutralTriggerActivated = delegate { };
    public static event Action OnPlayerCompleteLevel = delegate { };

    [Header("Player References")]
    [SerializeField] protected SpriteRenderer Sprite;
    [SerializeField] private Animator Animator;
    [SerializeField] protected Rigidbody2D Rigidbody;
    protected Vector3Int CurrentPosition;


    [Header("Settings")]
    [SerializeField] protected float health = 10;
    [SerializeField] private float chargeSpeed;
    [SerializeField] protected float speedHorizontal;
    [SerializeField] protected float speedVertical;
    [SerializeField] protected float invTime;

    private LevelData LevelData;
    private IEnumerator DeathSequence;

    protected float CurrentHealth;
    protected float currentInvTime;
    protected bool isInvincible;
    private bool PlayerHasControl = true;

    protected Vector2 inputValue;
    protected Vector2 direction;
    protected Vector2 EnemyKnockbackForce;

    public void UpdateCurrentLevel(LevelData levelData)
    {
        LevelData = levelData;

        CurrentPosition = LevelData.SpawnPoint;
        PlayerHasControl = true;
    }

    public void RespawnCharacter()
    {
        CurrentHealth = health;
        // animator.SetBool("IsMoving", false);
    }

    private void Update()
    {
        if (PlayerHasControl)
        {
            CheckForPlayerInput();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (EnemyKnockbackForce != Vector2.zero)
        {
            Rigidbody.AddForce(EnemyKnockbackForce, ForceMode2D.Impulse);
            EnemyKnockbackForce = Vector2.zero;
        }
    }

    private void CheckForPlayerInput()
    {
        Vector2 force = Vector2.zero;

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // move forward 
            Rigidbody.AddForce(direction.normalized * chargeSpeed, ForceMode2D.Impulse);
        }

        if (inputValue.y != 0f)
        {
            //animator.SetBool("IsMoving", true);
            force += Vector2.up * inputValue.y * speedVertical;
        }

        if (inputValue.x != 0)
        {
            //animator.SetBool("IsMoving", true);
            force += (Vector2.right * inputValue.x * speedHorizontal);
        }

        inputValue.x = Input.GetAxisRaw("Horizontal"); //Setting the x and y values of the "movement" var based on what keys are down
        inputValue.y = Input.GetAxisRaw("Vertical"); //^^

        Rigidbody.AddForce(force);
        Invincible();
    }

    public virtual void HandleCollisonEnter(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isInvincible)
        {
            DamageComponent damage = collision.collider.GetComponent<DamageComponent>();

            isInvincible = true;
            currentInvTime = invTime;

            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = 0f;
            EnemyKnockbackForce = (transform.position - collision.transform.position).normalized * damage.KnockBackForce;
            CurrentHealth -= damage.Damage;
        }

        if (CurrentHealth <= 0)
        {
            TriggerPlayerDeath();
        }
    }

    protected virtual void Invincible()
    {
        if (isInvincible == true)
        {
            currentInvTime -= Time.deltaTime;
        }
        if (currentInvTime <= 0)
        {
            isInvincible = false;
        }
    }

    private void UpdateCharacterUI()
    {

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
        //PlayerAnimator.SetBool("PlayerDead", false);
        DeathSequence = null;
        PlayerHasControl = true;
    }
}
