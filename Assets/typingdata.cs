using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class typingdata { 
    // �^�C�s���O�f�[�^
    private string[] qJ = { "���", "�e�X�g", "�^�C�s���O", "���߂��߂����" };
    private string[] qH = { "���񂾂�", "�Ă���", "�����҂�", "���߂��߂����" };
    private string[] qR = { "monndai", "tesuto", "taipinngu", "kamekumechann" };

    private List<(string J, string H)> JH = new List<(string J, string H)>()
    {
        ("���","���񂾂�"), ("�e�X�g","�Ă���"), ("�^�C�s���O","�����҂�")
    };

    public makeR R = new makeR();

    //�擾�p�֐�
    //�z��̉��Ԗڂ��������Ɏ��
    public string GetJ(int num)
    {
        return this.qJ[num];
    }
    public string GetH(int num)
    {
        return this.JH[num].J;
    }
    public List<List<string>> GetR(int num)
    {
        List<List<string>> Romaji = R.Convert(JH[num].H);
        return Romaji;
    }
}
