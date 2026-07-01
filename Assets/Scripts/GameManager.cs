using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;
    public Text timeText;
    public Text recordText;

    public Image[] hpHeart;
    
    private float surviveTime;
    private bool isGameover;

    public float SurviveTime => surviveTime;
    public bool IsGameover => isGameover;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 게임 시작시 값 초기화
        surviveTime = 0;
        isGameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 생존 시간 동안
        if (!isGameover)
        {
            // 생존 시간 갱신
            surviveTime += Time.deltaTime;
            // 갱신한 시간 화면에 표시
            timeText.text = "Time: " + (int)surviveTime;
            // 시간이 10초씩 늘어날 때 마다 게임 난이도 증가를 위해 Bullet 속도 증가
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void EndGame()
    {
        // 사망 상태로 변경
        isGameover = true;
        BGMManager.Instance.PlayDeadBGM();
        //bool isChange = false;

        // 게임오버 텍스트 활성화
        gameoverText.SetActive(true);

        // 유저의 PlayerPrefs에 저장된 최고 기록(BestTime) 불러오기
        float bestTime = PlayerPrefs.GetFloat("BestTime");

        // 최고기록 갱신시
        if (surviveTime > bestTime) {
            bestTime = surviveTime;
            // 갱신한 최고기록을 PlayerPrefs에 저장
            PlayerPrefs.SetFloat("BestTime", bestTime);

            // 갱신이 될 경우를 표시
            //isChange = true;
        }


        // 화면에 최고 기록 표시
        recordText.text = "Best Time: " + (int)bestTime;
        //if(isChange)
        //{
        //    // 만약 최고기록이 갱신될 경우 New Record! 텍스트 추가
        //    recordText.text += "\nNew Record!";
        //}
    }
    public void LossHP(float currentHP)
    {
        hpHeart[(int)currentHP - 1].gameObject.SetActive(false);
    }
}
