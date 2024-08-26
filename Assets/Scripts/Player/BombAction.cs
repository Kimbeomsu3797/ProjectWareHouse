using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum bombState
{
    start,
    release,
    fire
}

public class BombAction : MonoBehaviour
{
    private enum MouseState
    {
        Idle,   // 마우스 클릭 없음
        Start,  // 클릭 시작
        Release,// 클릭 유지 중
        Fire    // 클릭 해제
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 현재 상태를 저장하는 변수
    private MouseState currentState = MouseState.Idle;

    void Update()
    {
        switch (currentState)
        {
            case MouseState.Idle:
                if (Input.GetMouseButtonDown(0))
                {
                    currentState = MouseState.Start;
                }
                break;

            case MouseState.Start:
                if (Input.GetMouseButton(0))
                {
                    currentState = MouseState.Release;
                    //팔을 뒤로 넘기는 애니메이션 실행 후 릴리즈로 넘어가기
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    currentState = MouseState.Fire;
                    //팔을 뒤로 넘기는 애니메이션 실행 후 파이어로 넘어가기
                }
                break;

            case MouseState.Release:
                if (Input.GetMouseButtonUp(0))
                {
                    currentState = MouseState.Fire;
                }
                else
                {
                    //팔을 뒤로 넘기는 애니메이션 실행 후 유지하기
                }
                break;

            case MouseState.Fire:
                //수류탄을 던지는 애니메이션을 실행하기
                //수류탄을 던지기
                currentState = MouseState.Idle;
                break;
        }
    }
}
