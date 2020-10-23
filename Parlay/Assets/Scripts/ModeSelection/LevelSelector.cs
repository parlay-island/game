using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// tutorial follow: https://www.youtube.com/watch?v=tCr_i5CVv_w

public class LevelSelector : MonoBehaviour
{

    [SerializeField] private GameObject levelHolder;
    [SerializeField] private GameObject levelIcon;
    [SerializeField] private GameObject thisCanvas;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private Vector2 iconSpacing;
    [SerializeField] private ErrorDisplaySource errorDisplaySource;
    [SerializeField] private int amountPerPage = 9;


    private Rect panelDimensions;
    private Rect iconDimensions;
    private int currentLevelCount;
    private int numberOfLevels;
    private int totalPages;
    private int currentPage = 1;
    private List<GameObject> panels = new List<GameObject>();
    private List<LevelModel> levels;


    void Start()
    {
      try
      {
        ModeRetriever mode_retriever = GameObject.Find("ModeRetriever").GetComponent<ModeRetriever>();
        levels = mode_retriever.GetLevels();
        numberOfLevels = levels.Count;

        panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        iconDimensions = levelIcon.GetComponent<RectTransform>().rect;
        totalPages = Mathf.CeilToInt((float)numberOfLevels / amountPerPage);
        LoadPanels(totalPages);
        CheckAndActivateButtons();
      }
      catch (Exception e)
      {
        errorDisplaySource.DisplayNewError("Cannot load levels", "An error occurred while loading "
                        + "levels. Please try again later.");
      }
    }

    private void LoadPanels(int numberOfPanels){
        GameObject panelClone = Instantiate(levelHolder) as GameObject;

        for(int i = 1; i <= numberOfPanels; i++){
            GameObject panel = Instantiate(panelClone) as GameObject;
            if(i != 1)
            {
              panel.SetActive(false);
            }
            panel.transform.SetParent(thisCanvas.transform, false);
            panel.transform.SetParent(levelHolder.transform);
            panel.name = "Page-" + i;
            panel.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            SetUpGrid(panel);
            int numberOfIcons = i == numberOfPanels ? numberOfLevels - currentLevelCount : amountPerPage;
            LoadIcons(numberOfIcons, panel);
            panels.Add(panel);
        }
        Destroy(panelClone);
    }

    private void SetUpGrid(GameObject panel){
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(iconDimensions.width, iconDimensions.height);
        grid.childAlignment = TextAnchor.UpperLeft;
        grid.spacing = iconSpacing;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 3;
    }

    private void LoadIcons(int numberOfIcons, GameObject parentObject){
        for(int i = 1; i <= numberOfIcons; i++){
            currentLevelCount++;
            GameObject icon = Instantiate(levelIcon) as GameObject;
            LevelModel currLevel = levels[i - 1];
            icon.transform.SetParent(thisCanvas.transform, false);
            icon.transform.SetParent(parentObject.transform);
            icon.name = currLevel.name;
            icon.GetComponentInChildren<TextMeshProUGUI>().SetText(currLevel.name);
        }
    }

    public void LeftButtonPress()
    {
      if(currentPage > 1)
      {
        panels[currentPage - 1].SetActive(false);
        panels[currentPage - 2].SetActive(true);
        currentPage--;
        CheckAndActivateButtons();
      }
    }

    public void RightButtonPress()
    {
      if(currentPage < totalPages)
      {
        panels[currentPage - 1].SetActive(false);
        panels[currentPage].SetActive(true);
        currentPage++;
        CheckAndActivateButtons();
      }
    }

    private void CheckAndActivateButtons()
    {
      if(currentPage == 1)
      {
        leftButton.SetActive(false);
      } else if (!leftButton.activeSelf){
        leftButton.SetActive(true);
      }

      if(currentPage == totalPages)
      {
        rightButton.SetActive(false);
      } else if (!rightButton.activeSelf){
        rightButton.SetActive(true);
      }
    }

}
