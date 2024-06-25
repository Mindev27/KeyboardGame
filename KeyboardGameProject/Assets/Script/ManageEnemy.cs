using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageEnemy : MonoBehaviour
{
    public GameObject manageKeyboard;
    public GameObject enemyPrefab;
    List<ManageKeyBoard.key> keyBoard;

    // Start is called before the first frame update
    void Start()
    {
         keyBoard = manageKeyboard.GetComponent<ManageKeyBoard>().keyBoard;//����Ʈ ��������
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void SummonEnemy()// �� ����
    {
        int num = Random.Range(1, ManageKeyBoard.enemyV + 1);// ���� ������ �� �ִ� ĭ�� ����
        int i, now = 0;
        
        for(i = 0; i < ManageKeyBoard.numV; ++i)
        {
            if (keyBoard[i].isSuburb && !keyBoard[i].isEnemy)// ���� ���� ������ �ÿ� +1
            {
                now += 1;
            }
            if(now >= num)// ���� ���� ���������� �ߴ�
            {
                break;
            }
        }
        if (i < ManageKeyBoard.numV)
        {
            Debug.Log("num: " + num);
            Debug.Log("i: " + i);
            keyBoard[i].SetEnemy(true);// �ߴ� ������ �� ����
        }
    }

    public void CalcDelay()
    {
        for (int i = 0; i < ManageKeyBoard.numV; ++i)
        {
            keyBoard[i].Countdelay(enemyPrefab);
        }

        SummonEnemy();
    }

}