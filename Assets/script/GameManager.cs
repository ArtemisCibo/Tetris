﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private const int gameViewRow = 16;
    private const int gameViewColumn = 10;
    public float fallIntervalTime = .6f;
    private const float blockSideLen = 75;
    private float totalTime;
    public bool isFasterFall = false;
    public Vector3 leftUpGridPos;
    public GameObject curCom;
    private ComScript curComScript;
    private GameObject view;
    private GameObject shadowObj;
    /*0-ready 1-gaming */
    public int gameState;
    private ArrayList buildBlockUnitScrArr = new ArrayList();
    private ArrayList comArr = new ArrayList();
    public float scale;
    private AudioSource sound_complete;
    private AudioSource sound_move;
    private AudioSource sound_falled;
    private AudioSource sound_ratate;
    /*0-blank 1-curComUnit 2-build*/
    public int[,] gameViewData = new int[gameViewRow, gameViewColumn] {
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 }
    };

    private void Awake()
    {
        instance = this;
        if ((float)UnityEngine.Screen.width / UnityEngine.Screen.height >= 750f / 1334f)
        {
            scale = UnityEngine.Screen.height / 1334f;
        }
        else
        {
            scale = UnityEngine.Screen.width / 750f;
        }
        gameState = 1;
        totalTime = 0f;
    }

    private void OnGUI()
    {

    }
    private void OnEnable()
    {


        sound_complete = GameObject.Find("sound_complete").GetComponent<AudioSource>();
        sound_move = GameObject.Find("sound_move").GetComponent<AudioSource>();
        sound_falled = GameObject.Find("sound_falled").GetComponent<AudioSource>();
        sound_ratate = GameObject.Find("sound_ratate").GetComponent<AudioSource>();

        view = GameObject.Find("view");
        view.transform.localScale = new Vector3(scale, scale, 1);
        leftUpGridPos = new Vector3(-337.5f, 829.5f, 0f);

        Init();
    }
    public void PlaySound(string name)
    {
        switch (name)
        {
            case "sound_complete":
                sound_complete.Play();
                break;
            case "sound_move":
                sound_move.Play();
                break;
            case "sound_falled":
                sound_falled.Play();
                break;
            case "sound_ratate":
                sound_ratate.Play();
                break;
        }
    }
    private void Init()
    {
        CreateRandomCom();
    }
    private void CreateRandomCom()
    {
        //(int)Mathf.Round(Random.Range(0, 6))
        GameObject go = GameObject.Find("Coms").transform.GetChild((int)Mathf.Round(Random.Range(0, 6))).gameObject;
        curCom = Instantiate(go);
        curCom.name = go.name;
       
        curComScript = curCom.GetComponent<ComScript>();
        curComScript.SwitchColor(curCom.name);
        InitComData();
        comArr.Add(curCom);

        CreateShadow();
    }
    private void CreateShadow()
    {
        if (shadowObj) Destroy(shadowObj);
        shadowObj = Instantiate(curCom);
        shadowObj.GetComponent<ComScript>().enabled = false;
        shadowObj.transform.SetParent(GameObject.Find("view").transform);
        shadowObj.transform.localScale = new Vector3(1, 1, 1);
        shadowObj.transform.localPosition = curCom.transform.localPosition;
        for (int i = 0; i < 4; i++)
        {
            shadowObj.transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, .3f);
        }
    }
    private void UpdateGameViewData()
    {
        ClearGameViewComData();
        BlockUnitScript b;
        for (int i = 0; i < 4; i++)
        {
            b = curComScript.transform.GetChild(i).GetComponent<BlockUnitScript>();
            gameViewData[curComScript.m_y + b.m_localY, curComScript.m_x + b.m_localX] = 1;
        }
    }


    private void InitComData()
    {
        curCom.transform.SetParent(GameObject.Find("view").transform);
        curCom.transform.localScale = new Vector3(1, 1, 1);
        //curCom.transform.localPosition = leftUpGridPos;



    }
    private void FixedUpdate()
    {
        if (gameState == 0) return;
        totalTime += Time.deltaTime;
        if (totalTime >= fallIntervalTime)
        {
            if (CanFallNextGrid())
            {
                curComScript.Fall(1);
            }
            else
            {
                FreezeCurCom();
                CheckBulid();
                CreateRandomCom();      
            }
            totalTime = 0;
        }

        UpdateGameViewData();
        UpdateCurComPos();
        CastShadow();
        //for (int i = 0; i < gameViewRow; i++)
        //{
        //    Debug.Log(
        //        (GameManager.instance.gameViewData[i, 0] == 0 ? "□" : GameManager.instance.gameViewData[i, 0] == 1 ? "◯" : "■") + " " +
        //        (GameManager.instance.gameViewData[i, 1] == 0 ? "□" : GameManager.instance.gameViewData[i, 1] == 1 ? "◯" : "■") + " " +
        //        (GameManager.instance.gameViewData[i, 2] == 0 ? "□" : GameManager.instance.gameViewData[i, 2] == 1 ? "◯" : "■") + " " +
        //        (GameManager.instance.gameViewData[i, 3] == 0 ? "□" : GameManager.instance.gameViewData[i, 3] == 1 ? "◯" : "■") + " " +
        //        (GameManager.instance.gameViewData[i, 4] == 0 ? "□" : GameManager.instance.gameViewData[i, 4] == 1 ? "◯" : "■") + " " +
        //        (GameManager.instance.gameViewData[i, 5] == 0 ? "□" : GameManager.instance.gameViewData[i, 5] == 1 ? "◯" : "■") + " " +
        //        (GameManager.instance.gameViewData[i, 6] == 0 ? "□" : GameManager.instance.gameViewData[i, 6] == 1 ? "◯" : "■") + " " +
        //        (GameManager.instance.gameViewData[i, 7] == 0 ? "□" : GameManager.instance.gameViewData[i, 7] == 1 ? "◯" : "■") + " " +
        //        (GameManager.instance.gameViewData[i, 8] == 0 ? "□" : GameManager.instance.gameViewData[i, 8] == 1 ? "◯" : "■") + " " +
        //        (GameManager.instance.gameViewData[i, 9] == 0 ? "□" : GameManager.instance.gameViewData[i, 9] == 1 ? "◯" : "■") + "   " + i
        //        );
        //}
        //Debug.Log("-------------------------");
        //GameObject.Find("t1").GetComponent<Text>().text = "x:" + curComScript.m_x + " u:" + curComScript.m_y;
        //GameObject.Find("t2").GetComponent<Text>().text = "curComx:" + curComScript.m_x + " curComy:" + curComScript.m_y;
        //GameObject.Find("t3").GetComponent<Text>().text = "curComPosx:" + curCom.transform.position.x + " curComPosy:" + curCom.transform.position.y;
        //GameObject.Find("t4").GetComponent<Text>().text = "curComLocalposx:" + curCom.transform.localPosition.x + " curComLocalposy:" + curCom.transform.localPosition.y;
    }
    private void CastShadow()
    {     
        
        int count = CanFallGridCount();
        GameObject curComChild;
        GameObject shadowChild;
        shadowObj.transform.localPosition = new Vector3(curCom.transform.localPosition.x, curCom.transform.localPosition.y+ -count*blockSideLen,0);
        for (int i = 0;i < 4; i++)
        {
            curComChild = curCom.transform.GetChild(i).gameObject;
            shadowChild = shadowObj.transform.GetChild(i).gameObject;
            shadowChild.transform.localPosition = curComChild.transform.localPosition;
        }
    }
    private void UpdateCurComPos()
    {
        curCom.transform.localPosition = new Vector3(
        leftUpGridPos.x + curComScript.m_x * blockSideLen,
        leftUpGridPos.y + -curComScript.m_y * blockSideLen,
        0
        );
    }
    private void ClearGameViewComData()
    {
        int k = 0;
        foreach (int i in gameViewData)
        {
            if (i == 1)
            {
                gameViewData[k / 10, k % 10] = 0;
            }
            k++;
        }
    }
    private void FreezeCurCom()
    {
        for (int i = 0; i < 4; i++)
        {
            //b = curComScript.gameObject.transform.GetChild(i).GetComponent<blockUnit>();
            gameViewData[curComScript.m_y + curComScript.b[i].m_localY, curComScript.m_x + curComScript.b[i].m_localX] = 2;
            buildBlockUnitScrArr.Add(curComScript.b[i]);
        }
        
    }
    private void CheckBulid()
    {
        BlockUnitScript b;
        for (int i = 0; i < 4; i++)
        {
            b = curComScript.transform.GetChild(i).GetComponent<BlockUnitScript>();
            if (curComScript.m_y + b.m_localY < 2)
            {
                GameOver();
                return;
            }
        }

        bool allBuild = true;
        bool playSound = false;
        ArrayList removeArrList = new ArrayList();
        for (int i = 0;i < gameViewRow; i++)
        {
            allBuild = true;
            for (int j = 0; j < gameViewColumn; j++)
            {
                if (gameViewData[i, j] != 2) { 
                    allBuild = false;
                    break;
                } 
            }
            if (allBuild)
            {
                playSound = true;
                removeArrList.Clear();
                foreach(BlockUnitScript element in buildBlockUnitScrArr)
                {
                    ComScript parentScript = element.transform.parent.gameObject.GetComponent<ComScript>();
                    if (element.m_localY + parentScript.m_y == i)
                    {
                        element.gameObject.SetActive(false);
                        removeArrList.Add(element);
                    }
                    for (int j = 0;j < gameViewColumn; j++)
                    {
                        gameViewData[i, j] = 0;
                    }
                   
                    //for (int j = 0; j < gameViewColumn; j++)
                    //{
                    //    ComScript parentScript = element.transform.parent.gameObject.GetComponent<ComScript>();
                    //    if (element.m_localX + parentScript.m_x == j && element.m_localY + parentScript.m_y == i)
                    //    {
                    //        element.gameObject.SetActive(false);
                    //        removeArrList.Add(element);                           
                    //    }
                    //    gameViewData[i,j] = 0;
                    //}
                }
                foreach (BlockUnitScript element in removeArrList)
                {
                    buildBlockUnitScrArr.Remove(element);
                }
                //data process
                for (int n = i - 1; n >=0; n--)
                {
                    for (int j = 0; j < gameViewColumn; j++)
                    {
                        if(gameViewData[n, j] == 2)
                        {
                            gameViewData[n, j] = 0;
                            gameViewData[n+1, j] = 2;
                            BlockUnitScript o  = GetBuildBlockUnit(j, n);
                            if(o != null)
                            {
                                o.m_localY += 1;
                                o.UpdateBlockPosition();
                            }
                            
                        }
                    }
                }
            }
        }
        bool allHide = true;
        if (playSound)
        {
            PlaySound("sound_complete");
        }
        foreach (GameObject curCom in comArr)
        {
            for (int i = 0; i < 4; i++)
            {
                if (curCom.transform.GetChild(i).gameObject.activeSelf == true) allHide = false;
            }
        }
        if (allHide)
        {
            Destroy(curCom);
        }

    }
    private BlockUnitScript GetBuildBlockUnit(int x, int y)
    {
        foreach (BlockUnitScript element in buildBlockUnitScrArr)
        {
            if(element.m_localX + element.transform.parent.gameObject.GetComponent<ComScript>().m_x == x && element.m_localY + element.transform.parent.gameObject.GetComponent<ComScript>().m_y == y)
            {
                return element;
            }
        }
        return null;
    }
    private void GameOver()
    {
        gameState = 0;
        Debug.Log("GameOver");
    }
    private bool CanFallNextGrid()
    {
        BlockUnitScript b;
        for (int i = 0; i < 4; i++)
        {
            b = curComScript.transform.GetChild(i).GetComponent<BlockUnitScript>();
            if (curComScript.m_y + b.m_localY >= gameViewRow-1) return false;   
            if (gameViewData[curComScript.m_y + b.m_localY + 1, curComScript.m_x + b.m_localX] == 2) return false;       
        }
        
        return true;
    }
    private int CanFallGridCount()
    {
        BlockUnitScript b;
        int count = 0;
        while (true)
        {

            for (int i = 0; i < 4; i++)
            {
                b = curComScript.transform.GetChild(i).GetComponent<BlockUnitScript>();
   
                if (gameViewData[curComScript.m_y + b.m_localY + count, curComScript.m_x + b.m_localX] == 2)
                {
                    return count-1;
                }

            }
            for (int i = 0; i < 4; i++)
            {
                b = curComScript.transform.GetChild(i).GetComponent<BlockUnitScript>();
                if (curComScript.m_y + b.m_localY + count >= gameViewRow - 1)
                {
                    return count;
                }
            }


            count++;
        } 
    }
    public void MoveLeft()
    {      
        curComScript.ChangeComTranslate("MoveLeft");
    }
    public void MoveRight()
    {
        curComScript.ChangeComTranslate("MoveRight");      
    }
    public void CounterClockWiseRotate()
    {
        curComScript.ChangeComTranslate("CounterClockWiseRotate");
    }
    public void ClockWiseRotate()
    {
        curComScript.ChangeComTranslate("ClockWiseRotate");
    }
    public void FallFaster()
    {
        if (!CanFallNextGrid()) return;
        curComScript.Fall(1);
        UpdateCurComPos();
        //FreezeCurCom();
        totalTime = 0;
    }
    public void Falled()
    {
        curComScript.Fall(CanFallGridCount());
        UpdateCurComPos();
        FreezeCurCom();
        CheckBulid();
        CreateRandomCom();
        totalTime = 0;
        //CheckBulid();

    }

}
