using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    [SerializeField] Transform targetTr;
    [SerializeField] Transform monsterTr;

    NavMeshAgent agent;

    float distance;

    public float currentHp = 100;
    public float maxHp = 100;

    enum State
    {
        IDLE,
        TRACE,
        TRACE_RUN,
        ATTACK,
        DIE
    }

    State state = State.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(CheckMosnterState());   
        StartCoroutine(MonsterAction());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator CheckMosnterState()
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

    IEnumerator MonsterAction()
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

}
