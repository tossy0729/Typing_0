using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class typingdata { 
    // �^�C�s���O�f�[�^
    private string[] qJ = { "���", "�e�X�g", "�^�C�s���O", "���߂��߂����" };
    private string[] qH = { "���񂾂�", "�Ă���", "�����҂�", "���߂��߂����" };
    private string[] qR = { "monndai", "tesuto", "taipinngu", "kamekumechann" };

    //�擾�p�֐�
    //�z��̉��Ԗڂ��������Ɏ��
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
