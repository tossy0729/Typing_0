using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class typingdata { 
    // タイピングデータ
    private string[] qJ = { "問題", "テスト", "タイピング", "かめくめちゃん" };
    private string[] qH = { "もんだい", "てすと", "たいぴんぐ", "かめくめちゃん" };
    private string[] qR = { "monndai", "tesuto", "taipinngu", "kamekumechann" };

    private List<(string J, string H)> JH = new List<(string J, string H)>()
    {
        ("問題","もんだい"), ("テスト","てすと"), ("タイピング","たいぴんぐ")
    };

    //取得用関数
    //配列の何番目かを引数に取る
    public string GetJ(int num)
    {
        return this.qJ[num];
    }
    public string GetH(int num)
    {
        return this.qH[num];
    }
    public string GetR(int num)
    {
        return this.qR[num];
    }
}
