using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlockUnitScript : MonoBehaviour
{
    public int m_localX;
    public int m_localY;
    //public int m_x;
    //public int m_y;
    private void OnEnable()
    {
        m_localX = (int)(transform.localPosition.x / 75f);
        m_localY = (int)(-transform.localPosition.y / 75f);
        //m_x = m_localX + 5;
        //m_y = m_localY + 1;
        //transform.position = new Vector3(
        //   (m_x * 75 ) * GameManager.instance.scale,
        //   (-m_y * 75 ) * GameManager.instance.scale,
        //   0);
    }
    public void SwitchColor(string _name)
    {
        switch (_name)
        {
            case "I":
                GetComponent<Image>().sprite = Resources.Load("blue", typeof(Sprite)) as Sprite;
                break;
            case "J":
                GetComponent<Image>().sprite = Resources.Load("deepblue", typeof(Sprite)) as Sprite;
                break;
            case "L":
                GetComponent<Image>().sprite = Resources.Load("orange", typeof(Sprite)) as Sprite;
                break;
            case "O":
                GetComponent<Image>().sprite = Resources.Load("yellow", typeof(Sprite)) as Sprite;
                break;
            case "S":
                GetComponent<Image>().sprite = Resources.Load("green", typeof(Sprite)) as Sprite;
                break;
            case "T":
                GetComponent<Image>().sprite = Resources.Load("purple", typeof(Sprite)) as Sprite;
                break;
            case "Z":
                GetComponent<Image>().sprite = Resources.Load("red", typeof(Sprite)) as Sprite;
                break;
        }
    }
    //public void UpdateGobalXY(int x,int y)
    //{
    //    m_x = m_localX + x;
    //    m_y = m_localY + y;
    //}
    public void UpdateBlockPosition()
    {
        transform.localPosition = new Vector3(
            m_localX * 75 ,
            -m_localY * 75 ,
            0);   
    }
    public void FreshUnitBlockTween()
    {

    }
    //public void MoveLeft(int step)
    //{
    //    m_x -= step;
    //}
    //public void MoveUp(int step)
    //{
    //    m_y -= step;
    //}
    //public void MoveRight(int step)
    //{
    //    m_x += step;
    //}
    //public void MoveDown(int step)
    //{
    //    m_y += step;
    //}
}
