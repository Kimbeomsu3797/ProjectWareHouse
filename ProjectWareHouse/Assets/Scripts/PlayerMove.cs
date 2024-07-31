using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController cC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizon");
        float v = Input.GetAxis("Vertical");
        Vector3 movePos = transform.forward * h + transform.right * v;
        cC.Move(movePos * Time.deltaTime);
        
    }
}
