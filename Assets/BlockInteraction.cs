using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    private Ray2D groundRay;

    public event Action<blockType> BlockAction;

    public blockType CheckGroundType(Transform pos, LayerMask groundLayer)
    {
        var _pos = (Vector2) pos.position;
        groundRay = new Ray2D(_pos,Vector2.down);
        var hitInfo = Physics2D.Raycast(groundRay.origin, groundRay.direction,1f,groundLayer);
        if (hitInfo.collider !=null)
        {
            var block = hitInfo.collider.gameObject.GetComponent<blockType>();
            BlockAction?.Invoke(block);
            //Debug.Log("I'm standing on a "+block.ToString());
            return block;
/*
            switch (block.ToString())
            {
                case "Damager":
                    Debug.Log("Ouch");
                    break;
                default:
                    Debug.Log("I'm standing on a " + block.ToString());
                    break;

            }
*/

        }
        else
        {
            return null;
        }
    }

}
