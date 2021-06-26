using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeR 
{
    //白狐さんのブログを参照
    //https://qiita.com/Arthur_Lugh/items/43b61877819e402c50d6

    // ひらがな文を受け取り、入力パターンを出力する
    public List<string> convert(string s)
    {
        var pattern = new List<string>(); // ローマ字の入力パターン

        string one, two;
        for(int i=0; i<s.Length;i++)
        {
            one = s[i].ToString();
            if(i+1 < s.Length){ two = s[i].ToString() + s[i + 1].ToString();}
            else { two = ""; }

            if (HR.ContainsKey(two))
            {
                i += 2;
                pattern.Add(two);
            }
            else
            {
                i++;
                pattern.Add(one);
            }
        }
        return pattern;
    }
    //入力パターンのdictionary
    //非対応箇所があれば随時追加
    private Dictionary<string, string[]> HR = new Dictionary<string, string[]>
    {
        ;
    }
}
