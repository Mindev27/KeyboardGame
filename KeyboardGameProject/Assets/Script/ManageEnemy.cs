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
        keyBoard = manageKeyboard.GetComponent<ManageKeyBoard>().keyBoard;//리스트 가져오기\
        random = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void SummonEnemy()// 적 생성
    {
        random.Clear();
        int num = 0;
        int i;
        
        for(i = 0; i < ManageKeyBoard.numV; ++i)
        {
            do
            {
                num = Random.Range(0, ManageKeyBoard.numV);// 적이 생성될 수 있는 칸의 범주
            } while (random.Contains(num));
            if (keyBoard[num].isSuburb && !keyBoard[num].isEnemy)// 적이 생성 가능할 시에 +1
                break;
            else
                random.Add(num);
        }
        if (i < ManageKeyBoard.numV)
        {
            // Debug.Log("num: " + num);
            // Debug.Log("i: " + i);
            keyBoard[num].SetEnemy(true);// 중단 값에서 적 생성
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