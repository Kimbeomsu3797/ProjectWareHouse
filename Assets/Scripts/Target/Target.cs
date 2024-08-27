using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
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
}
