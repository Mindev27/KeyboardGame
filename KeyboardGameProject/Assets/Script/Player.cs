using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string currentKey; // 현재 위치
    public bool turnMove;

    private Dictionary<string, List<string>> adjList; // 인접리스트
    private InteractAttack interactAttack;

    void Start()
    {
        StartCoroutine(InitializeAdjList()); // 키보드 인접리스트가 완성될때까지 대기하기위함
        interactAttack = FindObjectOfType<InteractAttack>(); 
        turnMove = false;
    }

    private IEnumerator InitializeAdjList()
    {
        // ManageKeyBoard 인스턴스를 찾아 adjList를 가져옴
        ManageKeyBoard manageKeyBoard = null;
        while (manageKeyBoard == null)
        {
            manageKeyBoard = FindObjectOfType<ManageKeyBoard>();
            yield return null; // 한 프레임 기다림
        }

        while (manageKeyBoard.adjList == null || manageKeyBoard.adjList.Count == 0)
        {
            yield return null; // 한 프레임 기다림
        }

        adjList = manageKeyBoard.adjList;

        currentKey = "G";
        MoveToKey(currentKey);
    }


    public void MoveToKey(string key)//플레이어 이동시킴
    {
        key = "back_" + key;
        GameObject keyObj = GameObject.Find(key);
        if (keyObj != null)
        {
            transform.position = keyObj.transform.position;
            currentKey = key.Split('_')[1];
            interactAttack.InputStack(currentKey);//해당 칸 공격 스택에 삽입
            try
            {
                keyObj.transform.Find(currentKey).Find("Enemy1(Clone)").Find("Enemy").GetComponent<Enemy>().Attack();
            }
            catch
            {
                //Debug.Log("적 X(데미지)");
            }
        }
        turnMove = false;
        FindFirstObjectByType<Timer>().GameStart2();//이동 후 다음 단계 진행
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
        // 특수문자에 대한 검사 로직을 추가
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


    // 키로 이동시도 (거리가 1인경우 이동가능)
    private void AttemptMove(string key)
    {
        //Debug.Log(currentKey + "에서 " + key + "로 이동 시도");
        
        if (adjList[currentKey].Contains(key))
        {
            MoveToKey(key);
            //Debug.Log(currentKey + "에서 " + key + "로 이동");
        }
        else
        {
            //Debug.Log(currentKey + "와 " + key + "로의 연결 없음");
        }
    }

}
