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
                    //���ʹ� fsm �ҷ��ͼ�
                    //hitenemy�� ȣ���Ͽ� �� �������� ������ ������
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
            //���̸� ������ �� �߻�� ��ġ�� ���� ������ �����Ѵ�.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //���̰� �ε��� ����� ������ ������ ������ �����Ѵ�.
            RaycastHit hitinfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitinfo))
            {
                GameObject bullet = bulletEffectPool[0];
                bulletEffectPool.Remove(bullet);
                //�ǰ� ����Ʈ�� ��ġ�� ���̰� �ε��� �������� �̵���Ų��.
                bullet.transform.position = hitinfo.point;
                //�ǰ� ����Ʈ�� forward ������ ���̰� �ε��� ������ ���� ���Ϳ� ��ġ��Ų��.
                bullet.transform.forward = hitinfo.normal;
                //�ǰ� ����Ʈ �÷���
                bullet.GetComponent<ParticleSystem>().Play();
                //DataManager.instance.bullet--;
                delay = 0f;
                bulletEffectPool.Add(bullet);
            }
        }*/
    }
}
