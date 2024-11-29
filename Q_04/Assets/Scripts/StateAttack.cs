using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateAttack : PlayerState
{
    private float _delay = 2;
    private WaitForSeconds _wait;
    Coroutine _routine;

    public StateAttack(PlayerController controller) : base(controller)
    {
        
    }

    public override void Init()
    {
        _wait = new WaitForSeconds(_delay);
        ThisType = StateType.Attack;
    }

    public override void Enter()
    {
        _routine = Controller.StartCoroutine(DelayRoutine(Attack));
    }

    public override void OnUpdate()
    {
        Debug.Log("Attack On Update");
    }

    public override void Exit()
    {
        Controller.StopCoroutine(_routine);
    }

    private void Attack()
    {
        Collider[] cols = Physics.OverlapSphere(
            Controller.transform.position,
            Controller.AttackRadius
            );

        IDamagable damagable;
        foreach (Collider col in cols)
        {
            damagable = col.GetComponent<IDamagable>();

            if (damagable == null)
            {
                Machine.ChangeState(StateType.Idle);
                return;
            }

            damagable.TakeHit(Controller.AttackValue);
        }
    }

    public IEnumerator DelayRoutine(Action action)
    {
        yield return _wait;

        Attack();
        Exit();
    }

}
