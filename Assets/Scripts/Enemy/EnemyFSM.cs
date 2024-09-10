using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Wait,
        Damaged,
        Die,
    }

    public EnemyState m_State;

    public float findDistance = 8f;

    public float attackDistance = 2f;

    public float moveSpeed = 5f;

    CharacterController cc;

    Transform player;

    float currentTime = 0;

    float attackDelay = 2f;

    Vector3 originPos;
    Quaternion originRot;

    public float moveDistance = 20f;

    public int attackPower = 3;

    public int hp = 15;
    public int maxHp = 15;
    public Slider EnemyHpslider;

    Animator anim;

    NavMeshAgent smith;
    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Wait;
        player = GameObject.Find("Player").transform;
        cc = GetComponent<CharacterController>();
        originPos = transform.position;
        maxHp = hp;
        anim = transform.GetComponentInChildren<Animator>();
        originRot = transform.rotation;
        smith = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
                /*case EnemyState.Damaged:
                    Damaged();
                    break;
                case EnemyState.Die:
                    Die();
                    break;*/
        }
        //EnemyHpslider.value = (float)hp / (float)maxHp;
        // NavMeshAgent의 상태 확인
        if (smith != null)
        {
            // NavMesh 위에 있는지 확인
            if (!smith.isOnNavMesh)
            {
                Debug.Log("NavMesh 위에 없습니다!");
            }

            // 경로가 계산 중인지 확인
            if (smith.pathPending)
            {
                Debug.Log("경로가 아직 계산 중입니다.");
            }
            else
            {
                // 경로가 존재하는지 확인
                if (smith.hasPath)
                {
                    Debug.Log("경로가 존재합니다.");
                    Debug.Log("목적지: " + smith.destination);
                    Debug.Log("현재 위치: " + transform.position);
                    Debug.Log("다음 목적지: " + smith.path.corners[0]); // 다음 경로 지점
                }
                else
                {
                    Debug.Log("경로가 없습니다.");
                }
            }
        }
        Debug.Log("목적지: " + smith.destination);
    }

    public void Idle()
    {

        if (Vector3.Distance(transform.position, player.position) < Mathf.Infinity)
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Idle -> Move");

            anim.SetTrigger("IdleToMove");
        }
    }
    void Move()
    {
        //cc.Move(pos * moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            //Vector3 dir = (player.position - transform.position).normalized;

            //cc.Move(dir * moveSpeed * Time.deltaTime);

            //transform.forward = dir;
            //네비게이션 에이젼트의 이동을 멈추고 경로를 초기화한다.
            smith.isStopped = false;
            float chase = 0; 
            chase += Time.deltaTime;
            if(chase >= 3)
            {
                smith.ResetPath();
                chase = 0;
            }

            anim.SetTrigger("Move");
            //내비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정한다.
            smith.stoppingDistance = attackDistance;
            //네비게이션의 목적지를 플레이어의 위치로 설정한다.
            smith.destination = player.position;
        }
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");
            currentTime = attackDelay;
            anim.SetTrigger("MoveToAttackDelay");
        }

    }
    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {

            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;
                anim.SetTrigger("StartAttack");
            }
        }
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Attack -> Move");
            currentTime = 0;
            anim.SetTrigger("AttackToMove");
        }
    }
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1.0f);

        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }
    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);
    }
    //데미지 실행 함수
    public void HitEnemy(int hitPower)
    {
        //만일, 이미 피격 상태이거나 사망 상태 또는 복귀 상태라면 아무런 처리도 하지 않고 함수를 종료한다.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }
        hp -= hitPower;
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환 : Any state -> Damaged");
            anim.SetTrigger("Damaged");
            Damaged();

        }
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");
            anim.SetTrigger("Die");
            Die();
        }
    }

    [SerializeField]
    private Transform spawnPoint;
    public void SetSpawnPoint(Transform spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }
    //타켓에 디스트로이 관련 함수를 작성하고 플레이어 파이어에서 호출?
    void OnDestroy()
    {
        if (spawnPoint != null)
        {
            Spawnpoint spm = FindObjectOfType<Spawnpoint>();
            if (spm != null)
            {
                spm.AddSpawnPoint(spawnPoint); // 스폰 포인트 다시 활성화
                Debug.Log("Spawn point reactivated: " + spawnPoint.name);
            }
            else
            {
                Debug.LogWarning("Spawnpoint not found!");
            }
        }
        UIManager.ins.emaxvalue++;
    }

}