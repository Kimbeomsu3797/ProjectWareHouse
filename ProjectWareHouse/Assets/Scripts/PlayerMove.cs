using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController cC;
    float gravityForce = -20f;
    bool isGround = true;
    // Start is called before the first frame update
    void Start()
    {
        cC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 movePos = transform.forward * h + transform.right * v;
        cC.Move(movePos * Time.deltaTime);
        if (cC.collisionFlags==CollisionFlags.Below)
        {
            isGround = true;
        }
        else if (!isGround)
        {

        }
    }
}
