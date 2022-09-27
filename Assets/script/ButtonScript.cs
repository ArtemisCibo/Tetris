using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.instance.gameState == 0) return;
        switch (name) {
            case "moveLeft":
                GameManager.instance.MoveLeft();
                GameManager.instance.PlaySound("sound_move");
                break;
            case "moveRIght":
                GameManager.instance.MoveRight();
                GameManager.instance.PlaySound("sound_move");
                break;
            case "counterclockwise":
                GameManager.instance.CounterClockWiseRotate();
                GameManager.instance.PlaySound("sound_ratate");
                break;
            case "clockwise":
                GameManager.instance.ClockWiseRotate();
                GameManager.instance.PlaySound("sound_ratate");
                break;
            case "fallfaster":
                GameManager.instance.FallFaster();
                
                break;
            case "falled":
                GameManager.instance.Falled();
                GameManager.instance.PlaySound("sound_falled");
                break;
        }

    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        switch (name)
        {
            case "fallfaster":
                GameManager.instance.fallIntervalTime = 0.6f;
                break;

        }
    }
}
