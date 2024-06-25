using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string currentKey; // ���� ��ġ
    private Dictionary<string, List<string>> adjList; // ��������Ʈ

    void Start()
    {
        StartCoroutine(InitializeAdjList()); // Ű���� ��������Ʈ�� �ϼ��ɶ����� ����ϱ�����
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

        currentKey = "A";
        MoveToKey(currentKey);

        PrintAdjList(); // adjList ���
    }


    public void MoveToKey(string key)
    {
        currentKey = key;
        GameObject keyObj = GameObject.Find(key);
        if (keyObj != null)
        {
            transform.position = keyObj.transform.position;
        }
    }

    void Update()
    {
        KeyInput();
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
                    if (key.Length == 1 && char.IsLetter(key[0]))
                    {
                        AttemptMove(key.ToUpper());
                    }
                }
            }
        }
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

    // ������
    private void PrintAdjList()
    {
        string[] row1 = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
        string[] row2 = { "A", "S", "D", "F", "G", "H", "J", "K", "L" };
        string[] row3 = { "Z", "X", "C", "V", "B", "N", "M", "<" };

        Debug.Log("Row 1:");
        PrintRowAdjList(row1);

        Debug.Log("Row 2:");
        PrintRowAdjList(row2);

        Debug.Log("Row 3:");
        PrintRowAdjList(row3);
    }

    private void PrintRowAdjList(string[] row)
    {
        foreach (var key in row)
        {
            if (adjList.ContainsKey(key))
            {
                string adjacentKeys = string.Join(", ", adjList[key]);
                Debug.Log("Key: " + key + " -> Adjacent Keys: " + adjacentKeys);
            }
        }
    }
}
