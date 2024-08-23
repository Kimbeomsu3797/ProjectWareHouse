using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
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
        delay += Time.deltaTime;
        if (Input.GetMouseButton(0) && delay >= 0.075f)
        {
            FireBullet();
            delay = 0f;
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
}
