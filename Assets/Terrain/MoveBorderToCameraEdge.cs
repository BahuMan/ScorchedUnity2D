using System;
using UnityEngine;

public class MoveBorderToCameraEdge : MonoBehaviour
{

    [SerializeField] TriggerResizeEvent mainUI;
    [SerializeField] BoxCollider2D LeftBorder;
    [SerializeField] BoxCollider2D RightBorder;
    [SerializeField] BoxCollider2D TopBorder;
    [SerializeField] BoxCollider2D BottomBorder;

    [SerializeField] PhysicsMaterial2D BounceMaterial;
    [SerializeField] PhysicsMaterial2D SolidMaterial;

    public enum BorderTypeEnum { SOLID, BOUNCE, WRAP }
    [SerializeField]
    BorderTypeEnum borderType;

    private void OnValidate()
    {
        switch (borderType)
        {
            case BorderTypeEnum.SOLID: MakeBordersSolid(); break;
            case BorderTypeEnum.WRAP: MakeBordersWrap(); break;
            case BorderTypeEnum.BOUNCE: MakeBordersBounce(); break;
        }
    }

    private void MakeBordersBounce()
    {
        LeftBorder.isTrigger = false;
        RightBorder.isTrigger = false;
        TopBorder.isTrigger = false;
        BottomBorder.isTrigger = false;

        LeftBorder.sharedMaterial = BounceMaterial;
        RightBorder.sharedMaterial = BounceMaterial;
        TopBorder.sharedMaterial = BounceMaterial;
        BottomBorder.sharedMaterial = BounceMaterial;

        LeftBorder.GetComponent<SpriteRenderer>().enabled = true;
        RightBorder.GetComponent<SpriteRenderer>().enabled = true;
        TopBorder.GetComponent<SpriteRenderer>().enabled = true;
        BottomBorder.GetComponent<SpriteRenderer>().enabled = true;

        LeftBorder.GetComponent<SpriteRenderer>().color = Color.cyan;
        RightBorder.GetComponent<SpriteRenderer>().color = Color.cyan;
        TopBorder.GetComponent<SpriteRenderer>().color = Color.cyan;
        BottomBorder.GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    private void MakeBordersWrap()
    {
        LeftBorder.isTrigger = true;
        RightBorder.isTrigger = true;
        TopBorder.isTrigger = true;
        BottomBorder.isTrigger = true;

        LeftBorder.GetComponent<SpriteRenderer>().enabled = true;
        RightBorder.GetComponent<SpriteRenderer>().enabled = true;
        TopBorder.GetComponent<SpriteRenderer>().enabled = true;
        BottomBorder.GetComponent<SpriteRenderer>().enabled = true;

        LeftBorder.GetComponent<SpriteRenderer>().color = Color.yellow;
        RightBorder.GetComponent<SpriteRenderer>().color = Color.yellow;
        TopBorder.GetComponent<SpriteRenderer>().color = Color.yellow;
        BottomBorder.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    private void MakeBordersSolid()
    {
        LeftBorder.isTrigger = false;
        RightBorder.isTrigger = false;
        TopBorder.isTrigger = false;
        BottomBorder.isTrigger = false;

        LeftBorder.sharedMaterial = SolidMaterial;
        RightBorder.sharedMaterial = SolidMaterial;
        TopBorder.sharedMaterial = SolidMaterial;
        BottomBorder.sharedMaterial = SolidMaterial;

        LeftBorder.GetComponent<SpriteRenderer>().enabled = true;
        RightBorder.GetComponent<SpriteRenderer>().enabled = true;
        TopBorder.GetComponent<SpriteRenderer>().enabled = true;
        BottomBorder.GetComponent<SpriteRenderer>().enabled = true;

        LeftBorder.GetComponent<SpriteRenderer>().color = Color.white;
        RightBorder.GetComponent<SpriteRenderer>().color = Color.white;
        TopBorder.GetComponent<SpriteRenderer>().color = Color.white;
        BottomBorder.GetComponent<SpriteRenderer>().color = Color.white;

    }


    private void OnEnable()
    {
        mainUI.OnDimensionsChanged.AddListener(mainUI_OnDimensionsChanged);
    }

    private void OnDisable()
    {
        mainUI.OnDimensionsChanged.RemoveListener(mainUI_OnDimensionsChanged);
    }

    private void mainUI_OnDimensionsChanged(Rect totalview)
    {
        Debug.Log("moving borders...");
        LeftBorder.transform.position = new Vector3(totalview.x - totalview.width - .4f, totalview.y);
        LeftBorder.transform.localScale = new Vector3(1f, 20f * totalview.height, 1f);

        RightBorder.transform.position = new Vector3(totalview.x + totalview.width + .4f, totalview.y);
        RightBorder.transform.localScale = new Vector3(1f, 20f * totalview.height, 1f);

        TopBorder.transform.position = new Vector3(totalview.x, totalview.y + totalview.height + .4f);
        TopBorder.transform.localScale = new Vector3(2f * totalview.width, 1f, 1f);

        BottomBorder.transform.position = new Vector3(totalview.x, totalview.y - totalview.height - .4f);
        BottomBorder.transform.localScale = new Vector3(2f * totalview.width, 1f, 1f);

    }

    public BorderTypeEnum GetBorderType()
    {
        return borderType;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (this.borderType == BorderTypeEnum.BOUNCE)
        {
            MissileControl missile = collision.GetComponent<MissileControl>();
            if (missile != null)
            {
                Debug.Log("missile hit border while type = " + borderType);
                if (collision == LeftBorder || collision == RightBorder)
                {
                    Debug.Log("mirroring horizontal component");
                    Vector3 velocity = missile.GetComponent<Rigidbody2D>().velocity;
                    velocity.x = -velocity.x;
                    missile.GetComponent<Rigidbody2D>().velocity = velocity;
                }
                else if (collision == TopBorder || collision == BottomBorder)
                {
                    Debug.Log("mirroring vertical component");
                    Vector3 velocity = missile.GetComponent<Rigidbody2D>().velocity;
                    velocity.y = -velocity.y;
                    missile.GetComponent<Rigidbody2D>().velocity = velocity;
                }
            }
        }
    }

}
