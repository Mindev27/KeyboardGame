using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageEnemy : MonoBehaviour
{
    public GameObject manageKeyboard;
    public int setEnemyDelay; //������ �� ���� ���� ���ϴ� ������

    private int nowDelay;
    private Player player;
    private List<int> random;
    private List<ManageKeyBoard.key> keyBoard;

    // Start is called before the first frame update
    void Start()
    {
        keyBoard = manageKeyboard.GetComponent<ManageKeyBoard>().keyBoard;//����Ʈ ��������
        random = new List<int>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void SummonEnemy()// �� ����
    {
        if (nowDelay > 0)
        {
            nowDelay -= 1;
            return;
        }

        nowDelay = setEnemyDelay;
        random.Clear();
        int num = 0;
        int i;
        
        for (i = 0; i < ManageKeyBoard.numV; ++i)
        {
            do
            {
                num = Random.Range(0, ManageKeyBoard.numV);// ���� ������ �� �ִ� ĭ�� ����
            } while (random.Contains(num));

            

            if (keyBoard[num].isSuburb && !keyBoard[num].isEnemy && (player.currentKey != keyBoard[num].name))// ���� ���� ������ �ÿ� +1
                break;
            else
                random.Add(num);
        }
        if (i < ManageKeyBoard.numV)
        {
            keyBoard[num].SetEnemy(true);// �ߴ� ������ �� ����
        }
    }

    public void CalcDelay()
    {
        for (int i = 0; i < ManageKeyBoard.numV; ++i)
        {
            int enemyPrefab = Random.Range(1, 4);//�� ���� �������� ��ȯ
            keyBoard[i].Countdelay(Resources.Load<GameObject>("Prefab/" + "enemy" + enemyPrefab.ToString()));//�� ĭ�� �� ��ȯ ����(������ ���̸� ��ȯX)
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