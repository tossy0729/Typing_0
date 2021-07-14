using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class typingdata {
    // �^�C�s���O�f�[�^
    //private string[] qJ = { "���", "�e�X�g", "�^�C�s���O", "���߂��߂����" };
    //private string[] qH = { "���񂾂�", "�Ă���", "�����҂�", "���߂��߂����" };
    //private string[] qR = { "monndai", "tesuto", "taipinngu", "kamekumechann" };
    private List<(string J, string H)> JH = new List<(string J, string H)>() {
        ("���","���񂾂�"),
        ("�e�X�g","�Ă���"),
        ("�^�C�s���O","�����҂�"),
        ("���߂��߂����","���߂��߂����")
    };
    
    public makeR R = new makeR();

    //�擾�p�֐�
    //�z��̉��Ԗڂ��������Ɏ��
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

    //��蕶�𐶐�����֐�
    //�����͖����ŁA���{�ꕶ�A�Ђ炪�ȕ��A���[�}�����̏��ɐ�������B�Ăяo���Ƃ��͂��ꂼ��Ă�ł�
    public (string Ja, string Hi, List<string> hiraganasep, List<List<string>> Ro) Get()
    {
        string Ja = "�ӂ����A�A�����l�̂܂܂��悧";
        string Hi = "�ӂ����A�A�����l�̂܂܂��悧";
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
