using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager ins;
    
    public InputField targetValue;
    public InputField enemyValue;
    public GameObject targetPrefab;
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform eSpawnPoint;
    public int minValue = 0;
    public int tmaxValue = 15;
    public int emaxvalue = 12;
    private Spawnpoint tsP;
    private Spawnpoint esP;
    public float spawnDelay = 1f;
    public List<GameObject> enemy = new List<GameObject>();
    private void Awake()
    {
        ins = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //연습 모드 시작 시 enemy가 IDLE상태로 진입 
        //과녁 생성할 때 일정 시간의 딜레이를 주고 생성
        targetValue.onValueChanged.AddListener((input)=>ValidateInput(input,targetValue));
        enemyValue.onValueChanged.AddListener((input) => ValidateInput(input, enemyValue));
        tsP = GameObject.Find("TargetSpawn").GetComponent<Spawnpoint>();
        esP = eSpawnPoint.GetComponent<Spawnpoint>();
        if (tsP == null)
        {
            Debug.LogError("SpawnPointManager not found.");
        }
    }

    private void ValidateInput(string input, InputField iF)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
        {
            // 입력값이 숫자가 아닌 경우 마지막 문자 제거
            iF.text = System.Text.RegularExpressions.Regex.Replace(input, @"[^\d]", "");
            return;
        }
        if (string.IsNullOrEmpty(input))
        {
            return;
        }
        int value;
        if(int.TryParse(input, out value))
        {
            if (value < minValue)
            {
                iF.text = minValue.ToString();
            }
            else if (value > tmaxValue && iF.name == "TargetInput")
            {
                iF.text = tmaxValue.ToString();
            }
            else if(value > emaxvalue && iF.name == "EnemyInput")
            {
                iF.text = emaxvalue.ToString();
            }
        }
    }

    public void OnbuttonClick(Button button)
    {
        string tValue = targetValue.text.Trim();
        string eValue = enemyValue.text.Trim();
        
        if(button.name == "TargetButton")
        {
            if (int.TryParse(tValue, out int number))
            {
                StartCoroutine(targetSpawn(number));
            }
        }
        else if(button.name == "EnemyButton")
        {
            if (int.TryParse(eValue, out int number))
            {
                StartCoroutine(EnemySpawn(number));
            }
        }
        else if(button.name == "TestMode")
        {
            for(int i = 0; i < enemy.Count; i++)
            {
                Debug.Log(enemy[i].GetComponent<EnemyFSM>().m_State);
                enemy[i].GetComponent<EnemyFSM>().m_State = EnemyFSM.EnemyState.Move;
                Debug.Log(enemy[i].GetComponent<EnemyFSM>().m_State);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator targetSpawn(int number)
    {
        
        for (int i = 0; i < number; i++)
        {
            tmaxValue--;
            spawnPoint = tsP.GetRandomSpawnPoint();
            Debug.Log(number);
            GameObject target = Instantiate(targetPrefab, spawnPoint.position, Quaternion.Euler(0,-90,0));
            Target targetScripts = target.GetComponent<Target>();
            if (targetScripts != null)
            {
                targetScripts.SetSpawnPoint(spawnPoint);
            }
            tsP.RemoveSpawnPoint(spawnPoint);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    
    IEnumerator EnemySpawn(int number)
    {

        for (int i = 0; i < number; i++)
        {
            emaxvalue--;
            eSpawnPoint = esP.GetRandomSpawnPoint();
            Debug.Log(number);
            enemy.Add(Instantiate(enemyPrefab, eSpawnPoint.position, Quaternion.identity));
            EnemyFSM targetScript = enemy[i].GetComponent<EnemyFSM>();
            if (targetScript != null)
            {
                targetScript.SetSpawnPoint(eSpawnPoint);
            }
            esP.RemoveSpawnPoint(eSpawnPoint);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
