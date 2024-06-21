using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageEnemy : MonoBehaviour
{
    public GameObject manageKeyboard;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(ManageKeyBoard.numV);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonEnemy()// �� ����
    {
        int num = Random.Range(1, ManageKeyBoard.enemyV);// ���� ������ �� �ִ� ĭ�� ����
        int i, now = 0;
        List<ManageKeyBoard.key> keyBoard = manageKeyboard.GetComponent<ManageKeyBoard>().keyBoard;//����Ʈ ��������
        for(i = 0; i < ManageKeyBoard.numV; ++i)
        {
            if (keyBoard[i].isSuburb)// ���� ���� ������ �ÿ� +1
            {
                now += 1;
            }
            if(now == num)// ���� ���� ���������� �ߴ�
            {
                break;
            }
        }

        keyBoard[i].setEnemy(true);// �ߴ� ������ �� ����
    }

}