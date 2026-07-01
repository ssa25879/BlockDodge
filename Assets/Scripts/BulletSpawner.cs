using System.Collections;
using UnityEngine;

public enum BulletPattern
{
    Single,
    Burst,
    Spread
}

public class BulletSpawner : MonoBehaviour
{
    public bool isActive = true;

    public GameObject prefab;
    public Transform shotPos;

    public float rateMin = 0.5f;
    public float rateMax = 3f;

    public float bulletSpeedInitial = 8f;   // 총알 초기 속도
    public float bulletSpeedIncrement = 0.1f; // 매 초마다 증가할 속도
    public float bulletSpeedMax = 15f;      // 총알 최대 속도

    public BulletPattern bulletPattern = BulletPattern.Single;
    public int burstCount = 1;
    public float burstInterval = 0.12f;
    public float spreadAngle = 15f;

    private float bulletSpeed;
    private float speedTimer;

    private Transform target;
    private float spawnRate;
    private float timeAfterSpawn;
    private bool isBursting;

    public float CurrentBulletSpeed => bulletSpeed;

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
        if (!isActive) { return; }

        // 매 초마다 총알 속도 증가
        speedTimer += Time.deltaTime;
        if (speedTimer >= 1f)
        {
            speedTimer -= 1f;
            bulletSpeed = Mathf.Min(bulletSpeed + bulletSpeedIncrement, bulletSpeedMax);
        }

        // 발사하기 전까지 timeAfterSpawn에 틱당 경과한 시간 누적
        timeAfterSpawn += Time.deltaTime;


        if(timeAfterSpawn >= spawnRate && !isBursting)
        {
            // 발사 한 후 시간 초기화
            timeAfterSpawn = 0f;
            // 발사 하는 대포의 방향을 플레이어를 바라보도록 회전
            AimAtTarget();

            FirePattern();

            // 다음 발사까지의 시간 랜덤 설정
            spawnRate = Random.Range(rateMin, rateMax);

        }
    }

    public void ApplyDifficulty(
        float newRateMin,
        float newRateMax,
        float newBulletSpeedInitial,
        float newBulletSpeedIncrement,
        float newBulletSpeedMax,
        BulletPattern newBulletPattern,
        int newBurstCount,
        float newBurstInterval,
        float newSpreadAngle)
    {
        rateMin = Mathf.Max(0.01f, newRateMin);
        rateMax = Mathf.Max(rateMin, newRateMax);
        bulletSpeedInitial = newBulletSpeedInitial;
        bulletSpeedIncrement = newBulletSpeedIncrement;
        bulletSpeedMax = newBulletSpeedMax;
        bulletPattern = newBulletPattern;
        burstCount = Mathf.Max(1, newBurstCount);
        burstInterval = Mathf.Max(0f, newBurstInterval);
        spreadAngle = Mathf.Max(0f, newSpreadAngle);

        bulletSpeed = Mathf.Min(bulletSpeedInitial, bulletSpeedMax);
        speedTimer = 0f;
        timeAfterSpawn = 0f;
        isBursting = false;
        StopAllCoroutines();
        spawnRate = Random.Range(rateMin, rateMax);
    }

    public void SetSpawnerActive(bool active)
    {
        isActive = active;
        enabled = active;

        if (!active)
        {
            isBursting = false;
            timeAfterSpawn = 0f;
            StopAllCoroutines();
        }
    }

    private void AimAtTarget()
    {
        if (target == null) { return; }

        Vector3 direction = target.position - transform.position;
        direction.y = 0; // 수평 방향으로만 회전하도록 y축 고정
        if(direction != Vector3.zero) {
            transform.right = -direction;
        }
    }

    private void FirePattern()
    {
        if (target == null) { return; }

        switch (bulletPattern)
        {
            case BulletPattern.Burst:
                StartCoroutine(FireBurst());
                break;
            case BulletPattern.Spread:
                FireSpread();
                break;
            default:
                FireBulletAtTarget();
                break;
        }
    }

    private IEnumerator FireBurst()
    {
        isBursting = true;

        for (int i = 0; i < burstCount; i++)
        {
            if (!isActive || target == null) { break; }

            AimAtTarget();
            FireBulletAtTarget();

            if (i < burstCount - 1)
            {
                yield return new WaitForSeconds(burstInterval);
            }
        }

        isBursting = false;
    }

    private void FireSpread()
    {
        int count = Mathf.Max(1, burstCount);

        if (count == 1)
        {
            FireBulletAtTarget();
            return;
        }

        float startAngle = -spreadAngle * (count - 1) * 0.5f;
        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + spreadAngle * i;
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f) * GetTargetRotation();
            FireBullet(rotation);
        }
    }

    private void FireBulletAtTarget()
    {
        FireBullet(GetTargetRotation());
    }

    private Quaternion GetTargetRotation()
    {
        Vector3 direction = target.position - shotPos.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            return shotPos.rotation;
        }

        return Quaternion.LookRotation(direction);
    }

    private void FireBullet(Quaternion rotation)
    {
        GameObject bullet = Instantiate(prefab, shotPos.position, rotation);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.speed = bulletSpeed;
    }
}
