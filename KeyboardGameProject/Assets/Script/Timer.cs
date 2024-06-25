using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject manageEnemy;
    public GameObject manageKeyBoard;
    public static float cycle = 1;//�� ����Ŭ�� ����
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= cycle)
        {
            manageEnemy.GetComponent<ManageEnemy>().CalcDelay();
            time = 0;
        }
    }
}
