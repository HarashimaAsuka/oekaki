using UnityEngine;

public class Goal : MonoBehaviour
{
    float duration;
    bool stay;


    void Update()
    {
        if (this.stay)
        {
            this.duration += Time.deltaTime;
            // ３秒経過でゴールと判定
            if (this.duration > 3.0f)
            {
                Debug.Log("Clear!!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        this.duration = 0; 
        this.stay = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        this.stay = false;
    }
}