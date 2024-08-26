using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    Animator anim;
    Transform transform;
    private Vector3 moveDir;

    PlayerInput playerInput;
    InputActionMap mainActionMap;
    InputAction moveAction;
    InputAction attackAction;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        playerInput = GetComponent<PlayerInput>();
        //ActionMap 추출
        mainActionMap = playerInput.actions.FindActionMap("Player");
        //Move, Attack 액션 추출
        moveAction = mainActionMap.FindAction("Move");
        //attackAction = mainActionMap.FindAction("Attack");

        //Move 액션의 performed 이벤트 연결
        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            //애니메이션 실행
            anim.SetFloat("Float", dir.magnitude);
        };

        //Move 액션의 canceled 이벤트 연결//키보드 버튼이 눌리지 않을 때
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;

            anim.SetFloat("Float", 0.0f);
        };

        //Attack 액션의 performed 이벤트 연결
        /*attackAction.performed += ctx =>
        {
            Debug.Log("Attack by c# event");
            anim.SetTrigger("Attack");
        };*/
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }

    }
}
