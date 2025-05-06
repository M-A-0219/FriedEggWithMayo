using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrder : MonoBehaviour
{
    private SpriteRenderer mySR; 
    private Transform playerTrans; 
    [SerializeField] private float boundValue;
    private float boundY;  
    private string currentLayer; 

    void Awake()
    {

        mySR = GetComponent<SpriteRenderer>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTrans = playerObj.transform;
        }


        boundY = transform.position.y - boundValue;
        currentLayer = mySR.sortingLayerName;
    }

    void Update()
    {
        if (playerTrans == null) return; 

        if (playerTrans.position.y < boundY && currentLayer != "CharacterBack")
        {
            mySR.sortingLayerName = "CharacterBack"; // プレイヤーが前
            currentLayer = "CharacterBack";
        }
        else if (playerTrans.position.y >= boundY && currentLayer != "CharacterFront")
        {
            mySR.sortingLayerName = "CharacterFront"; // プレイヤーが後ろ
            currentLayer = "CharacterFront";
        }
    }

    public void UpdateSorting(float playerY)
    {

        boundY = transform.position.y - boundValue;

        if (playerY < boundY && currentLayer != "CharacterBack")
        {
            mySR.sortingLayerName = "CharacterBack";
            currentLayer = "CharacterBack";
        }
        else if (playerY >= boundY && currentLayer != "CharacterFront")
        {
            mySR.sortingLayerName = "CharacterFront";
            currentLayer = "CharacterFront";
        }
    }
}
