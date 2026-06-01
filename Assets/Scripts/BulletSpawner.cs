using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public bool isActive = true;

    public GameObject prefab;
    public Transform shotPos;

    public float rateMin = 0.5f;
    public float rateMax = 3f;

    public float bulletSpeedInitial = 8f;   // 총알 초기 속도
    public float bulletSpeedIncrement = 0.1f; // 매 초마다 증가할 속도

    private float bulletSpeed;
    private float speedTimer;

    private Transform target;
    private float spawnRate;
    private float timeAfterSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 플레이어 오브젝트를 찾아서 타겟으로 설정
        // target = GameObject.FindGameObjectWithTag("Player").transform;
        // PlayerController 컴포넌트를 가진 게임오브젝트를 찾아 타겟으로 설정
        target = FindFirstObjectByType<PlayerController>().transform;

        // 최초 시간 초기화
        bulletSpeed = bulletSpeedInitial;
        speedTimer = 0f;
        timeAfterSpawn = 0f;

        // 스폰 주기 랜덤 설정
        spawnRate = Random.Range(rateMin, rateMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) { gameObject.SetActive(false); }

        // 매 초마다 총알 속도 증가
        speedTimer += Time.deltaTime;
        if (speedTimer >= 1f)
        {
            speedTimer -= 1f;
            bulletSpeed += bulletSpeedIncrement;
        }

        // 발사하기 전까지 timeAfterSpawn에 틱당 경과한 시간 누적
        timeAfterSpawn += Time.deltaTime;


        if(timeAfterSpawn >= spawnRate)
        {
            // 발사 한 후 시간 초기화
            timeAfterSpawn = 0f;
            // 발사 하는 대포의 방향을 플레이어를 바라보도록 회전
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // 수평 방향으로만 회전하도록 y축 고정
            if(direction != Vector3.zero) {
                transform.right = -direction;
            }

            // 총알 프리팹을 현재 위치와 회전으로 인스턴스화
            GameObject bullet = Instantiate(prefab, shotPos.position, transform.rotation);
            // 총알이 타겟을 향하도록 회전
            bullet.transform.LookAt(target);
            // 현재 속도를 총알에 적용
            bullet.GetComponent<Bullet>().speed = bulletSpeed;

            // 다음 발사까지의 시간 랜덤 설정
            spawnRate = Random.Range(rateMin, rateMax);

        }
    }
}
