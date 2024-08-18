using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject manageEnemy;
    public GameObject manageKeyBoard;
    public GameObject attack;
    public GameObject player;
    public static float cycle = 2;//�� ����Ŭ�� ����
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        GameStart1();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GameStart1()
    {
        manageEnemy.GetComponent<ManageEnemy>().CalcDelay();//�� ���� ĭ ����
        player.GetComponent<Player>().turnMove = true; //�÷��̾� �̵�
    }
    public void GameStart2()
    {
        attack.GetComponent<Attack>().SummonAttack();//���� ����
        manageEnemy.GetComponent<ManageEnemy>().moveEnemy();//��ü �� �̵�(����Ŭ ���� �Ǵ�)
        Invoke("GameStart1",0.2f);
    }

}
