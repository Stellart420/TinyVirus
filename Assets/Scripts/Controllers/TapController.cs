using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{
    public static TapController instance;
    public Action<Vector2, InteractionType, Collider2D> Interaction;


    Vector2 touchStartPos;
    public Vector2 TouchStartPos => touchStartPos;

    Vector2 touchStartPos2;
    public Vector2 TouchStartPos2 => touchStartPos2;
    Collider2D touchedCollider;

    public Collider2D TouchedCollider => touchedCollider;

    Collider2D touchedCollider2;

    public Collider2D TouchedCollider2 => touchedCollider2;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == 0)
                    CheckTouch(Input.touches[0]);

                if (touch.fingerId == 1)
                    CheckTouch(Input.touches[1], true);
            }
        }
        else if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            CheckMouse();
        }
        else
        {

        }
    }

    void CheckTouch(Touch touch, bool second = false)
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

        if (touch.phase == TouchPhase.Began)
        {
            TapFinger(touchPosition, second);
        }

        if (touch.phase == TouchPhase.Moved)
        {
            HoldFinger(touchPosition, second);
        }

        if (touch.phase == TouchPhase.Ended)
        {
            UpFinger(second);
        }
    }

    void CheckMouse(bool second = false)
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            TapFinger(touchPosition);
        }

        if (Input.GetMouseButton(0))
        {
            HoldFinger(touchPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            UpFinger();
        }
    }

    void TapFinger(Vector2 touchPosition, bool second = false)
    {
        if (!second)
        {
            touchedCollider = Physics2D.OverlapPoint(touchPosition);
            touchStartPos = touchPosition;
            if (touchedCollider)
                print(touchedCollider.name);

            Interaction?.Invoke(touchPosition, InteractionType.Tap, touchedCollider);
        }
        else
        {
            touchedCollider2 = Physics2D.OverlapPoint(touchPosition);
            touchStartPos2 = touchPosition;
            if (touchedCollider2)
                print(touchedCollider2.name);

            Interaction?.Invoke(touchPosition, InteractionType.Tap, touchedCollider2);
        }
    }

    void HoldFinger(Vector2 touchPosition, bool second = false)
    {
        if (!second)
        {
            Interaction?.Invoke(touchPosition, InteractionType.Hold, touchedCollider);
            CheckDrag(touchPosition);
        }
        else
        {
            Interaction?.Invoke(touchPosition, InteractionType.Hold, touchedCollider2);
            CheckDrag(touchPosition, second);
        }
    }

    void CheckDrag(Vector2 touchPosition, bool second = false)
    {
        if (!second)
        {
            var touch_pos = TapController.instance.TouchStartPos;
            var dist = Mathf.Sqrt(Mathf.Pow(Mathf.Clamp(touchPosition.x, GameController.Instance.minX, GameController.Instance.maxX) - touchStartPos.x, 2) + Mathf.Pow(Mathf.Clamp(touchPosition.y, GameController.Instance.minY, GameController.Instance.maxY) - touchStartPos.y, 2));
            if (dist > 5)
            {
                Interaction?.Invoke(touchPosition, InteractionType.Drag, touchedCollider);
            }
        }
        else
        {
            var touch_pos = TapController.instance.TouchStartPos2;
            var dist = Mathf.Sqrt(Mathf.Pow(Mathf.Clamp(touchPosition.x, GameController.Instance.minX, GameController.Instance.maxX) - touchStartPos2.x, 2) + Mathf.Pow(Mathf.Clamp(touchPosition.y, GameController.Instance.minY, GameController.Instance.maxY) - touchStartPos2.y, 2));
            if (dist > 5)
            {
                Interaction?.Invoke(touchPosition, InteractionType.Drag, touchedCollider2);
            }
        }
    }

    void UpFinger(bool second = false)
    {
        if (!second)
        {
            touchedCollider = null;
            Interaction?.Invoke(Vector3.zero, InteractionType.Up, touchedCollider);
        }
        else
        {
            touchedCollider2 = null;
            Interaction?.Invoke(Vector3.zero, InteractionType.Up, touchedCollider2);
        }
    }
}

public enum InteractionType
{
    Tap,
    Hold,
    Up,
    Drag,
    DoubleTap,
}
