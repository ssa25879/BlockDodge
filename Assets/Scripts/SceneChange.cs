using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void Change()
    {
        SceneManager.LoadScene(1);  // 씬 번호로 씬 전환
        //SceneManager.LoadScene("Game"); => 씬 이름으로 씬 전환 위와 같은 결과
    }
    public void Exit()
    {
        Application.Quit();    // 게임 종료
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 }
