using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typing : MonoBehaviour
{ 
    public typingdata td = new typingdata();
    //public typingdata qR = new typingdata();
    //public typingdata qH = new typingdata();
    //　表示テキスト
    private Text UIJ;
    private Text UIR;
    private Text UIH;
    //　入力した文字列テキスト
    private Text UII;

    //　正解した文字列を入れておく
    private string correctString;

    //問題サイズ
    private int Qsize = 4;

    //　問題
    string nQJ, nQH;
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
        UII = transform.Find("InputPanel/Input").GetComponent<Text>();
        UIH = transform.Find("InputPanel/Hiragana").GetComponent<Text>();
        
        UIcorrectA = transform.Find("DataPanel/Correct Answer").GetComponent<Text>();
        UImistake = transform.Find("DataPanel/Mistake").GetComponent<Text>();
        UIcorrectAR = transform.Find("DataPanel/Correct Answer Rate").GetComponent<Text>();

        //　データ初期化処理
        correctN = 0;
        UIcorrectA.text = correctN.ToString();
        mistakeN = 0;
        UImistake.text = mistakeN.ToString();
        correctAR = 0;
        UIcorrectAR.text = correctAR.ToString();

        OutputQ();
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
        numberOfQuestion = Random.Range(0, Qsize);

        //　選択した問題をテキストUIにセット
        nQJ = td.GetJ(numberOfQuestion);
        Rpattern = td.GetR(numberOfQuestion);
        nQH = td.GetH(numberOfQuestion);
        UIJ.text = nQJ;
        UIR.text = Rpattern[0][0];
        UIH.text = nQH;
       
    }
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
            UII.text = correctString + "<color=#ff0000ff>" + Input.inputString + "</color>";
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
