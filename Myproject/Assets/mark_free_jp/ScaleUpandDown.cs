using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpandDown : MonoBehaviour
{
    public GameObject eatingButton;//食事ボタン
    public GameObject exercisingButton;// 運動ボタン
    public GameObject mark;// マーク
    private float scale; //拡大率
    private float x;// X座標
    private float y;// Y座標
    private static bool isExercising;
    private int countPosition;
    private int countScale;
    
    void Start(){
        isExercising = false;
    }
    
    void Update(){
        x = mark.transform.localPosition.x;
        y = mark.transform.localPosition.y;
        if(isExercising){
            countPosition ++;
            countScale ++;
            scale = mark.transform.localScale.x;
            if(countScale % 20 == 0){
                countScale = 0;
                if(scale > 50f ){
                    scale -= 1f;
                    mark.transform.localScale = new Vector3(scale, scale, 0);
                }    
            }
            
            if(countPosition % 2 == 0){
                if(x <550){
                    mark.transform.localPosition = new Vector3(x+1, y, 0);    
                }else{
                    mark.transform.localPosition = new Vector3(-450, y, 0);
                }
                countPosition = 0;
            }
        }
    }

    public void ScaleUp(){
        isExercising = false;
        scale = mark.transform.localScale.x;
        if(scale < 500f){
            scale += 10f;
            mark.transform.localScale = new Vector3(scale, scale, 0);
        }
    }
    
    public void ScaleDown(){       
        isExercising = true;
        /**
        scale = mark.transform.localScale.x;
        x = mark.transform.localPosition.x;
        y = mark.transform.localPosition.y;
        Debug.Log(scale);
        Debug.Log(x);
        if(scale > 100f){
            scale -= 10f;
            x += 10f;
            Debug.Log(scale);
            mark.transform.localScale = new Vector3(scale, scale, 0);
            mark.transform.localPosition = new Vector3(x, y, 0);
        }
        **/
    }
}
