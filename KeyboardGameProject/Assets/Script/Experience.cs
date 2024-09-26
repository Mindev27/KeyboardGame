using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    public int EXPLevel;
    public GameObject levelText;
    public GameObject EXPBar;
    public GameObject levelUp;
    public GameObject timer;

    private bool selectOption = false;
    private int EXP;
    private List<int> EXPList;
    private const int maxEXPLevel = 10;

    // Start is called before the first frame update
    void Start()
    {
        EXPList = new List<int>(10);
        EXP = 0;
        EXPLevel = 0;
        if (EXPLevel < maxEXPLevel - 1)
        {
            InitEXPList();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selectOption)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SelectLevelUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectLevelUp();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectLevelUp();
            }
        }
    }

    public void AddEXP(int addedEXP)//적 처치시 작동
    {
        EXP += addedEXP;
        CalcEXPLevel();
    }

    public void InitLevelUpOption()//레벨 업 선택지 초기화
    {

    }

    public void SelectLevelUp()
    {
        selectOption = false;
        this.GetComponent<Player>().turnMove = true;
        levelUp.SetActive(false);
        timer.GetComponent<Timer>().Resume();
        FindAnyObjectByType<ManageEnemy>().ChangeDelay(9 - EXPLevel > 0 ? 9 - EXPLevel : 1);
    }

    private void CalcEXPLevel()//경험치에 따른 경험치 레벨 처리
    {
        if(EXPList[EXPLevel] <= EXP)
        {
            EXP -= EXPList[EXPLevel];
            EXPLevel += 1;
            levelText.GetComponent<TextMeshProUGUI>().text = "Level " + (EXPLevel+1).ToString();
            levelUp.SetActive(true);
            selectOption = true;
            this.GetComponent<Player>().turnMove = false;
            timer.GetComponent<Timer>().Pause();
        }
        EXPBar.GetComponent<Slider>().value = (float)EXP / (float)EXPList[EXPLevel];
    }

    private void InitEXPList()//처음 경험치 테이블 초기화 / 추후 JSON사용
    {
        for(int i = 1; i <= 10; ++i)
        {
            EXPList.Add(i * 2);
        }
    }
}
