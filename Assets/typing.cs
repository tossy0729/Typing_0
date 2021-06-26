using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typing : MonoBehaviour
{
    // Start is called before the first frame update
    
    private string[] qJ = { "���", "�e�X�g", "�^�C�s���O", "���߂��߂����" };
    private string[] qH = { "���񂾂�", "�Ă���", "�����҂�", "���߂��߂����" };
    private string[] qR = { "monndai", "tesuto", "taipinngu", "kamekumechann" };
    //�@�\���e�L�X�g
    private Text UIJ;
    private Text UIR;
    private Text UIH;

    //�@���
    private string nQJ;
    private string nQR;

    //�@���ԍ�
    private int numberOfQuestion;
    //�@���͂���������e�L�X�g
    private Text UII;
    //�@����
    private int correctN;
    //�@���𐔕\���p�e�L�X�gUI
    private Text UIcorrectA;
    //�@������������������Ă���
    private string correctString;
    //�@���s��
    private int mistakeN;
    //�@���s���\���p�e�L�X�gUI
    private Text UImistake;
    //�@����
    private float correctAR;
    //�@���𗦕\���p�e�L�X�gUI
    private Text UIcorrectAR;

    void Start()
    {
        //�@�e�L�X�gUI���擾
        UIJ = transform.Find("InputPanel/QuestionJ").GetComponent<Text>();
        UIR = transform.Find("InputPanel/QuestionR").GetComponent<Text>();
        UII = transform.Find("InputPanel/Input").GetComponent<Text>();
        UIH = transform.Find("InputPanel/Hiragana").GetComponent<Text>();
        
        UIcorrectA = transform.Find("DataPanel/Correct Answer").GetComponent<Text>();
        UImistake = transform.Find("DataPanel/Mistake").GetComponent<Text>();
        UIcorrectAR = transform.Find("DataPanel/Correct Answer Rate").GetComponent<Text>();

        //�@�f�[�^����������
        correctN = 0;
        UIcorrectA.text = correctN.ToString();
        mistakeN = 0;
        UImistake.text = mistakeN.ToString();
        correctAR = 0;
        UIcorrectAR.text = correctAR.ToString();

        OutputQ();
    }

    private int indexOfString;

    // Update is called once per frame
    void Update()
    {
        // esc�������ꂽ��I��
        if (Input.GetKey(KeyCode.Escape)) Quit();

        //�@�����Ă��镶���ƃL�[�{�[�h����ł����������������ǂ���
        if (Input.GetKeyDown(nQR[indexOfString].ToString()))
        {
            //�@�������̏������Ăяo��
            Correct();
            //�@������͂��I�����玟�̖���\��
            if (indexOfString >= nQR.Length)
            {
                OutputQ();
            }
        }
        else if (Input.anyKeyDown)
        {
            //�@���s���̏������Ăяo��
            Mistake();
        }
    }
    //�@�V��������\�����郁�\�b�h
    void OutputQ()
    {
        //�@�e�L�X�gUI������������
        UIJ.text = "";
        UIR.text = "";
        UII.text = "";

        //�@���������������������
        correctString = "";
        //�@�����̈ʒu��0�Ԗڂɖ߂�
        indexOfString = 0;
        //�@��萔���Ń����_���ɑI��
        numberOfQuestion = Random.Range(0, qJ.Length);

        //�@�I�����������e�L�X�gUI�ɃZ�b�g
        nQJ = qJ[numberOfQuestion];
        nQR = qR[numberOfQuestion];
        UIJ.text = nQJ;
        UIR.text = nQR;
        UIH.text = qH[numberOfQuestion];
    }
    //�@�^�C�s���O�������̏���
    void Correct()
    {
        //�@���𐔂𑝂₷
        correctN++;
        UIcorrectA.text = correctN.ToString();
        //�@���𗦂̌v�Z
        CorrectAnswerRate();
        //�@��������������\��
        correctString += nQR[indexOfString].ToString();
        UII.text = correctString;
        //�@���̕������w��
        indexOfString++;
    }

    //�@�^�C�s���O���s���̏���
    void Mistake()
    {
        //�@���s���𑝂₷�i���������ɂ��Ή�������j
        mistakeN += Input.inputString.Length;

        UImistake.text = mistakeN.ToString();
        //�@���𗦂̌v�Z
        CorrectAnswerRate();
        //�@���s����������\��
        if (Input.inputString != "")
        {
            UII.text = correctString + "<color=#ff0000ff>" + Input.inputString + "</color>";
        }
    }

    //�@���𗦂̌v�Z����
    void CorrectAnswerRate()
    {
        //�@���𗦂̌v�Z
        correctAR = 100f * correctN / (correctN + mistakeN);
        //�@�����_�ȉ��̌������킹��
        UIcorrectAR.text = correctAR.ToString("0.00");
    }

    // �Q�[�����I������֐�
    void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
        #endif
    }
}
