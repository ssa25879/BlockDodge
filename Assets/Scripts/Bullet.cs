using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;    // 탄알 속도
    private Rigidbody rb;

    public GameObject explosionEffect;  // 폭발 효과 프리팹

    public AudioSource audioSource;     // 사운드 재생을 위한 AudioSource 컴포넌트
    public AudioClip clipExplosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = transform.forward * speed; // 탄알이 발사되는 방향으로 속도 설정

        //Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        // 탄알이 비활성화 될 때 제거
        //if (gameObject.activeInHierarchy == false)
        //{ 
        //    Destroy(gameObject);
        //}
    }
    private void OnTriggerEnter(Collider other)
    { 
        // Player 태그를 가진 오브젝트에 충돌했을 때
        if (other.tag == "Player")
        {
            Instantiate(explosionEffect, other.transform.position, Quaternion.identity); // 폭발 효과 생성
            
            audioSource.PlayOneShot(clipExplosion); // 사운드 재생

            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null) {
                player.DamageReact();
            }

            Destroy(gameObject);
        }
        // 벽에 부딛혔을 때 탄알 제거
        if(other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
