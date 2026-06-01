using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static Rigidbody playerRigidbody;
    public float speed = 8.0f;
    public float hp = 3f;
    public float dodgeDistance = 15f;
    public float dodgeDuration = 0.3f;   // 회피 지속 시간
    public float dodgeCooldown = 1f;     // 회피 쿨다운

    public Button dodgeButton;   // 회피 버튼

    private Animator anim;      // 애니메이터 컴포넌트 -> 애니메이션 컨트롤러 접근에 필요

    public VariableJoystick joystick;   // 조이스틱 입력 받기

    public Text hpText;    // 체력 UI 텍스트

    private bool isDodging = false;       // 회피 중 여부 (무적 판정에 사용)
    private bool isDodgeCooldown = false; // 쿨다운 여부
    private Vector3 lastMoveDir = Vector3.forward; // 마지막 이동 방향

    private GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();
        anim = GetComponentInChildren<Animator>();
        gameManager = FindFirstObjectByType<GameManager>();


        anim.SetFloat("FHp", hp);   // 초기 체력 설정
        
        dodgeButton.onClick.AddListener(Dodge);

    }

    // Update is called once per frame
    void Update()
    {
        // 키보드 입력 받기
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        
        //Vector3 move = h * Vector3.right + v * Vector3.forward;

        if (isDodging) return; // 회피 중엔 일반 이동 입력 차단

        // 조이스틱으로 입력 받기
        Vector3 move = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        if (move != Vector3.zero)
        {
            lastMoveDir = move.normalized; // 마지막 이동 방향 저장
            anim.SetBool("IsMove", true);
            transform.rotation = Quaternion.LookRotation(move);
        }
        else
        {
            anim.SetBool("IsMove", false);
        }

        // 물리 기반 이동 (transform.Translate 대신 velocity 사용)
        playerRigidbody.linearVelocity = new Vector3(move.x * speed, playerRigidbody.linearVelocity.y, move.z * speed);


        /*
        // 수직, 수평 이동 입력
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // 입력값에 따라 이동 속도 계산
        float xSpeed = speed * x;
        float ySpeed = speed * y;

        // 이동 벡터 생성
        Vector3 newVelocity = new Vector3(xSpeed, 0f, ySpeed);

        // 캐릭터가 이동하는 방향으로 회전
        playerRigidbody.rotation = Quaternion.LookRotation(newVelocity);

        // 이동 벡터를 속도로 설정
        playerRigidbody.linearVelocity = newVelocity;
        */
        /*
        // 이동 구현
        if(Input.GetKey(KeyCode.UpArrow) == true || Input.GetKey(KeyCode.W) == true)
        {
            playerRigidbody.AddForce(0f, 0f, speed);
        }
        if(Input.GetKey(KeyCode.DownArrow) == true || Input.GetKey(KeyCode.S) == true)
        {
            playerRigidbody.AddForce(0f, 0f, -speed);
        }
        if(Input.GetKey(KeyCode.RightArrow) == true || Input.GetKey(KeyCode.D) == true)
        {
            playerRigidbody.AddForce(speed, 0f, 0f);
        }
        if(Input.GetKey(KeyCode.LeftArrow) == true || Input.GetKey(KeyCode.A) == true)
        {
            playerRigidbody.AddForce(-speed, 0f, 0f);
        }
        */
    }
    public void die()
    {
        if (isDodging) return; // 회피 중 무적

        // 사망시 오브젝트 비활성화
        gameObject.SetActive(false);

        // 게임 종료처리

        gameManager.EndGame();

        
        BulletSpawner[] bulletSpawner = FindObjectsOfType<BulletSpawner>();
        foreach(BulletSpawner spawner in bulletSpawner)
        {
            spawner.isActive = false;
        }
    }

    public void Dodge()
    {
        if (isDodging || isDodgeCooldown) return;
        StartCoroutine(DodgeCoroutine());
    }

    private IEnumerator DodgeCoroutine()
    {
        // 회피가 여러번 시행될 수 없게
        isDodging = true;
        isDodgeCooldown = true;

        dodgeButton.interactable = false; // 회피 버튼 비활성화

        anim.SetTrigger("Dodge");

        // dodgeDistance와 dodgeDuration으로 회피 속도 계산
        float dodgeSpeed = dodgeDistance / dodgeDuration;
        float elapsed = 0f;     // 경과 시간

        while (elapsed < dodgeDuration)
        {
            playerRigidbody.linearVelocity = new Vector3(
                lastMoveDir.x * dodgeSpeed,
                playerRigidbody.linearVelocity.y,
                lastMoveDir.z * dodgeSpeed
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerRigidbody.linearVelocity = Vector3.zero;
        isDodging = false;

        // 쿨다운 대기
        yield return new WaitForSeconds(dodgeCooldown);
        dodgeButton.interactable = true; // 회피 버튼 활성화
        isDodgeCooldown = false;
    }


    public void DamageReact()
    {
        if (isDodging) return; // 회피 중 무적

        float hp = anim.GetFloat("FHp");
        gameManager.LossHP(hp);
        hp -= 1f;
        anim.SetFloat("FHp", hp);
        if (hp <= 0)
        {
            die();
        }
    }
}
