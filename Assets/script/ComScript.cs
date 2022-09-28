using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComScript : MonoBehaviour
{
    public int m_x;
    public int m_y;
    /*0-active 1-lock*/
    public int state = 0;
    public BlockUnitScript[] b = { null, null, null, null };
    private void OnEnable()
    {
        m_x = 5;
        m_y = 2;
        for (int i = 0; i < 4; i++)
        {
            b[i] = transform.GetChild(i).GetComponent<BlockUnitScript>();
        }
    }
    public void ChangeComTranslate(string operation)
    {
        switch (operation)
        {
            case "CounterClockWiseRotate":
                CounterClockWiseRotate();
                break;
            case "ClockWiseRotate":
                ClockWiseRotate();
                break;
            case "MoveLeft":
                MoveLeft();
                break;
            case "MoveRight":
                MoveRight();
                break;
        }

            
    }
    public void SwitchColor(string _name)
    {
        for(int i = 0;i < 4; i++)
        {
            b[i].SwitchColor(_name);
        }
    }
    public void CounterClockWiseRotate()
    {
        if (state == 1) return;
        if (name == "O") return;

        int[] nextBX = { 0, 0, 0, 0 };
        int[] nextBY = { 0, 0, 0, 0 };
        for (int j = 0; j < 4; j++)
        {
            nextBX[j] = 0;
            nextBY[j] = 0;

        }
        for (int i = 0; i < 4; i++)
        {

            
            if (b[i].m_localY > 0)
            {
                if (b[i].m_localX > 0)
                {
                    nextBX[i] = b[i].m_localX;
                    nextBY[i] = -b[i].m_localY;
                    //b.m_localX = -b.m_localX;            
                }
                if (b[i].m_localX == 0)
                {

                    nextBX[i] = b[i].m_localY;
                    nextBY[i] = 0;
                    //b.m_localX = -b.m_localY;
                    //b.m_localY = 0;                                     
                }
                if (b[i].m_localX < 0)
                {

                    nextBY[i] = b[i].m_localY;
                    nextBX[i] = -b[i].m_localX;
                    //b.m_localY = -b.m_localY;                  
                }
            }
            if (b[i].m_localY == 0)
            {
                if (b[i].m_localX > 0)
                {
                    nextBY[i] = -b[i].m_localX;
                    nextBX[i] = 0;
                    //b.m_localY = b.m_localX;
                    //b.m_localX = 0;                
                }
                if (b[i].m_localX == 0)
                {
                    
                }
                if (b[i].m_localX < 0)
                {
                    nextBY[i] = -b[i].m_localX;
                    nextBX[i] = 0;
                    //b.m_localY = b.m_localX;
                    //b.m_localX = 0;                  
                }
            }
            if (b[i].m_localY < 0)
            {
                if (b[i].m_localX > 0)
                {
                    nextBY[i] = b[i].m_localY;
                    nextBX[i] = -b[i].m_localX;
                    //b.m_localY = -b.m_localY;
                }
                if (b[i].m_localX == 0)
                {
                    nextBX[i] = b[i].m_localY;
                    nextBY[i] = 0;
                    //b.m_localX = -b.m_localY;
                    //b.m_localY = 0;    
                }
                if (b[i].m_localX < 0)
                {
                    nextBX[i] = b[i].m_localX;
                    nextBY[i] = -b[i].m_localY;
                    //b.m_localX = -b.m_localX;         
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (m_x + nextBX[i] < 0) return;
            if (m_x + nextBX[i] >= 10) return;
            if (m_y + nextBY[i] < 0) return;
            if (m_y + nextBY[i] >= 14) return;
            if (GameManager.instance.gameViewData[m_y + nextBY[i], m_x + nextBX[i]] == 2)
            {
                return;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("m_localX" + b[i].m_localX + "--> " + nextBX[i]);
            //Debug.Log("m_localY" + b[i].m_localY + "--> " + nextBY[i]);
            b[i].m_localX = nextBX[i];
            b[i].m_localY = nextBY[i];
            //b[i].UpdateGobalXY(m_x, m_y);
            b[i].UpdateBlockPosition();
        }
    }

    public void ClockWiseRotate()
    {
        if (state == 1) return;
        if (name == "O") return;
       
        int[] nextBX = { 0, 0, 0, 0 };
        int[] nextBY = { 0, 0, 0, 0 };
        for (int j = 0; j < 4; j++)
        {
            nextBX[j] = 0;
            nextBY[j] = 0;
           
        }
        for (int i = 0; i < 4; i++)
        {

           
            if (b[i].m_localY > 0)
            {
                if (b[i].m_localX > 0)
                {           
                    nextBX[i] = -b[i].m_localX;
                    nextBY[i] = b[i].m_localY;
                    //b.m_localX = -b.m_localX;            
                }
                if (b[i].m_localX == 0)
                {                  
  
                    nextBX[i] = -b[i].m_localY;
                    nextBY[i] = 0;
                    //b.m_localX = -b.m_localY;
                    //b.m_localY = 0;                                     
                }
                if (b[i].m_localX < 0)
                {
                                     
                    nextBY[i] = -b[i].m_localY;
                    nextBX[i] = b[i].m_localX;
                    //b.m_localY = -b.m_localY;                  
                }
            }
            if (b[i].m_localY == 0)
            {
                if (b[i].m_localX > 0)
                {
                    nextBY[i] = b[i].m_localX;
                    nextBX[i] = 0;
                    //b.m_localY = b.m_localX;
                    //b.m_localX = 0;                
                }
                if (b[i].m_localX == 0)
                {
                              
                }
                if (b[i].m_localX < 0)
                {
                    nextBY[i] = b[i].m_localX;
                    nextBX[i] = 0;
                    //b.m_localY = b.m_localX;
                    //b.m_localX = 0;                  
                }
            }
            if (b[i].m_localY < 0)
            {
                if (b[i].m_localX > 0)
                {      
                    nextBY[i] = -b[i].m_localY;
                    nextBX[i] = b[i].m_localX;
                    //b.m_localY = -b.m_localY;
                }
                if (b[i].m_localX == 0)
                {
                    nextBX[i] = -b[i].m_localY;
                    nextBY[i] = 0;
                    //b.m_localX = -b.m_localY;
                    //b.m_localY = 0;    
                }
                if (b[i].m_localX < 0)
                {
                    nextBX[i] = -b[i].m_localX;
                    nextBY[i] = b[i].m_localY;
                    //b.m_localX = -b.m_localX;         
                }
            }   
        }
        for (int i = 0; i < 4; i++)
        {
            if (m_x + nextBX[i] < 0) return;
            if (m_x + nextBX[i] >= 10) return;
            if (m_y + nextBY[i] < 0) return;
            if (m_y + nextBY[i] >= 14) return;
            if (GameManager.instance.gameViewData[m_y + nextBY[i], m_x + nextBX[i]] == 2)
            {
                return;
            }       
        }
        for (int i = 0; i < 4; i++)
        {
            b[i].m_localX = nextBX[i];
            b[i].m_localY = nextBY[i];
            //b[i].UpdateGobalXY(m_x, m_y);
            b[i].UpdateBlockPosition();
        }


    }

    public void Fall(int grid)
    {
        m_y += grid;
    }
    public void MoveLeft()
    {

        for (int i = 0; i < 4; i++)
        {
            if (m_x + b[i].m_localX <= 0) return;
            if (GameManager.instance.gameViewData[m_y + b[i].m_localY, m_x + b[i].m_localX - 1] == 2) return;
        }
        m_x -= 1;
    }
    public void MoveRight()
    {

        for (int i = 0; i < 4; i++)
        {
            b[i] = transform.GetChild(i).GetComponent<BlockUnitScript>();
            if (m_x + b[i].m_localX >= 9) return;
            if (GameManager.instance.gameViewData[m_y + b[i].m_localY, m_x + b[i].m_localX + 1] == 2) return;
        }
        m_x += 1;
    }
    public void FallFaster()
    {

    }
    public void Falled()
    {

    }
    private void OnMouseDown()
    {
        Debug.Log("as");
    }

}
