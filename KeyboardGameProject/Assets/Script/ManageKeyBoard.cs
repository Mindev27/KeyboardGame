using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageKeyBoard : MonoBehaviour
{
    public Dictionary<string, List<string>> adjList;// ��������Ʈ
    public List<key> keyBoard;// �� ĭ�� �����ϴ� ����Ʈ
    public static int numV = 45;// �� ���� ����
    public static int enemyV = 26;// ���� ������ �� �ִ� ���� ����

    void Start()
    {
        KeyBoardInit(); //��������Ʈ �ʱ�ȭ
        // PrintAdjList(); 
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
        string[] row0 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" };
        string[] row1 = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}"};
        string[] row2 = { "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\""};
        string[] row3 = { "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?"};

        // �� ���� ���� ����
        AddEdges(row0, true);
        AddEdges(row1, false);
        AddEdges(row2, false);
        AddEdges(row3, true);

        // �� ������ ������ Ű �߰�
        AddRowNeighbors(row0, row1);
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
                    adjList[lowerRow[lowerIndex]].Add(upperKey);
                }

                if (i < lowerRow.Length)//���� �Ʒ�ĭ ����
                {
                    adjList[upperKey].Add(lowerRow[i]);
                    adjList[lowerRow[i]].Add(upperKey);
                }
            }
        }
    }

    public class key//�� ĭ
    {
        
        public string name;
        public int attack;// ���� �Ҵ�
        public int delay;
        public bool isEnemy;// �� ���� ����
        public bool isSuburb;// �ֿܰ� ����
        public bool isAttack;

        public key(string name)
        {
            this.name = name;
            attack = -1;//������ �Ҵ� �ȵ� ����
            delay = -1;
            isEnemy = false;
            isSuburb = false;
            isAttack = false;
        }

        public void Countdelay(GameObject enemy)
        {
            if (delay >= 0)
            {
                delay -= 1;
            }
            if (delay == 0)
            {
                GameObject enemy1 = Instantiate(enemy, GameObject.Find(this.name).transform);
                enemy1.SetActive(true);
                GameObject.Find("back_" + this.name).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        public void SetEnemy(bool enemy)//�� ���� ����
        {
            this.isEnemy = enemy;
            if (enemy)//���� �����Ǹ� �� ���� -> ���� ��������Ʈ �������� ����
            {
                GameObject.Find("back_" + this.name).GetComponent<SpriteRenderer>().color = Color.red;
                 this.delay = 1;
            }
        }
    }

    
}
