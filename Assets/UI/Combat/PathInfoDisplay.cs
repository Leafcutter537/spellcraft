using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.EventSystem;
using TMPro;
using UnityEngine;

public class PathInfoDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI pathNameText;
    [SerializeField] private TextMeshProUGUI pathDescriptionText;
    [Header("Event References")]
    [SerializeField] private MouseEnterPathEvent mouseEnterPathEvent;
    [SerializeField] private MouseExitPathEvent mouseExitPathEvent;

    private void OnEnable()
    {
        mouseEnterPathEvent.AddListener(OnMouseEnterPath);
        mouseExitPathEvent.AddListener(OnMouseExitPath);
    }
    private void OnDisable()
    {
        mouseEnterPathEvent.RemoveListener(OnMouseEnterPath);
        mouseExitPathEvent.RemoveListener(OnMouseExitPath);
    }
    private void OnMouseEnterPath(object sender, EventParameters args)
    {
        Path path = sender as Path;
        pathNameText.text = path.pathName;
        pathDescriptionText.text = "";
        if (path.playerProjectile != null)
        {
            pathDescriptionText.text += path.playerProjectile.GetProjectileDescription() + "\n\n";
        }
        if (path.enemyProjectile != null)
        {
            pathDescriptionText.text += path.enemyProjectile.GetProjectileDescription() + "\n\n";
        }
        if (path.playerShield != null)
        {
            pathDescriptionText.text += path.playerShield.GetShieldDescription() + "\n\n";
        }
        if (path.enemyShield != null)
        {
            pathDescriptionText.text += path.enemyShield.GetShieldDescription() + "\n\n";
        }
    }
    private void OnMouseExitPath(object sender, EventParameters args)
    {
        pathNameText.text = "";
        pathDescriptionText.text = "";
    }
}
