using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageEnemy : MonoBehaviour
{
    public GameObject manageKeyboard;
    //public GameObject enemyPrefab1;
    //public GameObject enemyPrefab2;

    private int summonDelay = 2; //�� ��ȯ �ֱ�(�÷��̾� �̵� ����)
    private int nowDelay;
    List<int> random;
    List<GameObject> enemyPrefabList;
    List<ManageKeyBoard.key> keyBoard;

    // Start is called before the first frame update
    void Start()
    {
        InitEnemyPrefab();
        keyBoard = manageKeyboard.GetComponent<ManageKeyBoard>().keyBoard;//����Ʈ ��������\
        random = new List<int>();
        nowDelay = summonDelay;
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void InitEnemyPrefab()//�� ������ ����Ʈȭ
    {
        enemyPrefabList = new List<GameObject>();
        for (int i = 1; i <= 3; i++)
        {
            enemyPrefabList.Add(Resources.Load<GameObject>("Prefab/" + "Enemy" + i.ToString()));//���¿��� ������ ��������
        }
    }

    public void SummonEnemy()// �� ����
    {
        if(nowDelay > 0)//�� ��ȯ �ֱ� ���
        {
            nowDelay -= 1;
            return;
        }
        random.Clear();
        int num = 0;
        int i;
        
        for(i = 0; i < ManageKeyBoard.numV; ++i)
        {
            do
            {
                num = Random.Range(0, ManageKeyBoard.numV);// ���� ������ �� �ִ� ĭ�� ����
            } while (random.Contains(num));

            Player player = FindObjectOfType<Player>();

            if (keyBoard[num].isSuburb && !keyBoard[num].isEnemy && player.currentKey != keyBoard[num].name)// ���� ���� ������ �ÿ� +1
                break;
            else
                random.Add(num);
        }
        if (i < ManageKeyBoard.numV)
        {
            keyBoard[num].SetEnemy(true);// �ߴ� ������ �� ����
        }
        nowDelay = summonDelay;
    }

    public void CalcDelay()
    {
        for (int i = 0; i < ManageKeyBoard.numV; ++i)
        {
            int enemyN = Random.Range(0, 3);//�� ���� �� ���� 1�� ����
            keyBoard[i].Countdelay(enemyPrefabList[enemyN]);//�� ���� �����̿� �Ѱ��ֱ�
        }

        SummonEnemy();
    }

    public void moveEnemy()//��ü �� ã�Ƽ� �̵�
    {
        foreach(ManageKeyBoard.key key in keyBoard)
        {
            try
            {
                GameObject.Find(key.name).GetComponent<Transform>().Find("Enemy1(Clone)").GetComponent<Enemy>().MoveToPlayer();
            }
            catch
            {
                Debug.Log("�� ���� X");
            }
        }
    }

}