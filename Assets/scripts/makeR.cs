using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;

public class makeR 
{
    //白狐さんのブログを参照
    //https://qiita.com/Arthur_Lugh/items/43b61877819e402c50d6

    public List<List<string>> Convert(string s)
    {
        var pattern = new List<List<string>>();
        var tmp = new List<string>();
        //パース
        tmp = ParseHiraganaSentence(s);
        //パターン生成
        pattern = MakeTypeSentence(tmp);

        //「テスト」なら1次元目に"te","su","to"が並ぶ(e.g. pattern[0] ="te"..)
        return pattern;
    }

    //ひらがな文を受け取り、パースする
    public List<string> ParseHiraganaSentence(string str)
    {
        var ret = new List<string>();
        int i = 0;
        string uni, bi;
        while (i < str.Length)
        {
            uni = str[i].ToString();
            if (i + 1 < str.Length)
            {
                bi = str[i].ToString() + str[i + 1].ToString();
            }
            else
            {
                bi = "";
            }
            if (HR.ContainsKey(bi))
            {
                i += 2;
                ret.Add(bi);
            }
            else
            {
                i++;
                ret.Add(uni);
            }
        }
        return ret;
    }

    // パースされたものを受け取り、入力パターンを出力する
    public List<List<string>> MakeTypeSentence(List<string> s)
    {
        var pattern = new List<List<string>>(); // ローマ字の入力パターン

        string one, two; //今見ている文字と、次の文字を入れるstring
        for (int i = 0; i < s.Count; i++)
        {
            one = s[i].ToString();
            //次の文字があれば入れる
            if (i + 1 < s.Count) { two = s[i].ToString() + s[i + 1].ToString(); }
            else { two = ""; }

            var tmpList = new List<string>();

            //「ん」の処理
            if (one.Equals("ん"))
            {
                bool isOKsingle;
                var nList = HR[one];
                //文末の「ん」-> nn, xnのみ
                if (s.Count == i + 1) { isOKsingle = false; }
                //次の文字が母音、な行、や行 -> nn, xnのみ
                else if (i + 1 < s.Count && (
                    two.Equals("あ") || two.Equals("い") || two.Equals("う") || two.Equals("え") || two.Equals("お") ||
                    two.Equals("な") || two.Equals("に") || two.Equals("ぬ") || two.Equals("ね") || two.Equals("の") ||
                    two.Equals("や") || two.Equals("ゆ") || two.Equals("よ")
                    ))
                { isOKsingle = false; }
                //それ以外の場合は「ん」は n のみでも良い
                else { isOKsingle = true; }
                foreach (var t in nList)
                {
                    // "n"一文字がだめな時追加しない
                    if (!isOKsingle && t.Equals("n")) { continue; }
                    tmpList.Add(t);
                }


            }
            // 「っ」の処理
            else if (one.Equals("っ"))
            {
                var ltuList = HR[one];
                var nextList = HR[two];
                var hs = new HashSet<string>();
                // 次の文字の子音だけとってくる
                foreach (string t in nextList)
                {
                    string c = t[0].ToString();
                    hs.Add(c);
                }
                var hsList = hs.ToList();
                List<string> ltuTypeList = hsList.Concat(ltuList).ToList();
                tmpList = ltuTypeList;
            }

            //「ちゃ」などの場合に「ち」+「ゃ」などを許容する
            else if (one.Length == 2 && !string.Equals("ん", one[0]))
            {
                //ちゃなどをそのまま打つパターン
                tmpList = tmpList.Concat(HR[one]).ToList();
                // ち + ゃ などの分解して入力するパターンを生成
                var fstList = HR[one[0].ToString()];
                var sndList = HR[one[1].ToString()];
                var retList = new List<string>();
                foreach (string fstStr in fstList)
                {
                    foreach (string sndStr in sndList)
                    {
                        string t = fstStr + sndStr;
                        retList.Add(t);
                    }
                }
                tmpList = tmpList.Concat(retList).ToList();
            }
            //それ以外
            
            else {
                try { tmpList = HR[one].ToList(); }
                catch { UnityEngine.Debug.Log(one); }
            }
            pattern.Add(tmpList);
        }

        return pattern;
    }
    //入力パターンのdictionary
    //非対応箇所があれば随時追加
    private Dictionary<string, string[]> HR = new Dictionary<string, string[]>
    {
        //一文字
        {"あ", new string[1] {"a"}},
        {"い", new string[2] {"i", "yi"}},
        {"う", new string[3] {"u", "wu" , "whu"}},
        {"え", new string[1] {"e"}},
        {"お", new string[1] {"o"}},
        {"か", new string[2] {"ka", "ca"}},
        {"き", new string[1] {"ki"}},
        {"く", new string[3] {"ku", "cu", "qu"}},
        {"け", new string[1] {"ke"}},
        {"こ", new string[2] {"ko", "co"}},
        {"さ", new string[1] {"sa"}},
        {"し", new string[2] {"si", "shi"}},
        {"す", new string[1] {"su"}},
        {"せ", new string[2] {"se", "ce"}},
        {"そ", new string[1] {"so"}},
        {"た", new string[1] {"ta"}},
        {"ち", new string[2] {"ti", "chi"}},
        {"つ", new string[2] {"tu", "tsu"}},
        {"て", new string[1] {"te"}},
        {"と", new string[1] {"to"}},
        {"な", new string[1] {"na"}},
        {"に", new string[1] {"ni"}},
        {"ぬ", new string[1] {"nu"}},
        {"ね", new string[1] {"ne"}},
        {"の", new string[1] {"no"}},
        {"は", new string[1] {"ha"}},
        {"ひ", new string[1] {"hi"}},
        {"ふ", new string[2] {"hu", "fu"}},
        {"へ", new string[1] {"he"}},
        {"ほ", new string[1] {"ho"}},
        {"ま", new string[1] {"ma"}},
        {"み", new string[1] {"mi"}},
        {"む", new string[1] {"mu"}},
        {"め", new string[1] {"me"}},
        {"も", new string[1] {"mo"}},
        {"や", new string[1] {"ya"}},
        {"ゆ", new string[1] {"yu"}},
        {"よ", new string[1] {"yo"}},
        {"ら", new string[1] {"ra"}},
        {"り", new string[1] {"ri"}},
        {"る", new string[1] {"ru"}},
        {"れ", new string[1] {"re"}},
        {"ろ", new string[1] {"ro"}},
        {"わ", new string[1] {"wa"}},
        {"を", new string[1] {"wo"}},
        {"ん", new string[3] {"nn", "xn","n"}}, //のちのち例外処理
        {"が", new string[1] {"ga"}},
        {"ぎ", new string[1] {"gi"}},
        {"ぐ", new string[1] {"gu"}},
        {"げ", new string[1] {"ge"}},
        {"ご", new string[1] {"go"}},
        {"ざ", new string[1] {"za"}},
        {"じ", new string[2] {"zi", "ji"}},
        {"ず", new string[1] {"zu"}},
        {"ぜ", new string[1] {"ze"}},
        {"ぞ", new string[1] {"zo"}},
        {"だ", new string[1] {"da"}},
        {"ぢ", new string[1] {"di"}},
        {"づ", new string[1] {"du"}},
        {"で", new string[1] {"de"}},
        {"ど", new string[1] {"do"}},
        {"ば", new string[1] {"ba"}},
        {"び", new string[1] {"bi"}},
        {"ぶ", new string[1] {"bu"}},
        {"べ", new string[1] {"be"}},
        {"ぼ", new string[1] {"bo"}},
        {"ぱ", new string[1] {"pa"}},
        {"ぴ", new string[1] {"pi"}},
        {"ぷ", new string[1] {"pu"}},
        {"ぺ", new string[1] {"pe"}},
        {"ぽ", new string[1] {"po"}},

        //小文字含む
        {"うぁ", new string[1] {"wha"}},
        {"うぃ", new string[2] {"whi", "wi"}},
        {"うぇ", new string[2] {"whe", "we"}},
        {"うぉ", new string[1] {"who"}},
        {"きゃ", new string[1] {"kya"}},
        {"きぃ", new string[1] {"kyi"}},
        {"きゅ", new string[1] {"kyu"}},
        {"きぇ", new string[1] {"kye"}},
        {"きょ", new string[1] {"kyo"}},
        {"ぎゃ", new string[1] {"gya"}},
        {"ぎぃ", new string[1] {"gyi"}},
        {"ぎゅ", new string[1] {"gyu"}},
        {"ぎぇ", new string[1] {"gye"}},
        {"ぎょ", new string[1] {"gyo"}},
        {"くぁ", new string[3] {"qwa", "qa", "kwa"}},
        {"くぃ", new string[3] {"qwi", "qi", "qyi"}},
        {"くぅ", new string[1] {"qwu"}},
        {"くぇ", new string[3] {"qwe", "qe", "qye"}},
        {"くぉ", new string[2] {"qwo", "qo"}},
        {"くゃ", new string[1] {"qyu"}},
        {"くゅ", new string[1] {"qye"}},
        {"くょ", new string[1] {"qyo"}},
        {"ぐぁ", new string[1] {"gwa"}},
        {"ぐぃ", new string[1] {"gwi"}},
        {"ぐぅ", new string[1] {"gwu"}},
        {"ぐぇ", new string[1] {"gwe"}},
        {"ぐぉ", new string[1] {"gwo"}},
        {"しゃ", new string[2] {"sya", "sha"}},
        {"しぃ", new string[1] {"syi"}},
        {"しゅ", new string[2] {"syu", "shu"}},
        {"しぇ", new string[2] {"sye", "she"}},
        {"しょ", new string[2] {"syo", "sho"}},
        {"じゃ", new string[3] {"zya", "ja", "jya"}},
        {"じぃ", new string[2] {"zyi", "jyi"}},
        {"じゅ", new string[3] {"zyu", "ju", "jyu"}},
        {"じぇ", new string[3] {"zye", "je", "jye"}},
        {"じょ", new string[3] {"zyo", "jo", "jyo"}},
        {"すぁ", new string[1] {"swa"}},
        {"すぃ", new string[1] {"swi"}},
        {"すぅ", new string[1] {"swu"}},
        {"すぇ", new string[1] {"swe"}},
        {"すぉ", new string[1] {"swo"}},
        {"ちゃ", new string[3] {"tya", "cha", "cya"}},
        {"ちぃ", new string[2] {"tyi", "cyi"}},
        {"ちゅ", new string[3] {"tyu", "chu", "cyu"}},
        {"ちぇ", new string[3] {"tye", "che", "cye"}},
        {"ちょ", new string[3] {"tyo", "cho", "cyo"}},
        {"ぢゃ", new string[1] {"dya"}},
        {"ぢぃ", new string[1] {"dyi"}},
        {"ぢゅ", new string[1] {"dyu"}},
        {"ぢぇ", new string[1] {"dye"}},
        {"ぢょ", new string[1] {"dyo"}},
        {"つぁ", new string[1] {"tsa"}},
        {"つぃ", new string[1] {"tsi"}},
        {"つぇ", new string[1] {"tse"}},
        {"つぉ", new string[1] {"tso"}},
        {"てゃ", new string[1] {"tha"}},
        {"てぃ", new string[1] {"thi"}},
        {"てゅ", new string[1] {"thu"}},
        {"てぇ", new string[1] {"the"}},
        {"てょ", new string[1] {"tho"}},
        {"でゃ", new string[1] {"dha"}},
        {"でぃ", new string[1] {"dhi"}},
        {"でゅ", new string[1] {"dhu"}},
        {"でぇ", new string[1] {"dhe"}},
        {"でょ", new string[1] {"dho"}},
        {"とぁ", new string[1] {"twa"}},
        {"とぃ", new string[1] {"twi"}},
        {"とぅ", new string[1] {"twu"}},
        {"とぇ", new string[1] {"twe"}},
        {"とぉ", new string[1] {"two"}},
        {"どぁ", new string[1] {"dwa"}},
        {"どぃ", new string[1] {"dwi"}},
        {"どぅ", new string[1] {"dwu"}},
        {"どぇ", new string[1] {"dwe"}},
        {"どぉ", new string[1] {"dwo"}},
        {"にゃ", new string[1] {"nya"}},
        {"にぃ", new string[1] {"nyi"}},
        {"にゅ", new string[1] {"nyu"}},
        {"にぇ", new string[1] {"nye"}},
        {"にょ", new string[1] {"nyo"}},
        {"ひゃ", new string[1] {"hya"}},
        {"ひぃ", new string[1] {"hyi"}},
        {"ひゅ", new string[1] {"hyu"}},
        {"ひぇ", new string[1] {"hye"}},
        {"ひょ", new string[1] {"hyo"}},
        {"ぴゃ", new string[1] {"pya"}},
        {"ぴぃ", new string[1] {"pyi"}},
        {"ぴゅ", new string[1] {"pyu"}},
        {"ぴぇ", new string[1] {"pye"}},
        {"ぴょ", new string[1] {"pyo"}},
        {"びゃ", new string[1] {"bya"}},
        {"びぃ", new string[1] {"byi"}},
        {"びゅ", new string[1] {"byu"}},
        {"びぇ", new string[1] {"bye"}},
        {"びょ", new string[1] {"byo"}},
        {"ふゃ", new string[1] {"fya"}},
        {"ふゅ", new string[1] {"fyu"}},
        {"ふょ", new string[1] {"fyo"}},
        {"ふぁ", new string[2] {"fwa", "fa"}},
        {"ふぃ", new string[3] {"fwi", "fi", "fyi"}},
        {"ふぅ", new string[1] {"fwu"}},
        {"ふぇ", new string[3] {"fwe", "fe", "fye"}},
        {"ふぉ", new string[2] {"fwo", "fo"}},
        {"みゃ", new string[1] {"mya"}},
        {"みぃ", new string[1] {"myi"}},
        {"みゅ", new string[1] {"myu"}},
        {"みぇ", new string[1] {"mye"}},
        {"みょ", new string[1] {"myo"}},
        {"りゃ", new string[1] {"rya"}},
        {"りぃ", new string[1] {"ryi"}},
        {"りゅ", new string[1] {"ryu"}},
        {"りぇ", new string[1] {"rye"}},
        {"りょ", new string[1] {"ryo"}},
        {"ヴぁ", new string[1] {"va"}},
        {"ヴぃ", new string[2] {"vi","vyi"}},
        {"ヴ", new string[1] {"vu"}},
        {"ヴぇ", new string[2] {"ve","vye"}},
        {"ヴぉ", new string[1] {"vo"}},
        {"ヴゃ", new string[1] {"vya"}},
        {"ヴゅ", new string[1] {"vyu"}},
        {"ヴょ", new string[1] {"vyo"}},

        //小文字
        {"ぁ", new string[2] {"la", "xa"}},
        {"ぃ", new string[4] {"li", "lyi", "xi", "xyi"}},
        {"ぅ", new string[2] {"lu", "xu"}},
        {"ぇ", new string[4] {"le", "xe", "lye", "xye"}},
        {"ぉ", new string[2] {"lo", "xo"}},
        {"ヵ", new string[2] {"lka", "xka"}},
        {"ヶ", new string[2] {"lke", "xke"}},
        {"っ", new string[4] {"ltu", "xtu", "ltsu", "xtsu"}},
        {"ゃ", new string[2] {"lya", "xya"}},
        {"ゅ", new string[2] {"lyu", "xyu"}},
        {"ょ", new string[2] {"lyo", "xyo"}},
        {"ゎ", new string[2] {"lwa", "xwa"}},


    };
}
