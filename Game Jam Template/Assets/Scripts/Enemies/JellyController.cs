using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDataTypes;

public class JellyController : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D MyRigidBody;
    [Space]
    [SerializeField] protected EnemyState State = EnemyState.Idle;
    [SerializeField] protected float MovementSpeed = 3;
    [SerializeField] protected float StateDuration;

    protected Vector2 movementDirection;
    protected float currentStateTime = float.PositiveInfinity; // ensures a new state is always chosen

    protected virtual void FixedUpdate()
    {
        if (currentStateTime > StateDuration)
        {
            ChooseANewState();
        }
        else
        {
            if (State != EnemyState.Idle)
            {
                MyRigidBody.AddForce(movementDirection * MovementSpeed);
            }

            currentStateTime += Time.fixedDeltaTime;
        }
    }

    protected virtual void ChooseANewState()
    {
        if (UnityEngine.Random.Range(100, 0) > 30)
        {
            State = EnemyState.Moving;
            //Sprite.color = Color.green;
            movementDirection = GenerateRandomMovementVector();
        }
        else
        {
            // Sprite.color = Color.blue;
            State = EnemyState.Idle;
        }

        currentStateTime = 0;
    }

    protected Vector2 GenerateRandomMovementVector()
    {
        return new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
    }
}
