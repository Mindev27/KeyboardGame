using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageEnemy : MonoBehaviour
{
    public GameObject manageKeyboard;
    public GameObject enemyPrefab;
    List<int> random;
    List<ManageKeyBoard.key> keyBoard;

    // Start is called before the first frame update
    void Start()
    {
        keyBoard = manageKeyboard.GetComponent<ManageKeyBoard>().keyBoard;//����Ʈ ��������\
        random = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void SummonEnemy()// �� ����
    {
        random.Clear();
        int num = 0;
        int i;
        
        for(i = 0; i < ManageKeyBoard.numV; ++i)
        {
            do
            {
                num = Random.Range(0, ManageKeyBoard.numV);// ���� ������ �� �ִ� ĭ�� ����
            } while (random.Contains(num));
            if (keyBoard[num].isSuburb && !keyBoard[num].isEnemy)// ���� ���� ������ �ÿ� +1
                break;
            else
                random.Add(num);
        }
        if (i < ManageKeyBoard.numV)
        {
            // Debug.Log("num: " + num);
            // Debug.Log("i: " + i);
            keyBoard[num].SetEnemy(true);// �ߴ� ������ �� ����
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