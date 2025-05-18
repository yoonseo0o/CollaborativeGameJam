using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    public float spawnInterval { get; private set; } = 20;
    private int spawnType; //?
    public int spawnAmount { get; private set; } = 2;
    private Transform playerTrf;
    private Transform lanternTrf;
    [SerializeField] GameObject[] monsters;
    [SerializeField] private MonsterSpawnData monsterSpawnData;

    //spawnRange 
    public float bigRadius1 = 50f; // player
    public float bigRadius2 = 50f; // lantern
    public float smallRadius1 = 15f; // player
    public float smallRadius2 = 5f;  // lantern

    public int maxAttempts = 1000;

    Coroutine co;
    private void Start()
    {
        playerTrf = GameManager.Instance.playerTrf;
        lanternTrf = GameManager.Instance.lanternTrf;
    }
    private void OnDestroy()
    {
        if(co!=null) StopCoroutine(co);
    }
    public void StartSpawn()
    {
        co = StartCoroutine(MonsterSpawnInterval());
    } 
    public void SetSpawnOption(float interval,int amount)
    {
        spawnInterval = interval;
        spawnAmount = amount;
    }
    IEnumerator MonsterSpawnInterval()
    {
        int diff = GameManager.Instance.Difficulty;
        SpawnData data = monsterSpawnData.spawnData[Random.Range(diff * 3 - 3, diff * 3)];
        while (true)
        { 
            MonsterSpawn(0, data.monster1_1Amount); 
            MonsterSpawn(1, data.monster1_2Amount);
            MonsterSpawn(2, data.monster2_1Amount);
            MonsterSpawn(3, data.monster2_2Amount);
            MonsterSpawn(4, data.monster3_1Amount);
            MonsterSpawn(5, data.monster3_2Amount);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void MonsterSpawn(int type, int amount)
    {
        if(type >= monsters.Length) { 
            Debug.LogError("지정한 type이 monster인덱스를 벗어남");
            return;
        }
        while(amount-->0)
        {
            Vector2? result = GetRandomPositionInRingUnion();
            if (result.HasValue)
            { 
                GameObject obj = Instantiate(monsters[type], transform);
                obj.transform.position = new Vector3(result.Value.x, 0, result.Value.y);
                GameManager.Instance.MonsterManager.AddMonster(obj.GetComponent<Monster>());
            }
            else
                Debug.LogWarning("범위 내에서 위치를 찾지 못했습니다.");

        }
    } 
     

    Vector2? GetRandomPositionInRingUnion()
    {
        int attempt = 0;

        while (attempt < maxAttempts)
        {
            attempt++;

            // 큰 범위 안에서 임의의 점 생성
            Vector2 randomPoint = GetRandomPointAround(playerTrf.position, bigRadius1, lanternTrf.position, bigRadius2);

            float distToPlayer = Vector2.Distance(randomPoint, playerTrf.position);
            float distToLantern = Vector2.Distance(randomPoint, lanternTrf.position);

            // 큰 원 중 하나 안에 있는지 확인
            bool inBigUnion = distToPlayer <= bigRadius1 || distToLantern <= bigRadius2;

            // 작은 원 중 하나 안에 들어갔는지 확인
            bool inSmallUnion = distToPlayer <= smallRadius1 || distToLantern <= smallRadius2;

            // 조건 만족하면 반환
            if (inBigUnion && !inSmallUnion)
                return randomPoint;
        }

        return null;
    }

    Vector2 GetRandomPointAround(Vector2 center1, float radius1, Vector2 center2, float radius2)
    {
        // 랜덤으로 어느 원 중심 기준으로 생성할지 선택
        Vector2 center = Random.value < 0.5f ? center1 : center2;
        float radius = center == center1 ? radius1 : radius2;

        // 원 안의 무작위 점 생성
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float r = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;
        return center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;
    }
}
