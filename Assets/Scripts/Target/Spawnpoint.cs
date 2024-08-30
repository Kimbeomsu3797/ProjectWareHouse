using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
 
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            if (child.CompareTag("SpawnPoint"))
            {
                spawnPoints.Add(child);
                //child.gameObject.SetActive(true);
            }
        }
        //이 파트를 함수로 따로 빼서 버튼 호출 시 마다 혹은 과녁 생성이 끝날 때 마다 호출되도록 하는게 좋을듯
    }

    public Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points available.");
            return null;
        }

        int randomIndex = Random.Range(0, spawnPoints.Count);
        return spawnPoints[randomIndex];
    }

    public void RemoveSpawnPoint(Transform spawnPoint)
    {
        if (spawnPoints.Contains(spawnPoint))
        {
            spawnPoints.Remove(spawnPoint);
            spawnPoint.gameObject.SetActive(false); // 비활성화
        }
    }

    public void AddSpawnPoint(Transform spawnPoint)
    {
        if (!spawnPoints.Contains(spawnPoint))
        {
            spawnPoints.Add(spawnPoint);
            spawnPoint.gameObject.SetActive(true); // 다시 활성화
        }
    }
  
    // Update is called once per frame
    void Update()
    {
        
    }
}
