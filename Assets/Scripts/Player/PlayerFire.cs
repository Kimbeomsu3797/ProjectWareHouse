using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FireState
{
    Rifle,
    Snipe,
    Bomb
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
    [Header("무기")]
    public int poolSize = 15;
    private List<GameObject> bulletEffectPool;
    bool ZoomMode;
    RectTransform roriPos;
    RectTransform soriPos;
    [SerializeField]
    private float delay = 0.075f;
    [Header("애니메이션")]
    Animator anim;
    [Header("무기 이미지")]
    public GameObject rifle;
    public GameObject snipe;
    public GameObject generate;
    public GameObject rifleCrosshair;
    public GameObject snipeCrosshair;
    public GameObject sniper_Zoom;
    public int damage;
    void Start()
    {
        anim = GetComponent<Animator>();
        roriPos = rifleCrosshair.GetComponent<RectTransform>();
        soriPos = snipeCrosshair.GetComponent<RectTransform>();
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
                //float RifleDelay = 0.075f;
                
                if (Input.GetMouseButton(1))
                {
                    float RifleDelay = 0.5f;
                    roriPos.sizeDelta = new Vector2(100, 100);
                    if (Input.GetMouseButton(0) && delay >= RifleDelay)
                    {
                        damage = 1;
                        anim.SetTrigger("Shoot");
                        FireBullet();
                        delay = 0f;
                        //크로스헤어의 크기 증가
                        
                    }
                }
                else
                {
                    float RifleDelay = 0.075f;
                    roriPos.sizeDelta = new Vector2(50, 50);
                    if (Input.GetMouseButton(0) && delay >= RifleDelay)
                    {
                        damage = 5;
                        anim.SetTrigger("Shoot");
                        FireBullet();
                        delay = 0f;
                        //크로스헤어의 크기 원상복구
                        
                    }
                }
                break;
            case FireState.Snipe:
                delay += Time.deltaTime;
                if (Input.GetMouseButtonDown(0) && delay >= 1)
                {
                    damage = 10;
                    FireBullet();
                    delay = 0f;
                    
                }
                if (Input.GetMouseButtonDown(1))
                {
                    if (!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;
                        soriPos.sizeDelta = new Vector2(100, 100);
                        sniper_Zoom.SetActive(true);
                    }
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;
                        soriPos.sizeDelta = new Vector2(50, 50);
                        sniper_Zoom.SetActive(false);
                    }
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
            ZoomMode = false;
            Camera.main.fieldOfView = 60f;

            //UI 이미지 라이플로 변경
            //다른 상태 이미지 제거
            rifle.SetActive(true);
            snipe.SetActive(false);
            generate.SetActive(false);
            sniper_Zoom.SetActive(false);
            currentState = MouseState.Idle;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            state = FireState.Snipe;

            //UI 이미지 스나이프로 변경
            //다른 상태 이미지 제거
            rifle.SetActive(false);
            snipe.SetActive(true);
            generate.SetActive(false);
            roriPos.sizeDelta = new Vector2(50, 50);
            currentState = MouseState.Idle;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            state = FireState.Bomb;

            //UI 이미지 폭탄으로 변경
            //다른 상태 이미지 제거
            rifle.SetActive(false);
            snipe.SetActive(false);
            generate.SetActive(true);
            sniper_Zoom.SetActive(false);
            ZoomMode = false;
            Camera.main.fieldOfView = 60f;
            roriPos.sizeDelta = new Vector2(50, 50);
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
                if (hitInfo.collider.CompareTag("Target"))
                {
                    hitInfo.collider.GetComponent<Target>().Damage();
                }
                else if (hitInfo.collider.CompareTag("Enemy"))
                {
                    hitInfo.collider.GetComponent<EnemyFSM>().HitEnemy(damage);
                }
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
                    //수류탄 생성 + 왼손에 수류탄 자녀로 배치
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

            case MouseState.Fire:// 상태가 Fire로 전환된 후 수행할 작업을 여기에 추가합니다.
                Debug.Log("Fire 상태에 진입했습니다.");
                //수류탄을 자식에서 해제하고 Addforce를 사용하여 투척
                currentState = MouseState.Idle;
                break;
        }
    }

}
