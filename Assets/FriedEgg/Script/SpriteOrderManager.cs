using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderManager : MonoBehaviour
{
    public Transform playerTrans;
    public float processingRange = 20f;

    public List<SpriteOrder> spriteOrders = new List<SpriteOrder>();

    void LateUpdate()
    {
        foreach (var spriteOrder in spriteOrders)
        {
            if (Vector2.Distance(spriteOrder.transform.position, playerTrans.position) <= processingRange)
            {
                spriteOrder.UpdateSorting(playerTrans.position.y);
            }
        }
    }

    public void RegisterSprite(SpriteOrder spriteOrder)
    {
        if (!spriteOrders.Contains(spriteOrder))
        {
            spriteOrders.Add(spriteOrder);
        }
    }

    public void UnregisterSprite(SpriteOrder spriteOrder)
    {
        if (spriteOrders.Contains(spriteOrder))
        {
            spriteOrders.Remove(spriteOrder);
        }
    }
}
