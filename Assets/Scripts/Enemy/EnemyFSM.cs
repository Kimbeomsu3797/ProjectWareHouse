using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        Wait,
        Move,//chase로 바꾸고 wait,move,attack만 남기면 될듯
        Attack,
        Damaged,
        Die,
    }

    public EnemyState m_State;

    public float attackDistance = 2f;

    public float moveSpeed = 3f;

    CharacterController cc;

    Transform player;

    float currentTime = 0;

    float attackDelay = 2f;

    public int attackPower = 3;

    public int hp = 15;
    public int maxHp = 15;
    //public Slider EnemyHpslider;

    Animator anim;

    NavMeshAgent smith;
    public float detectionRange = Mathf.Infinity;
    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Wait;//Wait으로 변경
        player = GameObject.Find("Player").transform;
        cc = GetComponent<CharacterController>();
        maxHp = hp;
        anim = transform.GetComponentInChildren<Animator>();
        smith = GetComponent<NavMeshAgent>();
        smith.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        switch (m_State)
        {
            case EnemyState.Wait:
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
        //EnemyHpslider.value = (float)hp / (float)maxHp;

    }
    void Move()
    {
 
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            smith.isStopped = false;
            smith.ResetPath();
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
                print("공격");
                GameObject.Find("Player").GetComponent<PlayerMove>().DamageAction(attackPower);
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
    public void TestMode()
    {
        m_State = EnemyState.Move;
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