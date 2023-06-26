using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowCursor : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler; 
    public bool isHidden;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    void Update()
    {
        if (!isHidden)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            float halfWidth = (corners[1].x - corners[2].x) / -2;
            float halfHeight = (corners[1].y - corners[3].y) / 2;
            float xPosition = Input.mousePosition.x + halfWidth;
            if (xPosition + halfWidth > canvas.pixelRect.width)
                xPosition = Input.mousePosition.x - halfWidth;
            float yPosition = Input.mousePosition.y - halfHeight;
            if (yPosition - halfHeight < 0)
                yPosition = yPosition + 2 * halfHeight;
            /*
            xPosition = Mathf.Max(xPosition, halfWidth);
            yPosition = Mathf.Max(yPosition, halfHeight);
            xPosition = Mathf.Min(xPosition, canvas.pixelRect.width - halfWidth);
            yPosition = Mathf.Min(yPosition, canvas.pixelRect.height - halfHeight);
            */
            transform.position = new Vector3(xPosition, yPosition);
        }
        else
            transform.position = new Vector3(-10000, 0);
    }


}
