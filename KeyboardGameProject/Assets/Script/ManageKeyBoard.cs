using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageKeyBoard : MonoBehaviour
{
    private Dictionary<string, List<string>> adjList;// ��������Ʈ
    public List<key> keyBoard;// �� ĭ�� �����ϴ� ����Ʈ
    public static int numV = 27;// �� ���� ����
    public static int enemyV = 20;// ���� ������ �� �ִ� ���� ����

    void Start()
    {
        KeyBoardInit();//��������Ʈ �ʱ�ȭ
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void KeyBoardInit()
    {
        adjList = new Dictionary<string, List<string>>();
        keyBoard = new List<key>();

        // Ű������ �� ���� ����
        //string[] row0 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" };
        string[] row1 = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
        string[] row2 = { "A", "S", "D", "F", "G", "H", "J", "K", "L" };
        string[] row3 = { "Z", "X", "C", "V", "B", "N", "M", "<"};

        // �� ���� ���� ����
        AddEdges(row1, true);
        AddEdges(row2, false);
        AddEdges(row3, true);

        // �� ������ ������ Ű �߰�
        AddRowNeighbors(row1, row2);
        AddRowNeighbors(row2, row3);
    }

    private void AddEdges(string[] row, bool suburb)//�� ���� ���� ����
    {
        for (int i = 0; i < row.Length; i++)
        {
            key temp = new key(row[i]);
            temp.isSuburb = suburb;
            if (!adjList.ContainsKey(row[i]))
            {
                adjList[row[i]] = new List<string>();
            }
            if (i > 0)//�¿� ���� �߰�
            {
                adjList[row[i]].Add(row[i - 1]);
            }
            else
            {
                temp.isSuburb = true;
            }
            if (i < row.Length - 1)
            {
                adjList[row[i]].Add(row[i + 1]);
            }
            else
            {
                temp.isSuburb = true;
            }
            keyBoard.Add(temp);

        }
    }
    private void AddRowNeighbors(string[] upperRow, string[] lowerRow)// �� ������ ������ Ű �߰�
    {
        for (int i = 0; i < upperRow.Length; i++)
        {
            string upperKey = upperRow[i];
            if (adjList.ContainsKey(upperKey))
            {
                int lowerIndex = i - 1;
                if (lowerIndex >= 0)// ���� �Ʒ�ĭ ����
                {
                    adjList[upperKey].Add(lowerRow[lowerIndex]);
                }

                if (i < lowerRow.Length)//���� �Ʒ�ĭ ����
                {
                    adjList[upperKey].Add(lowerRow[i]);
                }
            }
        }
    }


    public struct key//�� ĭ
    {
        public string name;
        public int attack;// ���� �Ҵ�
        public bool isEnemy;// �� ���� ����
        public bool isSuburb;// �ֿܰ� ����

        public key(string name)
        {
            this.name = name;
            attack = -1;//������ �Ҵ� �ȵ� ����
            isEnemy = false;
            isSuburb = false;
        }

        public void setEnemy(bool enemy)//�� ���� ����
        {
            this.isEnemy = enemy;
            if (enemy)//���� �����Ǹ� �� ���� -> ���� ��������Ʈ �������� ����
            {
                GameObject.Find(this.name).GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                GameObject.Find(this.name).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

    }

}
