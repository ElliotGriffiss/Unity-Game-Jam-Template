using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using CustomDataTypes;

public class CharacterController : MonoBehaviour
{
    public static event Action OnPlayerMove = delegate { };
    public static event Action OnPlayerDeathAnimationTriggered = delegate { };
    public static event Action OnPlayerDeath = delegate { };
    public static event Action OnNeutralTriggerActivated = delegate { };
    public static event Action OnPlayerCompleteLevel = delegate { };

    [Header("Player References")]
    [SerializeField] private CameraFollow camera;
    [SerializeField] private SpriteRenderer Sprite;
    [SerializeField] private Animator Animator;
    [SerializeField] private Rigidbody2D Rigidbody;
    [SerializeField] private Light2D Light;

    [Header("UI")]
    [SerializeField] protected AnimatedHealthBar HealthBar;

    [Header("Settings")]
    [SerializeField] private float health = 10;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float speedHorizontal;
    [SerializeField] private float speedVertical;
    [SerializeField] private float abilityCooldown;
    [SerializeField] private float invTime;
    [Space]
    [SerializeField] private float lightOutRadiusMin;
    [SerializeField] private float lightOutRadiusMax;
    [SerializeField] private float lightHealthReduction;
    [SerializeField] private float lightGrowthModifier;

    private LevelData LevelData;
    private IEnumerator DeathSequence;

    private float CurrentHealth;
    private float currentAbilityCoolDown;
    private float currentInvTime;
    private bool isInvincible;
    private bool PlayerHasControl = true;

    private Vector2 inputValue;
    private Vector2 direction;
    private Vector2 EnemyKnockbackForce;

    private void Start()
    {
        RespawnCharacter();
        camera.UpdateFollowTarget(this.transform, false);
    }

    public void UpdateCurrentLevel(LevelData levelData)
    {
        LevelData = levelData;

        //CurrentPosition = LevelData.SpawnPoint;
        PlayerHasControl = true;

        RespawnCharacter();
    }

    public void RespawnCharacter()
    {
        CurrentHealth = health;
        // animator.SetBool("IsMoving", false);
        UpdateHealth(DamageType.Immediate);
    }

    private void Update()
    {
        if (PlayerHasControl)
        {
            CheckForPlayerInput();
            LookAtMouse();
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

    protected virtual void LookAtMouse()
    {
        if (Time.timeScale > 0)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            direction = mousePos - transform.position;
            Rigidbody.transform.up = direction;
        }
    }

    private void CheckForPlayerInput()
    {
        Vector2 force = Vector2.zero;
        inputValue.x = Input.GetAxisRaw("Horizontal"); //Setting the x and y values of the "movement" var based on what keys are down
        inputValue.y = Input.GetAxisRaw("Vertical"); //^^
        bool lightPressed = Input.GetKey(KeyCode.Space);

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && 0 > currentAbilityCoolDown)
        {
            Rigidbody.AddForce(direction.normalized * chargeSpeed, ForceMode2D.Impulse);
            currentAbilityCoolDown = abilityCooldown;
            Debug.LogError("Move");
        }

        currentAbilityCoolDown -= Time.deltaTime;

        float lightChange;

        if (lightPressed)
        {
            lightChange = Light.pointLightOuterRadius + (lightGrowthModifier * Time.deltaTime);
            CurrentHealth -= lightHealthReduction;
            UpdateHealth(DamageType.LightDamage);
        }
        else
        {
            lightChange = Light.pointLightOuterRadius - (lightGrowthModifier * Time.deltaTime);
            UpdateHealth(DamageType.Immediate);

        }

        float newRadius = Mathf.Clamp(lightChange, lightOutRadiusMin, lightOutRadiusMax);

        Light.pointLightOuterRadius = newRadius;

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
            UpdateHealth(DamageType.Damage);
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

    protected virtual void UpdateHealth(DamageType type)
    {
        if (CurrentHealth > health)
        {
            health = CurrentHealth;
        }

        if (CurrentHealth <= 0)
        {
            TriggerPlayerDeath();
        }

        HealthBar.SetHealth(type, CurrentHealth, health);
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


        while (Light.pointLightInnerRadius > 0)
        {
            Light.pointLightInnerRadius = Light.pointLightInnerRadius - (lightGrowthModifier * Time.deltaTime);
            yield return null;
        }

        while (Light.pointLightOuterRadius > 0)
        {
            Light.pointLightOuterRadius = Light.pointLightOuterRadius - (lightGrowthModifier * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        CharacterController.OnPlayerDeath();
        //CurrentPosition = LevelData.SpawnPoint;
        //PlayerAnimator.SetBool("PlayerDead", false);
        DeathSequence = null;
        PlayerHasControl = true;

    }
}
