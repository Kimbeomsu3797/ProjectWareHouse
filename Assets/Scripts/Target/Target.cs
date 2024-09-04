using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //과녁에 총알이 충돌했다면 (particlecollider와 충돌했다면)
        //본인 파괴 + 점수 획득
        //본인이 파괴될 때 스폰포인트 다시 활성화
    }
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
            }
        }
    }
}
