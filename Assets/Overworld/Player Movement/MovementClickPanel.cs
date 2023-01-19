using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MovementClickPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PlayerMovement playerMovement;
    private PlayerInputActions playerInputActions;
    private InputAction point;
    private new Camera camera;

    private void Awake()
    {
        camera = Camera.main;
        playerInputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        point = playerInputActions.UI.Point;
        point.Enable();
    }
    private void OnDisable()
    {
        point.Disable();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 mousePos = point.ReadValue<Vector2>() - new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2);
        if (mousePos.x > mousePos.y)
        {
            if (mousePos.x > -mousePos.y)
            {
                playerMovement.MoveRight();
            }
            else
            {
                playerMovement.MoveDown();
            }
        }
        else
        {
            if (mousePos.x > -mousePos.y)
            {
                playerMovement.MoveUp();
            }
            else
            {
                playerMovement.MoveLeft();
            }
        }
    }
}
