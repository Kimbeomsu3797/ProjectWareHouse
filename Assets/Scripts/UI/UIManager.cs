using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public InputField targetValue;
    public InputField enemyValue;
    public GameObject targetPrefab;
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public int minValue = 0;
    public int maxValue = 15;
    private Spawnpoint tsP;
    private Spawnpoint esP;
    public float spawnDelay = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //연습 모드 시작 시 enemy가 IDLE상태로 진입 
        //과녁 생성할 때 일정 시간의 딜레이를 주고 생성
        targetValue.onValueChanged.AddListener((input)=>ValidateInput(input,targetValue));
        enemyValue.onValueChanged.AddListener((input) => ValidateInput(input, enemyValue));
        tsP = FindObjectOfType<Spawnpoint>();
        esP = FindObjectOfType<Spawnpoint>();
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
            else if (value > maxValue)
            {
                iF.text = maxValue.ToString();
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
                targetSpawn(number);
            }
        }
        else if(button.name == "EnemyButton")
        {
            if (int.TryParse(eValue, out int number))
            {
                spawnPoint = esP.GetRandomSpawnPoint();
                for (int i = 0; i < number; i++)
                {
                    Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                    //스폰포인트 랜덤 + 좌표가 겹치지않게 리스트로 값빼주기 해야함 + 비활성화 된 스폰포인트가 활성화되면 리스트에 다시 더해줘야함
                    //에너미를 리스트에 담아야함
                }
            }
        }
        else if(button.name == "TestMode")
        {
            Debug.LogError("Unknown Button");
            //담은 에너미를 for문으로 상태를 전부 IDLE로 변경해야함
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
            spawnPoint = tsP.GetRandomSpawnPoint();
            Debug.Log(number);
            GameObject target = Instantiate(targetPrefab, spawnPoint.position, Quaternion.identity);
            Target targetScripts = target.GetComponent<Target>();
            if (targetScripts != null)
            {
                targetScripts.SetSpawnPoint(spawnPoint);
            }
            tsP.RemoveSpawnPoint(spawnPoint);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
