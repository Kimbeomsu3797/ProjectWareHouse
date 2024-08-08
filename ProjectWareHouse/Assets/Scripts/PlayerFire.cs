using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    enum WeaponMode
    {
        Rifle,
        Sniper,
        Grenade,
    }
    WeaponMode wMode;

    bool ZoomMode = false;

    public GameObject firePosition;
    public GameObject grenadeFactory;
    public float throwPower = 15f;
    ParticleSystem ps;
    public GameObject bulletEffect;
    Animator anim;
    public Text gameState;
    public Text weaponMode;
    public GameObject[] eff_Flash;

    public List<GameObject> bulletEffectPool;

    public float delay = 0.075f;
    // Start is called before the first frame update
    void Start()
    {
        //ps[] = bulletEffect.GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
        wMode = WeaponMode.Rifle;
        weaponMode.text = "Rifle";
        bulletEffectPool = new List<GameObject>();
        ps = bulletEffect.GetComponent<ParticleSystem>();
        for (int i = 0; i < 15; i++)
        {
            GameObject bullet = Instantiate(bulletEffect);
            bulletEffectPool.Add(bullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position,Camera.main.transform.forward);
            RaycastHit hit = new RaycastHit();
            //Debug.DrawRay(transform.position,)
                
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    //에너미 fsm 불러와서
                    //hitenemy를 호출하여 내 데미지로 적에게 데미지
                }
                else
                {
                    bulletEffect.transform.position = hit.point;
                    bulletEffect.transform.forward = hit.point;

                    ps.Play();
                }
            }
        }
        /*delay += Time.deltaTime;
        if (Input.GetMouseButton(0) && delay >= 0.075f )//&& DataManager.instance.bullet >= 1)
        {
            //레이를 생성한 후 발사될 위치와 진행 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //레이가 부딪힌 대상의 정보를 저장할 변수를 생선한다.
            RaycastHit hitinfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitinfo))
            {
                GameObject bullet = bulletEffectPool[0];
                bulletEffectPool.Remove(bullet);
                //피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킨다.
                bullet.transform.position = hitinfo.point;
                //피격 이펙트의 forward 방향을 레이가 부딧힌 지점의 법선 벡터와 일치시킨다.
                bullet.transform.forward = hitinfo.normal;
                //피격 이펙트 플레이
                bullet.GetComponent<ParticleSystem>().Play();
                //DataManager.instance.bullet--;
                delay = 0f;
                bulletEffectPool.Add(bullet);
            }
        }*/
    }
}
