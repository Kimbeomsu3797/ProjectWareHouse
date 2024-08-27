using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FireState
{
    Rifle,
    Snipe,
    Bomb
}
enum bombState
{
    start,
    release,
    fire
}

public class PlayerFire : MonoBehaviour
{
    private enum MouseState
    {
        Idle,   // 마우스 클릭 없음
        Start,  // 클릭 시작
        Release,// 클릭 유지 중
        Fire    // 클릭 해제
    }
    FireState state = FireState.Rifle;
    public GameObject bulletEffectPrefab; // Prefab으로서의 총알 이펙트
    public int poolSize = 15;
    private List<GameObject> bulletEffectPool;
    private float delay = 0.075f;

    void Start()
    {
        
        // 풀 초기화
        bulletEffectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletEffectPrefab);
            bullet.SetActive(false); // 처음에는 비활성화
            bulletEffectPool.Add(bullet);
        }
    }

    void Update()
    {
        switch (state)
        {
            case FireState.Rifle:
                delay += Time.deltaTime;
                if (Input.GetMouseButton(0) && delay >= 0.075f)
                {
                    FireBullet();
                    delay = 0f;
                    
                }
                //우클릭을 유지하면서 좌클릭 시 딜레이 속도 증가
                break;
            case FireState.Snipe:
                delay += Time.deltaTime;
                if (Input.GetMouseButtonDown(0) && delay >= 1)
                {
                    FireBullet();
                    delay = 0f;
                }
                //우클릭 시 줌 모드 + 좌클릭 딜레이 증가
                break;
            case FireState.Bomb:
                bulletFire();
                break;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            state = FireState.Rifle;
            //다른 상태에서의 변화 해제
            //bombaction상태 idle로 변경
            //UI 이미지 라이플로 변경
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            state = FireState.Snipe;
            //다른 상태에서의 변화 해제
            //bombaction상태 idle로 변경
            //UI 이미지 스나이프로 변경
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            state = FireState.Bomb;
            //다른 상태에서의 변화 해제
            //bombaction상태 idle로 변경
            //UI 이미지 폭탄으로 변경
        }
    }

    void FireBullet()
    {
        // 레이를 생성한 후 발사될 위치와 진행 방향을 설정한다.
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        // 레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다.
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject bullet = GetPooledBullet();
            if (bullet != null)
            {
                // 피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킨다.
                bullet.transform.position = hitInfo.point;
                // 피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다.
                bullet.transform.forward = hitInfo.normal;
                // 피격 이펙트 활성화
                bullet.SetActive(true);
                bullet.GetComponent<ParticleSystem>().Play();
                // 일정 시간 후 비활성화
                StartCoroutine(DeactivateBullet(bullet, bullet.GetComponent<ParticleSystem>().main.duration));
            }
        }
    }

    GameObject GetPooledBullet()
    {
        foreach (GameObject bullet in bulletEffectPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        // 모든 총알이 활성화 상태라면 새로운 총알을 생성할 수 있지만, 
        // 여기서는 풀에 총알이 없을 때의 처리를 생략합니다.
        return null;
    }

    IEnumerator DeactivateBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }
    private MouseState currentState = MouseState.Idle;
    public void bulletFire()
    {
        switch (currentState)
        {
            case MouseState.Idle:
                if (Input.GetMouseButtonDown(0)) // 좌클릭이 눌린 경우
                {
                    Debug.Log("모션이 시작됩니다.");
                    currentState = MouseState.Start;
                }
                break;

            case MouseState.Start:
                if (Input.GetMouseButton(0)) // 좌클릭이 계속 눌려진 경우
                {
                    Debug.Log("릴리즈 상태에 진입합니다");
                    currentState = MouseState.Release;
                }
                else if (Input.GetMouseButtonUp(0)) // 좌클릭이 해제된 경우
                {
                    currentState = MouseState.Fire;
                }
                break;

            case MouseState.Release:
                if (Input.GetMouseButtonUp(0)) // 좌클릭이 해제된 경우
                {
                    currentState = MouseState.Fire;
                }
                break;

            case MouseState.Fire:
                // 상태가 Fire로 전환된 후 수행할 작업을 여기에 추가합니다.
                Debug.Log("Fire 상태에 진입했습니다.");
                // 상태를 Idle로 리셋하여 마우스 클릭을 다시 시작할 수 있게 함
                currentState = MouseState.Idle;
                break;
        }
    }
}
