using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class typing : MonoBehaviour
{ 
    //タイピングデータクラスのインスタンス
    public typingdata td = new typingdata();
    //　表示テキスト
    private Text UIJ;
    private Text UIR;
    private Text UIH;
    //　入力した文字列テキスト
    //private Text UII;
    //入力された文字のqueue
    private static Queue<char> Inqueue = new Queue<char>();
    //入力された時刻を入れるqueue
    private static Queue<double> Timequeue = new Queue<double>();

    //時間
    private static double JudgeTime;

    //　正解した文字列を入れておく
    private string correctString;

    //問題サイズ
    private int Qsize = 4;

    //　問題
    string nQJ, nQH, nQR;
    //ローマ字の入力パターン
    List<List<string>> Rpattern;

    //　問題番号
    private int numberOfQuestion;
    //　正解率
    private float correctAR;

    //　正解数と失敗数
    private int correctN, mistakeN;
    //　正解数表示用とかテキストUI
    private Text UIcorrectA, UImistake, UIcorrectAR;


    

    void Start()
    {
        //　テキストUIを取得
        UIJ = transform.Find("InputPanel/QuestionJ").GetComponent<Text>();
        UIR = transform.Find("InputPanel/QuestionR").GetComponent<Text>();
        //UII = transform.Find("InputPanel/Input").GetComponent<Text>();
        UIH = transform.Find("InputPanel/Hiragana").GetComponent<Text>();
        
        UIcorrectA = transform.Find("DataPanel/Correct Answer").GetComponent<Text>();
        UImistake = transform.Find("DataPanel/Mistake").GetComponent<Text>();
        UIcorrectAR = transform.Find("DataPanel/Correct Answer Rate").GetComponent<Text>();

        //　データ初期化処理
        init();

        OutputQ();
    }
    void init()
    {
        correctN = 0;
        UIcorrectA.text = correctN.ToString();
        mistakeN = 0;
        UImistake.text = mistakeN.ToString();
        correctAR = 0;
        UIcorrectAR.text = correctAR.ToString();
        JudgeTime = -1.0;
    }

    private int indexOfString;

    // Update is called once per frame
    //void Update()
    //{
    //    // escが押されたら終了
    //    if (Input.GetKey(KeyCode.Escape)) Quit();

    //    //　今見ている文字とキーボードから打った文字が同じかどうか
    //    if (Input.GetKeyDown(nQR[indexOfString].ToString()))
    //    {
    //        //　正解時の処理を呼び出す
    //        Correct();
    //        //　問題を入力し終えたら次の問題を表示
    //        if (indexOfString >= nQR.Length)
    //        {
    //            OutputQ();
    //        }
    //    }
    //    else if (Input.anyKeyDown)
    //    {
    //        //　失敗時の処理を呼び出す
    //        Mistake();
    //    }
    //}
    //　新しい問題を表示するメソッド
    void OutputQ()
    {
        //　テキストUIを初期化する
        UIJ.text = "";
        UIR.text = "";
        UIH.text = "";

        //　正解した文字列を初期化
        correctString = "";
        //　文字の位置を0番目に戻す
        indexOfString = 0;
        //　問題数内でランダムに選ぶ
        numberOfQuestion = UnityEngine.Random.Range(0, Qsize);

        //　選択した問題をテキストUIにセット
        var t = td.Get();
        nQJ = t.Ja;
        Rpattern = t.Ro;
        nQR = "";
        nQH = t.Hi;
        for(int i = 0; i < Rpattern.Count; i++)
        {
            nQR = String.Concat(nQR,Rpattern[i][0]);
        }
        UIJ.text = nQJ;
        UIR.text = nQR;
        UIH.text = nQH;
       
    }

    //タイピングの正誤判定
    //void IsOk()
    //{
    //    //入力のqueueがある限り
    //    while(Inqueue.Count > 0)
    //    {
    //        //Inqueueの先頭の文字を得る
    //        char inchar = Inqueue.Peek();
    //        Inqueue.Dequeue();
    //        double keypressedtime = Timequeue.Peek();
    //        Timequeue.Dequeue();
    //        //最後に判定された時間よりも前に押されていたら続ける
    //        if(keypressedtime <= JudgeTime) { continue; }
    //        JudgeTime = keypressedtime;

    //        //ミスかどうかを調べる
    //        bool isMiss = true;

    //        for(int i=0;i< )

    //    }
    //}


    //　タイピング正解時の処理
    //void Correct()
    //{
    //    //　正解数を増やす
    //    correctN++;
    //    UIcorrectA.text = correctN.ToString();
    //    //　正解率の計算
    //    CorrectAnswerRate();
    //    //　正解した文字を表示
    //    correctString += nQR[indexOfString].ToString();
    //    UII.text = correctString;
    //    //　次の文字を指す
    //    indexOfString++;
    //}

    //　タイピング失敗時の処理
    void Mistake()
    {
        //　失敗数を増やす（同時押しにも対応させる）
        mistakeN += Input.inputString.Length;

        UImistake.text = mistakeN.ToString();
        //　正解率の計算
        CorrectAnswerRate();
        //　失敗した文字を表示
        if (Input.inputString != "")
        {
            //UII.text = correctString + "<color=#ff0000ff>" + Input.inputString + "</color>";
        }
    }

    //　正解率の計算処理
    void CorrectAnswerRate()
    {
        //　正解率の計算
        correctAR = 100f * correctN / (correctN + mistakeN);
        //　小数点以下の桁を合わせる
        UIcorrectAR.text = correctAR.ToString("0.00");
    }

    // ゲームを終了する関数
    void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
        #endif
    }
}
