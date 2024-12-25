using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    private GoalChecker goalHousePrefab; // ゴール地点のプレファブをアサイン

    [SerializeField]
    private PlayerController playerController; // ヒエラルキーにある Yuko_Player ゲームオブジェクトをアサイン

    [SerializeField]
    private FloorGenerator[] floorGenerators; // floorGenerator スクリプトのアタッチされているゲームオブジェクトをアサイン

    private bool isSetUp;  // ゲームの準備判定用。true になるとゲーム開始

    private bool isGameUp;

    private int generateCount; // 空中床の生成回数

    // generateCount 変数のプロパティ
    public int GenerateCount
    {
        set
        {
            generateCount = value;

            Debug.Log("生成数/クリア目標数 : " + generateCount + "/" + clearCount);

            if (generateCount >= clearCount)
            {
                // ゴール地点を生成
                GenerateGoal();

                // ゲーム終了
                GameUp();
            }
        }
        get
        {
            return generateCount;
        }

    }

    public int clearCount;

    void Start()
    {
        isGameUp = false;
        isSetUp = false;

        // FloorGeneratorの準備
        SetUpFloorGenerators();

        // TODO 各ジェネレータを停止
        Debug.Log("生成停止");
    }

    // FloorGeneratorの準備
    private void SetUpFloorGenerators()
    {
        for (int i = 0; i < floorGenerators.Length; i++)
        {
            // FloorGeneratorの準備・初期設定を行う
            floorGenerators[i].SetUpGenerator(this);
        }
    }
    void Update()
    {
        // プレイヤーがはじめてバルーンを生成したら
        if (playerController.isFirstGenerateBallon && isSetUp == false)
        {

            // 準備完了
            isSetUp = true;

            // TODO 各ジェネレータを動かし始める
            Debug.Log("生成スタート");
        }
    }

    // ゴール地点の生成
    private void GenerateGoal()
    {
        // ゴール地点を生成
        GoalChecker goalHouse = Instantiate(goalHousePrefab);

        // TODO ゴール地点の初期設定
        Debug.Log("ゴール地点生成");
    }

    // ゲーム終了
    public void GameUp()
    {

        // ゲーム終了
        isGameUp = true;

        // TODO 各ジェネレータを停止
        Debug.Log("生成停止");
    }
}
