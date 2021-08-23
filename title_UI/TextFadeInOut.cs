using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeInOut : MonoBehaviour
{
    private int counter = 0;  //追加
    private Text targetText;
    float r, g, b;
    float alpha;
    public float fiSpeed;
    public float foSpeed;
    int state;

    // Start is called before the first frame update
    void Start()
    {
        
        this.targetText = this.GetComponent<Text>();
        Debug.Log(this.targetText);
        r = targetText.color.r;
        g = targetText.color.g;
        b = targetText.color.b;
        /*r = targetText.GetComponent<Image>().color.r;
        g = targetText.GetComponent<Image>().color.g;
        b = targetText.GetComponent<Image>().color.b;*/
        alpha = 0.0f;
        targetText.color = new Color(r, g, b, alpha);
        state = 0;
    }
   
    void Update()
    {
            //if (counter < 3)
            //{  //追加：例えば3回とする
                switch (state)
                {
                    //フェードイン
                    case 0:
                        targetText.color = new Color(r, g, b, alpha);
                        alpha += fiSpeed;
                        if (alpha > 0.55f)
                        {
                            alpha = 0.55f;
                            state = 1;
                        }
                        break;
                    //フェードアウト
                    case 1:
                        targetText.color = new Color(r, g, b, alpha);
                        alpha -= foSpeed;
                        if (alpha < 0.0f)
                        {
                            alpha = 0.0f;
                            state = 0;
                            counter++;  //追加
                        }
                        break;
                }
            }  //追加
        //}
    
}
