using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeR 
{
    //���ς���̃u���O���Q��
    //https://qiita.com/Arthur_Lugh/items/43b61877819e402c50d6

    // �Ђ炪�ȕ����󂯎��A���̓p�^�[�����o�͂���
    public List<string> convert(string s)
    {
        var pattern = new List<string>(); // ���[�}���̓��̓p�^�[��

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
    //���̓p�^�[����dictionary
    //��Ή��ӏ�������ΐ����ǉ�
    private Dictionary<string, string[]> HR = new Dictionary<string, string[]>
    {
        ;
    }
}
