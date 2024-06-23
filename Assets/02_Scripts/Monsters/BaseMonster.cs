using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseMonster : MonoBehaviour
{
    [SerializeField] protected Transform targetTr = null;
    [SerializeField] protected Transform monsterTr = null;

    protected NavMeshAgent agent = null;

    protected float distance;

    public float currentHp = 100;
    public float maxHp = 100;

    protected enum State
    {
        IDLE,
        TRACE,
        TRACE_RUN,
        ATTACK,
        DIE
    }

    protected State state = State.IDLE;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected abstract IEnumerator CheckMonsterState();

    protected abstract IEnumerator MonsterAction();

    public abstract void Hit(float damage);

}