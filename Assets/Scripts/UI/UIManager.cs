using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    InputField targetValue;
    InputField EnemyValue;
    // Start is called before the first frame update
    void Start()
    {
        //인풋 필드에 값이 입력되었을 때 그 값을 받아서 타겟 생성 or enemy 생성
        //연습 모드 시작 시 enemy가 IDLE상태로 진입
        //스폰포인트를 리스트로 관리하여 과녁이 생성된 스폰포인트를 리스트에서 제외
        //과녁이 생성될 때 스폰포인트를 비활성화
        //스폰포인트가 다시 활성화 될 때 리스트에 다시 추가
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
