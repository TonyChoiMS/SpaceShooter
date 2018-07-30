using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    // Enemy State enum
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    // save state variable
    public State state = State.PATROL;


    Transform m_trPlayer;       // player transfrom
    Transform m_trEnemy;        // Enemy transform
    Animator animator;

    public float m_flAttackDist = 5.0f;
    public float m_flTraceDist = 10.0f;

    public bool m_bIsDie = false;

    WaitForSeconds ws;

    MoveAgent m_moveAgent;

    readonly int hashMove = Animator.StringToHash("IsMove");
    readonly int hashSpeed = Animator.StringToHash("Speed");

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        if (player != null)
        {
            m_trPlayer = player.GetComponent<Transform>();
        }

        m_trEnemy = GetComponent<Transform>();
        m_moveAgent = GetComponent<MoveAgent>();
        animator = GetComponent<Animator>();

        ws = new WaitForSeconds(0.3f);
    }

    void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        while (!m_bIsDie)
        {
            if (state == State.DIE) yield break;

            float dist = Vector3.Distance(m_trPlayer.position, m_trEnemy.position);

            if (dist <= m_flAttackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= m_flTraceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while (!m_bIsDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    m_moveAgent.Patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    m_moveAgent.traceTarget = m_trPlayer.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    m_moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    break;
                case State.DIE:
                    m_moveAgent.Stop();
                    break;
            }
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        animator.SetFloat(hashSpeed, m_moveAgent.speed);
	}
}
