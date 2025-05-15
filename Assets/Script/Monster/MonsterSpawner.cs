using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    public float spawnInterval { get; private set; } = 5;
    private int spawnType; //?
    public int spawnAmount { get; private set; } = 2;
    private Transform playerTrf;
    private Transform lanternTrf;
    [SerializeField] GameObject[] monsters;

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
        co = StartCoroutine(MonsterSpawnInterval());
    }
    private void OnDestroy()
    {
        if(co!=null) StopCoroutine(co);
    }
    public void SetSpawnOption(float interval,int amount)
    {
        spawnInterval = interval;
        spawnAmount = amount;
    }
    IEnumerator MonsterSpawnInterval()
    {
        while (true)
        {
            int amount = spawnAmount;
            while (amount-- > 0)
            {
                MonsterSpawn(Random.Range(0, monsters.Length), 1);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void MonsterSpawn(int type, int amount)
    {
        if(type >= monsters.Length) { 
            Debug.LogError("������ type�� monster�ε����� ���");
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
                Debug.LogWarning("���� ������ ��ġ�� ã�� ���߽��ϴ�.");

        }
    } 
     

    Vector2? GetRandomPositionInRingUnion()
    {
        int attempt = 0;

        while (attempt < maxAttempts)
        {
            attempt++;

            // ū ���� �ȿ��� ������ �� ����
            Vector2 randomPoint = GetRandomPointAround(playerTrf.position, bigRadius1, lanternTrf.position, bigRadius2);

            float distToPlayer = Vector2.Distance(randomPoint, playerTrf.position);
            float distToLantern = Vector2.Distance(randomPoint, lanternTrf.position);

            // ū �� �� �ϳ� �ȿ� �ִ��� Ȯ��
            bool inBigUnion = distToPlayer <= bigRadius1 || distToLantern <= bigRadius2;

            // ���� �� �� �ϳ� �ȿ� ������ Ȯ��
            bool inSmallUnion = distToPlayer <= smallRadius1 || distToLantern <= smallRadius2;

            // ���� �����ϸ� ��ȯ
            if (inBigUnion && !inSmallUnion)
                return randomPoint;
        }

        return null;
    }

    Vector2 GetRandomPointAround(Vector2 center1, float radius1, Vector2 center2, float radius2)
    {
        // �������� ��� �� �߽� �������� �������� ����
        Vector2 center = Random.value < 0.5f ? center1 : center2;
        float radius = center == center1 ? radius1 : radius2;

        // �� ���� ������ �� ����
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float r = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;
        return center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;
    }
}
