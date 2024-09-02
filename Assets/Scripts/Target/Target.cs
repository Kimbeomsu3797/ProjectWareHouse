using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetSpawnPoint(Transform spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    void OnDestroy()
    {
        if (spawnPoint != null)
        {
            Spawnpoint spm = FindObjectOfType<Spawnpoint>();
            if (spm != null)
            {
                spm.AddSpawnPoint(spawnPoint); // 스폰 포인트 다시 활성화
                
            }
        }
    }
}
