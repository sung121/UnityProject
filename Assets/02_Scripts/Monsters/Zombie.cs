using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : BaseMonster
{
    protected override IEnumerator CheckMonsterState()
    {
        while (true)
        {
            distance = (targetTr.position - monsterTr.position).magnitude;

            if (distance > 100)
            {
                state = State.IDLE;
            }
            else if (distance < 100)
            {
                state = State.TRACE;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    public override void Hit(float damage)
    {
        currentHp -= damage;
    }

    protected override IEnumerator MonsterAction()
    {
        while (true)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;

                    break;
                case State.TRACE:
                    agent.SetDestination(targetTr.position);

                    break;
                case State.TRACE_RUN:
                    agent.SetDestination(targetTr.position);

                    break;
                case State.ATTACK:

                    break;
                case State.DIE:
                    break;

            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    // Update is called once per frame
    override protected void Update()
    {
        
    }
}
