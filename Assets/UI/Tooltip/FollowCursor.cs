using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    public bool isHidden;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (!isHidden)
        {
            float xPosition = Input.mousePosition.x + rectTransform.sizeDelta.x / 2;
            float yPosition = Input.mousePosition.y - rectTransform.sizeDelta.y / 2;
            yPosition = Mathf.Max(yPosition, rectTransform.sizeDelta.y / 2);
            xPosition = Mathf.Max(xPosition, rectTransform.sizeDelta.x / 2);
            yPosition = Mathf.Min(yPosition, canvas.pixelRect.height - rectTransform.sizeDelta.y / 2);
            xPosition = Mathf.Min(xPosition, canvas.pixelRect.width - rectTransform.sizeDelta.x / 2);
            transform.position = new Vector3(xPosition, yPosition);
        }
        else
            transform.position = new Vector3(-10000, 0);
    }
}
