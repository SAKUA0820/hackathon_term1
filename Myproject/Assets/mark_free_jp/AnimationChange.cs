using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChange : MonoBehaviour{
    
    public GameObject eatingButton;//食事ボタン
    public GameObject exercisingButton;// 運動ボタン
    public GameObject mark;// マーク
    
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
    }
    
    public void EatingChsange(){
        //GetComponentを用いてAnimatorコンポーネントを取り出す.
        Animator animator = mark.GetComponent<Animator>();
        //あらかじめ設定していたintパラメーター「trans」の値を取り出す.
        int trans = animator.GetInteger("trans");
        //intパラメーターの値を設定する.
        animator.SetInteger("trans", 0);
    }
    
    public void ExerciseChange(){
        //GetComponentを用いてAnimatorコンポーネントを取り出す.
        Animator animator = mark.GetComponent<Animator>();
        //あらかじめ設定していたintパラメーター「trans」の値を取り出す.
        int trans = animator.GetInteger("trans");
        //intパラメーターの値を設定する.
        animator.SetInteger("trans", 1);
    }
}
