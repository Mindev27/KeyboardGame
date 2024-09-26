using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageKeyBoard : MonoBehaviour
{
    public Dictionary<string, List<string>> adjList;// 인접리스트
    public List<key> keyBoard;// 각 칸을 보관하는 리스트


    public static int numV = 45;// 총 정점 개수
    public static int enemyV = 26;// 적이 생성될 수 있는 정점 개수

    void Start()
    {
        KeyBoardInit(); //인접리스트 초기화
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

        // 키보드의 각 행을 정의
        string[] row0 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "+" };
        string[] row1 = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}"};
        string[] row2 = { "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\""};
        string[] row3 = { "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?"};

        // 행 내의 간선 연결
        AddEdges(row0, true);
        AddEdges(row1, false);
        AddEdges(row2, false);
        AddEdges(row3, true);

        // 행 사이의 인접한 키 추가
        AddRowNeighbors(row0, row1);
        AddRowNeighbors(row1, row2);
        AddRowNeighbors(row2, row3);
    }

    private void AddEdges(string[] row, bool suburb)//행 내의 간선 연결
    {
        for (int i = 0; i < row.Length; i++)
        {
            key temp = new key(row[i]);
            temp.isSuburb = suburb;
            if (!adjList.ContainsKey(row[i]))
            {
                adjList[row[i]] = new List<string>();
            }
            if (i > 0)//좌우 간선 추가
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
    private void AddRowNeighbors(string[] upperRow, string[] lowerRow)// 행 사이의 인접한 키 추가
    {
        for (int i = 0; i < upperRow.Length; i++)
        {
            string upperKey = upperRow[i];
            if (adjList.ContainsKey(upperKey))
            {
                int lowerIndex = i - 1;
                if (lowerIndex >= 0)// 좌측 아래칸 연결
                {
                    adjList[upperKey].Add(lowerRow[lowerIndex]);
                    adjList[lowerRow[lowerIndex]].Add(upperKey);
                }

                if (i < lowerRow.Length)//우측 아래칸 연결
                {
                    adjList[upperKey].Add(lowerRow[i]);
                    adjList[lowerRow[i]].Add(upperKey);
                }
            }
        }
    }

    public class key//각 칸
    {
        public string name;
        public int attack;// 공격 할당
        public bool isEnemy;// 적 존재 유무
        public bool isSuburb;// 최외곽 유무
        public bool isAttack;

        private int enemyDelay = 2;//적 생성 딜레이
        private int delay;

        public key(string name)
        {
            this.name = name;
            attack = -1;//공격이 할당 안된 상태
            delay = -1;
            isEnemy = false;
            isSuburb = false;
            isAttack = false;
        }

        public void Countdelay(GameObject enemy)//스프라이트 생성
        {
            if (delay >= 0)
            {
                delay -= 1;
            }
            if (delay == 0)
            {
                GameObject enemy1 = Instantiate(enemy, GameObject.Find(this.name).transform);
                enemy1.name = "Enemy1(Clone)";
                enemy1.SetActive(true);
                GameObject.Find("back_" + this.name).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        public void SetEnemy(bool enemy)//적 생성 여부
        {
            this.isEnemy = enemy;
            if (enemy)//적이 생성되면 색 변경 
            {
                GameObject.Find("back_" + this.name).GetComponent<SpriteRenderer>().color = Color.red;
                this.delay = enemyDelay;
            }
        }

        public void ChangeCreateDelay(int delay)
        {
            enemyDelay = delay;
        }

    }

    
}
