using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Scrolls;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class ScrollDisplay : MonoBehaviour
{
    [Header("Rune In Scroll Display")]
    [SerializeField] private GameObject runeInScrollDisplayPrefab;
    [SerializeField] private GameObject runeInScrollConnectionPrefab;
    [SerializeField] private float runeDisplayPadding;
    [SerializeField] private Transform displayCenter;
    public ScrollData scrollData;
    private float runeDisplayWidth;
    private float runeDisplayHeight;
    [SerializeField] private float lineThickness;
    [Header("Serialized Object References")]
    [SerializeField] private ScrollStock scrollStock;
    [Header("References In Scene")]
    [SerializeField] private SpellPreview spellPreview;
    [Header("Instantiated Game Objects")]
    private List<RuneSelectPanelChoice> runeSlots;
    private List<GameObject> connections;

    private void Awake()
    {
        runeDisplayHeight = runeInScrollDisplayPrefab.GetComponent<RectTransform>().rect.height;
        runeDisplayWidth = runeInScrollDisplayPrefab.GetComponent<RectTransform>().rect.width;
    }

    public void ChooseScroll(ScrollData scrollData)
    {
        this.scrollData = scrollData;
        ClearScrollDisplay();
        CreateScrollDisplay();
    }

    private void ClearScrollDisplay()
    {
        if (runeSlots != null)
        {
            foreach (RuneSelectPanelChoice runeSlot in runeSlots)
            {
                Destroy(runeSlot.gameObject);
            }
        }
        if (connections != null)
        {
            foreach (GameObject connection in connections)
            {
                Destroy(connection.gameObject);
            }
        }
        runeSlots = new List<RuneSelectPanelChoice>();
        connections = new List<GameObject>();
    }

    private void CreateScrollDisplay()
    {
        CreateAllRuneDisplays();
        CreateConnectionLines();
        LinkSpellPreviewToDisplayedScroll();
        foreach (GameObject connection in connections)
        {
            connection.transform.SetParent(displayCenter, true);
        }
        foreach (RuneSelectPanelChoice runeSlot in runeSlots)
        {
            runeSlot.transform.SetParent(displayCenter, true);
        }
    }

    private void CreateAllRuneDisplays()
    {
        int numLevels = scrollData.numPerLevel.Length;
        for (int level = 0; level < numLevels; level++)
        {
            float y = displayCenter.position.y + (level - (numLevels - 1.0f) / 2.0f) * (runeDisplayHeight + runeDisplayPadding);
            CreateLevelRuneDisplays(scrollData.numPerLevel[level], displayCenter.position.x, y);
        }
    }

    private void CreateLevelRuneDisplays(int num, float centerx, float centery)
    {
        for (int i = 0; i < num; i++)
        {
            float x = displayCenter.position.x + (i - (num - 1.0f) / 2.0f) * (runeDisplayWidth + runeDisplayPadding);
            GameObject runeInScrollDisplay = Instantiate(runeInScrollDisplayPrefab, new Vector2(x, centery), Quaternion.identity);
            RuneSelectPanelChoice runeSlot = runeInScrollDisplay.GetComponent<RuneSelectPanelChoice>();
            runeSlot.spellPreview = spellPreview;
            runeSlots.Add(runeSlot);
        }
    }

    private void CreateConnectionLines()
    {
        for (int i = 0; i < scrollData.connections.Length; i += 2)
        {
            CreateConnectionLine(runeSlots[scrollData.connections[i]].transform.position, runeSlots[scrollData.connections[i + 1]].transform.position);
        }
    }
    
    private void CreateConnectionLine(Vector2 start, Vector2 end)
    {
        Vector2 middle = (start + end) / 2;
        GameObject connection = Instantiate(runeInScrollConnectionPrefab, middle, Quaternion.identity);
        RectTransform connectionRect = connection.GetComponent<RectTransform>();
        connectionRect.sizeDelta = new Vector2(Vector2.Distance(start, end), lineThickness);
        connectionRect.Rotate(new Vector3(0, 0, -Vector2.SignedAngle(end - start, Vector2.right)));
        connections.Add(connection);
    }

    private void LinkSpellPreviewToDisplayedScroll()
    {
        spellPreview.UpdateScroll(scrollData, runeSlots);
    }

}

