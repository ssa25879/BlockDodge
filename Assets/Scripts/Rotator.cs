using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public bool isRotate;
   
    void Start()
    {
        isRotate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate = false)
        {
            rotationSpeed = 0f;
        }
        else
        {
            rotationSpeed = 60f;
        }
        // 매 프레임마다 Y축 회전
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
