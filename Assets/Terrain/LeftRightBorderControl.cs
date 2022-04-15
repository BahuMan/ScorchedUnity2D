using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightBorderControl : MonoBehaviour
{
    [SerializeField] MoveBorderToCameraEdge _totalBorder;
    BoxCollider2D _thisBox;

    private void Start()
    {
        _thisBox = GetComponent<BoxCollider2D>();
    }

    public enum PositionEnum { Left, Right };
    public PositionEnum Position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MissileControl missile = collision.GetComponent<MissileControl>();
        if (missile == null) return;

        if (_totalBorder.GetBorderType() == MoveBorderToCameraEdge.BorderTypeEnum.WRAP)
        {
            Debug.Log("wrapping horizontally");
            Vector2 oldpos = missile.transform.position;
            //missile.GetComponent<Rigidbody2D>().MovePosition(new Vector2(-oldpos.x + Mathf.Sign(oldpos.x) * _thisBox.size.x, oldpos.y));
            missile.transform.position = new Vector2(-oldpos.x + Mathf.Sign(oldpos.x) * _thisBox.size.x / 2f, oldpos.y);
        }
    }
}
