using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class typingdata {
    // タイピングデータ
    //private string[] qJ = { "問題", "テスト", "タイピング", "かめくめちゃん" };
    //private string[] qH = { "もんだい", "てすと", "たいぴんぐ", "かめくめちゃん" };
    //private string[] qR = { "monndai", "tesuto", "taipinngu", "kamekumechann" };
    private List<(string J, string H)> JH = new List<(string J, string H)>() {
        ("問題","もんだい"),
        ("テスト","てすと"),
        ("タイピング","たいぴんぐ"),
        ("かめくめちゃん","かめくめちゃん")
    };
    
    public makeR R = new makeR();

    //取得用関数
    //配列の何番目かを引数に取る
    public string GetJ(int num)
    {
        return this.JH[num].J;
    }
    public string GetH(int num)
    {
        return this.JH[num].H;
    }
    public List<List<string>> GetR(int num)
    {
        List<List<string>> Romaji = R.Convert(JH[num].H);
        return Romaji;
    }

    //問題文を生成する関数
    //引数は無しで、日本語文、ひらがな文、ローマ字文の順に生成する。呼び出すときはそれぞれ呼んでね
    public (string Ja, string Hi, List<string> hiraganasep, List<List<string>> Ro) Get()
    {
        string Ja = "ふぇぇ、、初期値のままだよぉ";
        string Hi = "ふぇぇ、、初期値のままだよぉ";
        List<string> hiraganasep = new List<string>();
        List<List<string>> Ro = new List<List<string>>();

        int qnum = UnityEngine.Random.Range(0, JH.Count-1);
        Ja = JH[qnum].J;
        Hi = JH[qnum].H;
        hiraganasep = R.ParseHiraganaSentence(Hi);
        Ro = R.Convert(Hi);

        return (Ja, Hi, hiraganasep, Ro);
    }
}
