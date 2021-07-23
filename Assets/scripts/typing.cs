using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class typing : MonoBehaviour
{ 
    //�^�C�s���O�f�[�^�N���X�̃C���X�^���X
    public typingdata td = new typingdata();
    //�@�\���e�L�X�g
    private Text UIJ;
    private Text UIH;
    //�@���͂���������e�L�X�g
    private Text UII;

    //���͂��ꂽ������queue
    private static Queue<char> Inqueue = new Queue<char>();
    //���͂��ꂽ����������queue
    private static Queue<double> Timequeue = new Queue<double>();

    //����
    private static double JudgeTime;
    // �~�X�^�C�v�L�^
    private static bool isRecMistype;

    //�@������������������Ă���
    private string correctString;
    // ���͎󂯕t��
    private static bool isInputValid;

    //���T�C�Y
    //private int Qsize = 4;

    //index
    private static int index; // �����Ă��镔����index
    private static List<List<int>> indexAdd = new List<List<int>>();
    private static List<List<int>> sentenceIndex = new List<List<int>>();
    private static List<List<int>> sentenceValid = new List<List<int>>();//Rpattern�ƑΉ����Ă��̕\�L�̎d�����Ó����ǂ����𔻒f

    //�@���
    string nQJ, nQH, nQR;
    //���[�}���̓��̓p�^�[��
    private static List<List<string>> Rpattern;

    //�@���ԍ�
    private int numberOfQuestion;
    //�@����
    private float correctAR;

    //�@���𐔂Ǝ��s��
    private int correctN, mistakeN;
    //�@���𐔕\���p�Ƃ��e�L�X�gUI
    private Text UIcorrectA, UImistake, UIcorrectAR;
    // ���̓^�C�s���O�ǂ�
    private static List<List<string>> sentenceTyping;
    // �Z���e���X�̒���
    private static int sentenceLength;




    void Start()
    {
        //�@�f�[�^����������
        init();

        OutputQ();
    }
    void init()
    {
        //�@�e�L�X�gUI���擾
        UIJ = transform.Find("InputPanel/QuestionJ").GetComponent<Text>();
        UII = transform.Find("InputPanel/Input").GetComponent<Text>();
        UIH = transform.Find("InputPanel/Hiragana").GetComponent<Text>();

        UIcorrectA = transform.Find("DataPanel/Correct Answer").GetComponent<Text>();
        UImistake = transform.Find("DataPanel/Mistake").GetComponent<Text>();
        UIcorrectAR = transform.Find("DataPanel/Correct Answer Rate").GetComponent<Text>();
        correctN = 0;
        mistakeN = 0;
        correctAR = 0;
        UIcorrectA.text = "correct number:" + correctN.ToString();
        UImistake.text = "miss:" + mistakeN.ToString();
        UIcorrectAR.text = "correct rate:" + correctAR.ToString();
        JudgeTime = -1.0;
        isRecMistype = false;
        isInputValid = true;
        Inqueue.Clear();
        Timequeue.Clear();
    }

    //private int indexOfString;

    // Update is called once per frame
    void Update()
    {
        // esc�������ꂽ��I��
        if (Input.GetKey(KeyCode.Escape)) Quit();

        if(Inqueue.Count > 0 && Timequeue.Count > 0)
        {
            IsOk();
        }
    }
    //�@�V��������\�����郁�\�b�h
    void OutputQ()
    {
        //�@�e�L�X�gUI������������
        UIJ.text = "";
        UIH.text = "";
        index = 0;
        sentenceLength = 0;
        //�@���������������������
        correctString = "";
        //�@�����̈ʒu��0�Ԗڂɖ߂�
        //indexOfString = 0;

        //�@�I�����������e�L�X�gUI�ɃZ�b�g
        //���̃C���X�^���X���Q�b�g
        var t = td.Get();
        //���ꂼ���UI�ɃZ�b�g����
        nQJ = t.Ja;
        nQR = "";
        nQH = t.Hi;
        Rpattern = t.Ro;
        InitSentenceData();
        for (int i = 0; i < Rpattern.Count; i++)
        {
            nQR = String.Concat(nQR,Rpattern[i][0]);
        }
        UIJ.text = "<color=#000000>" + nQJ + "</color>";
        UIH.text = "<color=#808080>" + nQH + "</color>" ;
        UII.text = nQR;
        isInputValid = true;
    }
    //���딻��̏�����
    void InitSentenceData()
    {
        var Senlen = Rpattern.Count;
        sentenceIndex.Clear();
        sentenceValid.Clear();
        indexAdd.Clear();
        //������Ő��딻��A���̃��[�}���\�L�͑Ó����ǂ����Ȃǂ𔻒肷��
        sentenceIndex = new List<List<int>>();
        sentenceValid = new List<List<int>>();
        indexAdd = new List<List<int>>();
        for (int i = 0; i < Senlen; i++)
        {
            var typenum = Rpattern[i].Count;
            sentenceIndex.Add(new List<int>());
            sentenceValid.Add(new List<int>());
            indexAdd.Add(new List<int>());
            for (int j = 0; j < typenum; ++j)
            {
                sentenceIndex[i].Add(0);
                sentenceValid[i].Add(1);
                indexAdd[i].Add(0);

            }

        }
    }

    //�^�C�s���O�̐��딻��
    void IsOk()
    {
        //���͂�queue���������
        while (Inqueue.Count > 0)
        {
            //Inqueue�̐擪�̕����𓾂�
            char inchar = Inqueue.Peek();
            Inqueue.Dequeue();
            double keypressedtime = Timequeue.Peek();
            Timequeue.Dequeue();
            //�Ō�ɔ��肳�ꂽ���Ԃ����O�ɉ�����Ă����瑱����
            if (keypressedtime <= JudgeTime) { continue; }
            JudgeTime = keypressedtime;

            //�~�X���ǂ����𒲂ׂ�
            bool isMiss = true;

            //�����Ă���Ђ炪��1�����ɂ��Ẵp�^�[���𒲂ׂ�
            for (int i = 0; i < Rpattern[index].Count; i++)
            {
                //invalid�Ȃ�p�X
                if (sentenceValid[index][i] == 0)
                {
                    continue;
                }
                int j = sentenceIndex[index][i];
                char nextinchar = Rpattern[index][i][j];
                //�����^�C�v
                if (inchar == nextinchar)
                {
                    isMiss = false;
                    indexAdd[index][i] = 1;
                }
                else indexAdd[index][i] = 0;

            }
            if (!isMiss) Correct(inchar.ToString());
            else
            {
                
                Mistake();
            }

        }
    }

        //�L�[���͂̎󂯎��
        



        //�@�^�C�s���O�������̏���
    void Correct(string s)
    {
        //�@���𐔂𑝂₷
        correctN++;
        sentenceLength++;
        UIcorrectA.text = "correct number:" + correctN.ToString();
        //�@���𗦂̌v�Z
        CorrectAnswerRate();
        //�@���̕������w��
        isRecMistype = false;
        
        //�\�ȓ��̓p�^�[���̃`�F�b�N
        bool isIndexCountUp = CheckValidSentence(s);
        UpdateSentence(s);
        if (isIndexCountUp)
        {
            index++;
        }
        if (index >= Rpattern.Count)
        {
            CompleteTask();
        }

    }

        //�@�^�C�s���O���s���̏���
    void Mistake()
    {
        //�@���s���𑝂₷�i���������ɂ��Ή�������j
        mistakeN++;

        UImistake.text = "miss:" + mistakeN.ToString();
        //�@���𗦂̌v�Z
        CorrectAnswerRate();
        // �łׂ�������Ԃ��\��
        if (!isRecMistype)
        {
            string cor = "<color=#00bfff>" + correctString + "</color>";
            string s = UII.text.ToString().Replace(cor,"");
            string rest = s.Substring(1);
            string red = "<color=#ff0000>" + s[0].ToString() + "</color>";
            UII.text = cor + red + rest;
        }
        isRecMistype = true;
    }

    //�@���𗦂̌v�Z����
    void CorrectAnswerRate()
    {
        //�@���𗦂̌v�Z
        correctAR = 100f * correctN / (correctN + mistakeN);
        //�@�����_�ȉ��̌������킹��
        UIcorrectAR.text = "correct rate:" + correctAR.ToString("0.00");
    }
        
        /// ��ʏ�ɕ\������ł����̕\�����X�V����
    void UpdateSentence(string s)
    {
        ////�ł��I����������͏���
        string tmpTypingSentence = "";
        for (int i = 0; i < Rpattern.Count; ++i)
        {
            if (i < index)
            {
                continue;
            }
            for (int j = 0; j < Rpattern[i].Count; ++j)
            {
                if (index == i && sentenceValid[index][j] == 0)
                {
                    continue;
                }
                else if (index == i && sentenceValid[index][j] == 1)
                {
                    for (int k = 0; k < Rpattern[index][j].Length; ++k)
                    {
                        if (k >= sentenceIndex[index][j])
                        {
                            tmpTypingSentence += Rpattern[index][j][k].ToString();
                        }
                    }
                    break;
                }
                else if (index != i && sentenceValid[i][j] == 1)
                {
                    tmpTypingSentence += Rpattern[i][j];
                    break;
                }
            }
        }
        //UII.text = tmpTypingSentence;
        if (!isRecMistype)
        {
            correctString += s;
            UII.text = "<color=#00bfff>" + correctString + "</color>" + tmpTypingSentence;
        }

    }
    void CompleteTask()
    {
        
        Inqueue.Clear();
        Timequeue.Clear();
        isInputValid = false;
        OutputQ();
    }
        //�L���p�^�[���̃`�F�b�N
    bool CheckValidSentence(string str)
    {
        bool ret = false;
        // �\�ȓ��̓p�^�[�����c��
        for (int i = 0; i < Rpattern[index].Count; ++i)
        {
            // str �ƈ�v���Ȃ����̂𖳌�������
            if (!str.Equals(Rpattern[index][i][sentenceIndex[index][i]].ToString()))
            {
                sentenceValid[index][i] = 0;
            }
            // ���̃L�[��
            sentenceIndex[index][i] += indexAdd[index][i];
            // ���̕�����
            if (sentenceIndex[index][i] >= Rpattern[index][i].Length)
            {
                ret = true;
            }
        }
        return ret;
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
    void OnGUI()
    {
        
        Event e = Event.current;
        if (isInputValid && e.type == EventType.KeyDown && e.type != EventType.KeyUp)
        {
            if (e.character == '\0')
            {
                var inputchar = ConvertKeyCodeToChar(e.keyCode);
                Inqueue.Enqueue(inputchar);
                Timequeue.Enqueue(Time.realtimeSinceStartup);
            }
            
            
        }
    }
    //�L�[�R�[�h����char�ւ̕ϊ�
    char ConvertKeyCodeToChar(KeyCode kc)
        {
        switch (kc)
        {
            case KeyCode.A:
                return 'a';
            case KeyCode.B:
                return 'b';
            case KeyCode.C:
                return 'c';
            case KeyCode.D:
                return 'd';
            case KeyCode.E:
                return 'e';
            case KeyCode.F:
                return 'f';
            case KeyCode.G:
                return 'g';
            case KeyCode.H:
                return 'h';
            case KeyCode.I:
                return 'i';
            case KeyCode.J:
                return 'j';
            case KeyCode.K:
                return 'k';
            case KeyCode.L:
                return 'l';
            case KeyCode.M:
                return 'm';
            case KeyCode.N:
                return 'n';
            case KeyCode.O:
                return 'o';
            case KeyCode.P:
                return 'p';
            case KeyCode.Q:
                return 'q';
            case KeyCode.R:
                return 'r';
            case KeyCode.S:
                return 's';
            case KeyCode.T:
                return 't';
            case KeyCode.U:
                return 'u';
            case KeyCode.V:
                return 'v';
            case KeyCode.W:
                return 'w';
            case KeyCode.X:
                return 'x';
            case KeyCode.Y:
                return 'y';
            case KeyCode.Z:
                return 'z';
            case KeyCode.Minus:
                return '-';
            case KeyCode.Alpha1:
                return '1';
            case KeyCode.Alpha2:
                return '2';
            case KeyCode.Alpha3:
                return '3';
            case KeyCode.Alpha4:
                return '4';
            case KeyCode.Alpha5:
                return '5';
            case KeyCode.Alpha6:
                return '6';
            case KeyCode.Alpha7:
                return '7';
            case KeyCode.Alpha8:
                return '8';
            case KeyCode.Alpha9:
                return '9';
            case KeyCode.Alpha0:
                return '0';
            default:
                return '\\';
        }
        }
    }
