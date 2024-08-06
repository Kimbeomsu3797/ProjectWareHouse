using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    float h;
    float z;
    float playerSpeed = 5f;

    CharacterController cC;
    float gravityForce = -15f;
    float yvelocity = 0;
    public float jumpPower = 10f;
    public bool isJumping = false;
    public int hp = 20;
    int maxHp = 20;
    public Slider hpSlider;
    public GameObject hitEffect;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        cC = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        #region
        h = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, z);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        transform.position += dir * playerSpeed * Time.deltaTime;
        anim.SetFloat("Float", dir.magnitude);
        if(isJumping && cC.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yvelocity = 0;
        }
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            yvelocity = jumpPower;
            isJumping = true;
        }
        yvelocity += gravityForce * Time.deltaTime;
        dir.y = yvelocity;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            cC.Move(dir * playerSpeed/2 * Time.deltaTime);
        }
        else
        {
            cC.Move(dir * playerSpeed * Time.deltaTime);
        }
        #endregion
        // hpSlider.value = (float)hp / (float)maxHp;
    }

}
