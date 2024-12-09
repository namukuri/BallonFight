using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text TxtScore; // txtScore ゲームオブジェクトの持つ Text コンポーネントをインスペクターからアサインする

    // スコア表示を更新
    public void UpdateDisplayScore(int score) //　<=　この引数でスコアの情報を受け取る
    {
        TxtScore.text = score.ToString();
        
    }
       
    
}
