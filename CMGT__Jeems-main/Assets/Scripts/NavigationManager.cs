using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public GameObject chatButton;
    public GameObject vergaderenButton;
    public GameObject bestandenButton;
    public GameObject outlookButton;

    [Header("Detail Panels")]
    public GameObject chatPanel;
    public GameObject vergaderenPanel;
    public GameObject bestandenPanel;
    public GameObject outlookPanel;

    private void Start()
    {
        // Automatically default to the main menu view when the game launches
        OpenMainpanel();
    }

    public void OpenChatPanel()
    {
        SetMenuButtonsActive(false);
        CloseAllPanels();
        chatPanel.SetActive(true);
    }

    public void OpenVergaderenPanel()
    {
        SetMenuButtonsActive(false);
        CloseAllPanels();
        vergaderenPanel.SetActive(true);
    }

    public void OpenBestandenPanel()
    {
        SetMenuButtonsActive(false);
        CloseAllPanels();
        bestandenPanel.SetActive(true);
    }

    public void OpenOutlookPanel()
    {
        SetMenuButtonsActive(false);
        CloseAllPanels();
        outlookPanel.SetActive(true);
    }

    public void OpenMainpanel()
    {
        // Show all main menu buttons and hide all sub-pages
        SetMenuButtonsActive(true);
        CloseAllPanels();
    }

    private void SetMenuButtonsActive(bool isActive)
    {
        if (chatButton != null) chatButton.SetActive(isActive);
        if (vergaderenButton != null) vergaderenButton.SetActive(isActive);
        if (bestandenButton != null) bestandenButton.SetActive(isActive);
        if (outlookButton != null) outlookButton.SetActive(isActive);
    }

    private void CloseAllPanels()
    {
        if (chatPanel != null) chatPanel.SetActive(false);
        if (vergaderenPanel != null) vergaderenPanel.SetActive(false);
        if (bestandenPanel != null) bestandenPanel.SetActive(false);
        if (outlookPanel != null) outlookPanel.SetActive(false);
    }
}