using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;

public class makeR 
{
    //���ς���̃u���O���Q��
    //https://qiita.com/Arthur_Lugh/items/43b61877819e402c50d6

    public List<List<string>> Convert(string s)
    {
        var pattern = new List<List<string>>();
        var tmp = new List<string>();
        //�p�[�X
        tmp = ParseHiraganaSentence(s);
        //�p�^�[������
        pattern = MakeTypeSentence(tmp);

        //�u�e�X�g�v�Ȃ�1�����ڂ�"te","su","to"������(e.g. pattern[0] ="te"..)
        return pattern;
    }

    //�Ђ炪�ȕ����󂯎��A�p�[�X����
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

    // �p�[�X���ꂽ���̂��󂯎��A���̓p�^�[�����o�͂���
    public List<List<string>> MakeTypeSentence(List<string> s)
    {
        var pattern = new List<List<string>>(); // ���[�}���̓��̓p�^�[��

        string one, two; //�����Ă��镶���ƁA���̕���������string
        for (int i = 0; i < s.Count; i++)
        {
            one = s[i].ToString();
            //���̕���������Γ����
            if (i + 1 < s.Count) { two = s[i].ToString() + s[i + 1].ToString(); }
            else { two = ""; }

            var tmpList = new List<string>();

            //�u��v�̏���
            if (one.Equals("��"))
            {
                bool isOKsingle;
                var nList = HR[one];
                //�����́u��v-> nn, xn�̂�
                if (s.Count == i + 1) { isOKsingle = false; }
                //���̕������ꉹ�A�ȍs�A��s -> nn, xn�̂�
                else if (i + 1 < s.Count && (
                    two.Equals("��") || two.Equals("��") || two.Equals("��") || two.Equals("��") || two.Equals("��") ||
                    two.Equals("��") || two.Equals("��") || two.Equals("��") || two.Equals("��") || two.Equals("��") ||
                    two.Equals("��") || two.Equals("��") || two.Equals("��")
                    ))
                { isOKsingle = false; }
                //����ȊO�̏ꍇ�́u��v�� n �݂̂ł��ǂ�
                else { isOKsingle = true; }
                foreach (var t in nList)
                {
                    // "n"�ꕶ�������߂Ȏ��ǉ����Ȃ�
                    if (!isOKsingle && t.Equals("n")) { continue; }
                    tmpList.Add(t);
                }


            }
            // �u���v�̏���
            else if (one.Equals("��"))
            {
                var ltuList = HR[one];
                var nextList = HR[two];
                var hs = new HashSet<string>();
                // ���̕����̎q�������Ƃ��Ă���
                foreach (string t in nextList)
                {
                    string c = t[0].ToString();
                    hs.Add(c);
                }
                var hsList = hs.ToList();
                List<string> ltuTypeList = hsList.Concat(ltuList).ToList();
                tmpList = ltuTypeList;
            }

            //�u����v�Ȃǂ̏ꍇ�Ɂu���v+�u��v�Ȃǂ����e����
            else if (one.Length == 2 && !string.Equals("��", one[0]))
            {
                //����Ȃǂ����̂܂ܑłp�^�[��
                tmpList = tmpList.Concat(HR[one]).ToList();
                // �� + �� �Ȃǂ̕������ē��͂���p�^�[���𐶐�
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
            //����ȊO
            
            else {
                try { tmpList = HR[one].ToList(); }
                catch { UnityEngine.Debug.Log(one); }
            }
            pattern.Add(tmpList);
        }

        return pattern;
    }
    //���̓p�^�[����dictionary
    //��Ή��ӏ�������ΐ����ǉ�
    private Dictionary<string, string[]> HR = new Dictionary<string, string[]>
    {
        //�ꕶ��
        {"��", new string[1] {"a"}},
        {"��", new string[2] {"i", "yi"}},
        {"��", new string[3] {"u", "wu" , "whu"}},
        {"��", new string[1] {"e"}},
        {"��", new string[1] {"o"}},
        {"��", new string[2] {"ka", "ca"}},
        {"��", new string[1] {"ki"}},
        {"��", new string[3] {"ku", "cu", "qu"}},
        {"��", new string[1] {"ke"}},
        {"��", new string[2] {"ko", "co"}},
        {"��", new string[1] {"sa"}},
        {"��", new string[2] {"si", "shi"}},
        {"��", new string[1] {"su"}},
        {"��", new string[2] {"se", "ce"}},
        {"��", new string[1] {"so"}},
        {"��", new string[1] {"ta"}},
        {"��", new string[2] {"ti", "chi"}},
        {"��", new string[2] {"tu", "tsu"}},
        {"��", new string[1] {"te"}},
        {"��", new string[1] {"to"}},
        {"��", new string[1] {"na"}},
        {"��", new string[1] {"ni"}},
        {"��", new string[1] {"nu"}},
        {"��", new string[1] {"ne"}},
        {"��", new string[1] {"no"}},
        {"��", new string[1] {"ha"}},
        {"��", new string[1] {"hi"}},
        {"��", new string[2] {"hu", "fu"}},
        {"��", new string[1] {"he"}},
        {"��", new string[1] {"ho"}},
        {"��", new string[1] {"ma"}},
        {"��", new string[1] {"mi"}},
        {"��", new string[1] {"mu"}},
        {"��", new string[1] {"me"}},
        {"��", new string[1] {"mo"}},
        {"��", new string[1] {"ya"}},
        {"��", new string[1] {"yu"}},
        {"��", new string[1] {"yo"}},
        {"��", new string[1] {"ra"}},
        {"��", new string[1] {"ri"}},
        {"��", new string[1] {"ru"}},
        {"��", new string[1] {"re"}},
        {"��", new string[1] {"ro"}},
        {"��", new string[1] {"wa"}},
        {"��", new string[1] {"wo"}},
        {"��", new string[3] {"nn", "xn","n"}}, //�̂��̂���O����
        {"��", new string[1] {"ga"}},
        {"��", new string[1] {"gi"}},
        {"��", new string[1] {"gu"}},
        {"��", new string[1] {"ge"}},
        {"��", new string[1] {"go"}},
        {"��", new string[1] {"za"}},
        {"��", new string[2] {"zi", "ji"}},
        {"��", new string[1] {"zu"}},
        {"��", new string[1] {"ze"}},
        {"��", new string[1] {"zo"}},
        {"��", new string[1] {"da"}},
        {"��", new string[1] {"di"}},
        {"��", new string[1] {"du"}},
        {"��", new string[1] {"de"}},
        {"��", new string[1] {"do"}},
        {"��", new string[1] {"ba"}},
        {"��", new string[1] {"bi"}},
        {"��", new string[1] {"bu"}},
        {"��", new string[1] {"be"}},
        {"��", new string[1] {"bo"}},
        {"��", new string[1] {"pa"}},
        {"��", new string[1] {"pi"}},
        {"��", new string[1] {"pu"}},
        {"��", new string[1] {"pe"}},
        {"��", new string[1] {"po"}},

        //�������܂�
        {"����", new string[1] {"wha"}},
        {"����", new string[2] {"whi", "wi"}},
        {"����", new string[2] {"whe", "we"}},
        {"����", new string[1] {"who"}},
        {"����", new string[1] {"kya"}},
        {"����", new string[1] {"kyi"}},
        {"����", new string[1] {"kyu"}},
        {"����", new string[1] {"kye"}},
        {"����", new string[1] {"kyo"}},
        {"����", new string[1] {"gya"}},
        {"����", new string[1] {"gyi"}},
        {"����", new string[1] {"gyu"}},
        {"����", new string[1] {"gye"}},
        {"����", new string[1] {"gyo"}},
        {"����", new string[3] {"qwa", "qa", "kwa"}},
        {"����", new string[3] {"qwi", "qi", "qyi"}},
        {"����", new string[1] {"qwu"}},
        {"����", new string[3] {"qwe", "qe", "qye"}},
        {"����", new string[2] {"qwo", "qo"}},
        {"����", new string[1] {"qyu"}},
        {"����", new string[1] {"qye"}},
        {"����", new string[1] {"qyo"}},
        {"����", new string[1] {"gwa"}},
        {"����", new string[1] {"gwi"}},
        {"����", new string[1] {"gwu"}},
        {"����", new string[1] {"gwe"}},
        {"����", new string[1] {"gwo"}},
        {"����", new string[2] {"sya", "sha"}},
        {"����", new string[1] {"syi"}},
        {"����", new string[2] {"syu", "shu"}},
        {"����", new string[2] {"sye", "she"}},
        {"����", new string[2] {"syo", "sho"}},
        {"����", new string[3] {"zya", "ja", "jya"}},
        {"����", new string[2] {"zyi", "jyi"}},
        {"����", new string[3] {"zyu", "ju", "jyu"}},
        {"����", new string[3] {"zye", "je", "jye"}},
        {"����", new string[3] {"zyo", "jo", "jyo"}},
        {"����", new string[1] {"swa"}},
        {"����", new string[1] {"swi"}},
        {"����", new string[1] {"swu"}},
        {"����", new string[1] {"swe"}},
        {"����", new string[1] {"swo"}},
        {"����", new string[3] {"tya", "cha", "cya"}},
        {"����", new string[2] {"tyi", "cyi"}},
        {"����", new string[3] {"tyu", "chu", "cyu"}},
        {"����", new string[3] {"tye", "che", "cye"}},
        {"����", new string[3] {"tyo", "cho", "cyo"}},
        {"����", new string[1] {"dya"}},
        {"����", new string[1] {"dyi"}},
        {"����", new string[1] {"dyu"}},
        {"����", new string[1] {"dye"}},
        {"����", new string[1] {"dyo"}},
        {"��", new string[1] {"tsa"}},
        {"��", new string[1] {"tsi"}},
        {"��", new string[1] {"tse"}},
        {"��", new string[1] {"tso"}},
        {"�Ă�", new string[1] {"tha"}},
        {"�Ă�", new string[1] {"thi"}},
        {"�Ă�", new string[1] {"thu"}},
        {"�Ă�", new string[1] {"the"}},
        {"�Ă�", new string[1] {"tho"}},
        {"�ł�", new string[1] {"dha"}},
        {"�ł�", new string[1] {"dhi"}},
        {"�ł�", new string[1] {"dhu"}},
        {"�ł�", new string[1] {"dhe"}},
        {"�ł�", new string[1] {"dho"}},
        {"�Ƃ�", new string[1] {"twa"}},
        {"�Ƃ�", new string[1] {"twi"}},
        {"�Ƃ�", new string[1] {"twu"}},
        {"�Ƃ�", new string[1] {"twe"}},
        {"�Ƃ�", new string[1] {"two"}},
        {"�ǂ�", new string[1] {"dwa"}},
        {"�ǂ�", new string[1] {"dwi"}},
        {"�ǂ�", new string[1] {"dwu"}},
        {"�ǂ�", new string[1] {"dwe"}},
        {"�ǂ�", new string[1] {"dwo"}},
        {"�ɂ�", new string[1] {"nya"}},
        {"�ɂ�", new string[1] {"nyi"}},
        {"�ɂ�", new string[1] {"nyu"}},
        {"�ɂ�", new string[1] {"nye"}},
        {"�ɂ�", new string[1] {"nyo"}},
        {"�Ђ�", new string[1] {"hya"}},
        {"�Ђ�", new string[1] {"hyi"}},
        {"�Ђ�", new string[1] {"hyu"}},
        {"�Ђ�", new string[1] {"hye"}},
        {"�Ђ�", new string[1] {"hyo"}},
        {"�҂�", new string[1] {"pya"}},
        {"�҂�", new string[1] {"pyi"}},
        {"�҂�", new string[1] {"pyu"}},
        {"�҂�", new string[1] {"pye"}},
        {"�҂�", new string[1] {"pyo"}},
        {"�т�", new string[1] {"bya"}},
        {"�т�", new string[1] {"byi"}},
        {"�т�", new string[1] {"byu"}},
        {"�т�", new string[1] {"bye"}},
        {"�т�", new string[1] {"byo"}},
        {"�ӂ�", new string[1] {"fya"}},
        {"�ӂ�", new string[1] {"fyu"}},
        {"�ӂ�", new string[1] {"fyo"}},
        {"�ӂ�", new string[2] {"fwa", "fa"}},
        {"�ӂ�", new string[3] {"fwi", "fi", "fyi"}},
        {"�ӂ�", new string[1] {"fwu"}},
        {"�ӂ�", new string[3] {"fwe", "fe", "fye"}},
        {"�ӂ�", new string[2] {"fwo", "fo"}},
        {"�݂�", new string[1] {"mya"}},
        {"�݂�", new string[1] {"myi"}},
        {"�݂�", new string[1] {"myu"}},
        {"�݂�", new string[1] {"mye"}},
        {"�݂�", new string[1] {"myo"}},
        {"���", new string[1] {"rya"}},
        {"�股", new string[1] {"ryi"}},
        {"���", new string[1] {"ryu"}},
        {"�肥", new string[1] {"rye"}},
        {"���", new string[1] {"ryo"}},
        {"����", new string[1] {"va"}},
        {"����", new string[2] {"vi","vyi"}},
        {"��", new string[1] {"vu"}},
        {"����", new string[2] {"ve","vye"}},
        {"����", new string[1] {"vo"}},
        {"����", new string[1] {"vya"}},
        {"����", new string[1] {"vyu"}},
        {"����", new string[1] {"vyo"}},

        //������
        {"��", new string[2] {"la", "xa"}},
        {"��", new string[4] {"li", "lyi", "xi", "xyi"}},
        {"��", new string[2] {"lu", "xu"}},
        {"��", new string[4] {"le", "xe", "lye", "xye"}},
        {"��", new string[2] {"lo", "xo"}},
        {"��", new string[2] {"lka", "xka"}},
        {"��", new string[2] {"lke", "xke"}},
        {"��", new string[4] {"ltu", "xtu", "ltsu", "xtsu"}},
        {"��", new string[2] {"lya", "xya"}},
        {"��", new string[2] {"lyu", "xyu"}},
        {"��", new string[2] {"lyo", "xyo"}},
        {"��", new string[2] {"lwa", "xwa"}},


    };
}
