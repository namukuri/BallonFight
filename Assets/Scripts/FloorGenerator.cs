using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject aerialFloorPrefab; // プレファブにした AerialFloor_Mid ゲームオブジェクトをインスペクターからアサインする

    [SerializeField]
    private Transform generateTran; //プレファブのクローンを生成する位置の設定
    
    [Header("生成までの待機時間")]
    public float waitTime; // １回生成するまでの待機時間。どの位の間隔で自動生成を行うか設定

    private float timer; // 待機時間の計測用

    // Update is called once per frame
    void Update()
    {
        //時間を計測する
        timer += Time.deltaTime;

        //計測している時間が、waitTimeの値と同じが、超えたら
        if (timer >= waitTime)
        {
            //次回の計測用に、timerを０にする
            timer = 0;
            //クローン生成用のメソッドを呼び出す
            GenerateFloor();
        }
        
    }

    // プレファブを元にクローンのゲームオブジェクトを生成
    private void GenerateFloor()
    {
        // 空中床のプレファブを元にクローンのゲームオブジェクトを生成
        GameObject obj = Instantiate(aerialFloorPrefab, generateTran);

        //ランダムな値を取得
        float randomPosY = Random.Range(-4.0f, 4.0f);

        //生成されたゲームオブジェクトのY軸にランダムな値を加算して、生成されるたびに高さの位置を変更する
        obj.transform.position = new Vector2 (obj.transform.position.x, obj.transform.position.y + randomPosY);
    }
}
