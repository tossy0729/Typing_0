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
    private Text UIH;
    //　入力した文字列テキスト
    private Text UII;

    //入力された文字のqueue
    private static Queue<char> Inqueue = new Queue<char>();
    //入力された時刻を入れるqueue
    private static Queue<double> Timequeue = new Queue<double>();

    //時間
    private static double JudgeTime;
    // ミスタイプ記録
    private static bool isRecMistype;

    //　正解した文字列を入れておく
    private string correctString;
    // 入力受け付け
    private static bool isInputValid;

    //問題サイズ
    //private int Qsize = 4;

    //index
    private static int index; // 今見ている部分のindex
    private static List<List<int>> indexAdd = new List<List<int>>();
    private static List<List<int>> sentenceIndex = new List<List<int>>();
    private static List<List<int>> sentenceValid = new List<List<int>>();//Rpatternと対応してその表記の仕方が妥当かどうかを判断

    //　問題
    string nQJ, nQH, nQR;
    //ローマ字の入力パターン
    private static List<List<string>> Rpattern;

    //　問題番号
    private int numberOfQuestion;
    //　正解率
    private float correctAR;

    //　正解数と失敗数
    private int correctN, mistakeN;
    //　正解数表示用とかテキストUI
    private Text UIcorrectA, UImistake, UIcorrectAR;
    // 文章タイピング読み
    private static List<List<string>> sentenceTyping;
    // センテンスの長さ
    private static int sentenceLength;




    void Start()
    {
        //　データ初期化処理
        init();

        OutputQ();
    }
    void init()
    {
        //　テキストUIを取得
        UIJ = transform.Find("InputPanel/QuestionJ").GetComponent<Text>();
        UII = transform.Find("InputPanel/Input").GetComponent<Text>();
        UIH = transform.Find("InputPanel/Hiragana").GetComponent<Text>();

        UIcorrectA = transform.Find("DataPanel/Correct Answer").GetComponent<Text>();
        UImistake = transform.Find("DataPanel/Mistake").GetComponent<Text>();
        UIcorrectAR = transform.Find("DataPanel/Correct Answer Rate").GetComponent<Text>();
        correctN = 0;
        mistakeN = 0;
        correctAR = 0;
        UIcorrectA.text = "correct number:" + correctN.ToString();
        UImistake.text = "miss:" + mistakeN.ToString();
        UIcorrectAR.text = "correct rate:" + correctAR.ToString();
        JudgeTime = -1.0;
        isRecMistype = false;
        isInputValid = true;
        Inqueue.Clear();
        Timequeue.Clear();
    }

    //private int indexOfString;

    // Update is called once per frame
    void Update()
    {
        // escが押されたら終了
        if (Input.GetKey(KeyCode.Escape)) Quit();

        if(Inqueue.Count > 0 && Timequeue.Count > 0)
        {
            IsOk();
        }
    }
    //　新しい問題を表示するメソッド
    void OutputQ()
    {
        //　テキストUIを初期化する
        UIJ.text = "";
        UIH.text = "";
        index = 0;
        sentenceLength = 0;
        //　正解した文字列を初期化
        correctString = "";
        //　文字の位置を0番目に戻す
        //indexOfString = 0;

        //　選択した問題をテキストUIにセット
        //問題のインスタンスをゲット
        var t = td.Get();
        //それぞれのUIにセットする
        nQJ = t.Ja;
        nQR = "";
        nQH = t.Hi;
        Rpattern = t.Ro;
        InitSentenceData();
        for (int i = 0; i < Rpattern.Count; i++)
        {
            nQR = String.Concat(nQR,Rpattern[i][0]);
        }
        UIJ.text = "<color=#000000>" + nQJ + "</color>";
        UIH.text = "<color=#808080>" + nQH + "</color>" ;
        UII.text = nQR;
        isInputValid = true;
    }
    //正誤判定の初期化
    void InitSentenceData()
    {
        var Senlen = Rpattern.Count;
        sentenceIndex.Clear();
        sentenceValid.Clear();
        indexAdd.Clear();
        //こいつらで正誤判定、そのローマ字表記は妥当かどうかなどを判定する
        sentenceIndex = new List<List<int>>();
        sentenceValid = new List<List<int>>();
        indexAdd = new List<List<int>>();
        for (int i = 0; i < Senlen; i++)
        {
            var typenum = Rpattern[i].Count;
            sentenceIndex.Add(new List<int>());
            sentenceValid.Add(new List<int>());
            indexAdd.Add(new List<int>());
            for (int j = 0; j < typenum; ++j)
            {
                sentenceIndex[i].Add(0);
                sentenceValid[i].Add(1);
                indexAdd[i].Add(0);

            }

        }
    }

    //タイピングの正誤判定
    void IsOk()
    {
        //入力のqueueがある限り
        while (Inqueue.Count > 0)
        {
            //Inqueueの先頭の文字を得る
            char inchar = Inqueue.Peek();
            Inqueue.Dequeue();
            double keypressedtime = Timequeue.Peek();
            Timequeue.Dequeue();
            //最後に判定された時間よりも前に押されていたら続ける
            if (keypressedtime <= JudgeTime) { continue; }
            JudgeTime = keypressedtime;

            //ミスかどうかを調べる
            bool isMiss = true;

            //今見ているひらがな1文字についてのパターンを調べる
            for (int i = 0; i < Rpattern[index].Count; i++)
            {
                //invalidならパス
                if (sentenceValid[index][i] == 0)
                {
                    continue;
                }
                int j = sentenceIndex[index][i];
                char nextinchar = Rpattern[index][i][j];
                //正解タイプ
                if (inchar == nextinchar)
                {
                    isMiss = false;
                    indexAdd[index][i] = 1;
                }
                else indexAdd[index][i] = 0;

            }
            if (!isMiss) Correct(inchar.ToString());
            else
            {
                
                Mistake();
            }

        }
    }

        //キー入力の受け取り
        



        //　タイピング正解時の処理
    void Correct(string s)
    {
        //　正解数を増やす
        correctN++;
        sentenceLength++;
        UIcorrectA.text = "correct number:" + correctN.ToString();
        //　正解率の計算
        CorrectAnswerRate();
        //　次の文字を指す
        isRecMistype = false;
        
        //可能な入力パターンのチェック
        bool isIndexCountUp = CheckValidSentence(s);
        UpdateSentence(s);
        if (isIndexCountUp)
        {
            index++;
        }
        if (index >= Rpattern.Count)
        {
            CompleteTask();
        }

    }

        //　タイピング失敗時の処理
    void Mistake()
    {
        //　失敗数を増やす（同時押しにも対応させる）
        mistakeN++;

        UImistake.text = "miss:" + mistakeN.ToString();
        //　正解率の計算
        CorrectAnswerRate();
        // 打つべき文字を赤く表示
        if (!isRecMistype)
        {
            string cor = "<color=#00bfff>" + correctString + "</color>";
            string s = UII.text.ToString().Replace(cor,"");
            string rest = s.Substring(1);
            string red = "<color=#ff0000>" + s[0].ToString() + "</color>";
            UII.text = cor + red + rest;
        }
        isRecMistype = true;
    }

    //　正解率の計算処理
    void CorrectAnswerRate()
    {
        //　正解率の計算
        correctAR = 100f * correctN / (correctN + mistakeN);
        //　小数点以下の桁を合わせる
        UIcorrectAR.text = "correct rate:" + correctAR.ToString("0.00");
    }
        
        /// 画面上に表示する打つ文字の表示を更新する
    void UpdateSentence(string s)
    {
        ////打ち終わった文字は消す
        string tmpTypingSentence = "";
        for (int i = 0; i < Rpattern.Count; ++i)
        {
            if (i < index)
            {
                continue;
            }
            for (int j = 0; j < Rpattern[i].Count; ++j)
            {
                if (index == i && sentenceValid[index][j] == 0)
                {
                    continue;
                }
                else if (index == i && sentenceValid[index][j] == 1)
                {
                    for (int k = 0; k < Rpattern[index][j].Length; ++k)
                    {
                        if (k >= sentenceIndex[index][j])
                        {
                            tmpTypingSentence += Rpattern[index][j][k].ToString();
                        }
                    }
                    break;
                }
                else if (index != i && sentenceValid[i][j] == 1)
                {
                    tmpTypingSentence += Rpattern[i][j];
                    break;
                }
            }
        }
        //UII.text = tmpTypingSentence;
        if (!isRecMistype)
        {
            correctString += s;
            UII.text = "<color=#00bfff>" + correctString + "</color>" + tmpTypingSentence;
        }

    }
    void CompleteTask()
    {
        
        Inqueue.Clear();
        Timequeue.Clear();
        isInputValid = false;
        OutputQ();
    }
        //有効パターンのチェック
    bool CheckValidSentence(string str)
    {
        bool ret = false;
        // 可能な入力パターンを残す
        for (int i = 0; i < Rpattern[index].Count; ++i)
        {
            // str と一致しないものを無効化処理
            if (!str.Equals(Rpattern[index][i][sentenceIndex[index][i]].ToString()))
            {
                sentenceValid[index][i] = 0;
            }
            // 次のキーへ
            sentenceIndex[index][i] += indexAdd[index][i];
            // 次の文字へ
            if (sentenceIndex[index][i] >= Rpattern[index][i].Length)
            {
                ret = true;
            }
        }
        return ret;
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
    void OnGUI()
    {
        
        Event e = Event.current;
        if (isInputValid && e.type == EventType.KeyDown && e.type != EventType.KeyUp)
        {
            if (e.character == '\0')
            {
                var inputchar = ConvertKeyCodeToChar(e.keyCode);
                Inqueue.Enqueue(inputchar);
                Timequeue.Enqueue(Time.realtimeSinceStartup);
            }
            
            
        }
    }
    //キーコードからcharへの変換
    char ConvertKeyCodeToChar(KeyCode kc)
        {
        switch (kc)
        {
            case KeyCode.A:
                return 'a';
            case KeyCode.B:
                return 'b';
            case KeyCode.C:
                return 'c';
            case KeyCode.D:
                return 'd';
            case KeyCode.E:
                return 'e';
            case KeyCode.F:
                return 'f';
            case KeyCode.G:
                return 'g';
            case KeyCode.H:
                return 'h';
            case KeyCode.I:
                return 'i';
            case KeyCode.J:
                return 'j';
            case KeyCode.K:
                return 'k';
            case KeyCode.L:
                return 'l';
            case KeyCode.M:
                return 'm';
            case KeyCode.N:
                return 'n';
            case KeyCode.O:
                return 'o';
            case KeyCode.P:
                return 'p';
            case KeyCode.Q:
                return 'q';
            case KeyCode.R:
                return 'r';
            case KeyCode.S:
                return 's';
            case KeyCode.T:
                return 't';
            case KeyCode.U:
                return 'u';
            case KeyCode.V:
                return 'v';
            case KeyCode.W:
                return 'w';
            case KeyCode.X:
                return 'x';
            case KeyCode.Y:
                return 'y';
            case KeyCode.Z:
                return 'z';
            case KeyCode.Minus:
                return '-';
            case KeyCode.Alpha1:
                return '1';
            case KeyCode.Alpha2:
                return '2';
            case KeyCode.Alpha3:
                return '3';
            case KeyCode.Alpha4:
                return '4';
            case KeyCode.Alpha5:
                return '5';
            case KeyCode.Alpha6:
                return '6';
            case KeyCode.Alpha7:
                return '7';
            case KeyCode.Alpha8:
                return '8';
            case KeyCode.Alpha9:
                return '9';
            case KeyCode.Alpha0:
                return '0';
            default:
                return '\\';
        }
        }
    }
