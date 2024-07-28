using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string currentKey; // ���� ��ġ
    public bool turnMove;

    private Dictionary<string, List<string>> adjList; // ��������Ʈ
    private InteractAttack interactAttack;

    void Start()
    {
        StartCoroutine(InitializeAdjList()); // Ű���� ��������Ʈ�� �ϼ��ɶ����� ����ϱ�����
        interactAttack = FindObjectOfType<InteractAttack>();
        turnMove = false;
    }

    private IEnumerator InitializeAdjList()
    {
        // ManageKeyBoard �ν��Ͻ��� ã�� adjList�� ������
        ManageKeyBoard manageKeyBoard = null;
        while (manageKeyBoard == null)
        {
            manageKeyBoard = FindObjectOfType<ManageKeyBoard>();
            yield return null; // �� ������ ��ٸ�
        }

        while (manageKeyBoard.adjList == null || manageKeyBoard.adjList.Count == 0)
        {
            yield return null; // �� ������ ��ٸ�
        }

        adjList = manageKeyBoard.adjList;

        currentKey = "G";
        MoveToKey(currentKey);
    }


    public void MoveToKey(string key)
    {
        key = "back_" + key;
        GameObject keyObj = GameObject.Find(key);
        if (keyObj != null)
        {
            transform.position = keyObj.transform.position;
            currentKey = key.Split('_')[1];
            interactAttack.InputStack(currentKey);
        }
        turnMove = false;
        FindFirstObjectByType<Timer>().GameStart2();
    }

    void Update()
    {
        if (turnMove)
        {
            KeyInput();
        }
    }

    private void KeyInput()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    string key = keyCode.ToString();
                    if (key.Length == 1 && (char.IsLetter(key[0]) || IsSpecialCharacter(key[0])))
                    {
                        AttemptMove(key.ToUpper());
                    }
                    switch (keyCode)
                    {
                        case KeyCode.Alpha1:
                            AttemptMove("1");
                            break;
                        case KeyCode.Alpha2:
                            AttemptMove("2");
                            break;
                        case KeyCode.Alpha3:
                            AttemptMove("3");
                            break;
                        case KeyCode.Alpha4:
                            AttemptMove("4");
                            break;
                        case KeyCode.Alpha5:
                            AttemptMove("5");
                            break;
                        case KeyCode.Alpha6:
                            AttemptMove("6");
                            break;
                        case KeyCode.Alpha7:
                            AttemptMove("7");
                            break;
                        case KeyCode.Alpha8:
                            AttemptMove("8");
                            break;
                        case KeyCode.Alpha9:
                            AttemptMove("9");
                            break;
                        case KeyCode.Alpha0:
                            AttemptMove("0");
                            break;
                        case KeyCode.Minus:
                            AttemptMove("-");
                            break;
                        case KeyCode.Equals:
                            AttemptMove("+");
                            break;
                        case KeyCode.LeftBracket:
                            AttemptMove("{");
                            break;
                        case KeyCode.RightBracket:
                            AttemptMove("}");
                            break;
                        case KeyCode.Semicolon:
                            AttemptMove(":");
                            break;
                        case KeyCode.Quote:
                            AttemptMove("\"");
                            break;
                        case KeyCode.Comma:
                            AttemptMove("<");
                            break;
                        case KeyCode.Period:
                            AttemptMove(">");
                            break;
                        case KeyCode.Slash:
                            AttemptMove("?");
                            break;
                    }
                }
            }
        }
    }

    private bool IsSpecialCharacter(char c)
    {
        // Ư�����ڿ� ���� �˻� ������ �߰�
        char[] specialCharacters = { '-', '=', '[', ']', ';', '\'', ',', '.', '/' };
        foreach (char specialCharacter in specialCharacters)
        {
            if (c == specialCharacter)
            {
                return true;
            }
        }
        return false;
    }


    // Ű�� �̵��õ� (�Ÿ��� 1�ΰ�� �̵�����)
    private void AttemptMove(string key)
    {
        Debug.Log(currentKey + "���� " + key + "�� �̵� �õ�");
        
        if (adjList[currentKey].Contains(key))
        {
            MoveToKey(key);
            Debug.Log(currentKey + "���� " + key + "�� �̵�");
        }
        else
        {
            Debug.Log(currentKey + "�� " + key + "���� ���� ����");
        }
    }

}
