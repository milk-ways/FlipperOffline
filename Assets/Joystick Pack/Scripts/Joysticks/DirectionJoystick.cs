using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionJoystick : Joystick
{
    public float InputThreshold { get { return inputThreshold; } set { inputThreshold = Mathf.Abs(value); } }

    [SerializeField] private float inputThreshold = 0.5f;

    [SerializeField] private RectTransform moveRestriction = null;

    protected override void Start()
    {
        InputThreshold = inputThreshold;
        base.Start();
        moveRestriction.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        moveRestriction.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        moveRestriction.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    public bool OverRestriction()
    {
        return input.magnitude >= inputThreshold;
    }
}

