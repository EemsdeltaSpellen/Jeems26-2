using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    [Header("Panels & UI")]
    public GameObject MainPanel;          // The main background panel
    public GameObject buttonContainer;    // Create an empty object holding your 4 category buttons inside MainPanel
    public GameObject chatPanel;
    public GameObject vergaderenPanel;
    public GameObject bestandenPanel;
    public GameObject outlookPanel;

    private void Start()
    {
        // Automatically set the correct starting state when the game launches
        OpenMainpanel();
    }

    // Call this from your Chat button
    public void OpenChatPanel()
    {
        SwitchPanel(chatPanel);
    }

    // Call this from your Vergaderen button
    public void OpenVergaderenPanel()
    {
        SwitchPanel(vergaderenPanel);
    }

    // Call this from your Bestanden button
    public void OpenBestandenPanel()
    {
        SwitchPanel(bestandenPanel);
    }

    // Call this from your Outlook button
    public void OpenOutlookPanel()
    {
        SwitchPanel(outlookPanel);
    }

    // Call this from any "Terug" (Back) button
    public void OpenMainpanel()
    {
        if (MainPanel != null) MainPanel.SetActive(true);
        if (buttonContainer != null) buttonContainer.SetActive(true);

        chatPanel.SetActive(false);
        vergaderenPanel.SetActive(false);
        bestandenPanel.SetActive(false);
        outlookPanel.SetActive(false);
    }

    private void SwitchPanel(GameObject targetPanel)
    {
        // Keep MainPanel active so the background/frame stays, but hide the buttons
        if (MainPanel != null) MainPanel.SetActive(true);
        if (buttonContainer != null) buttonContainer.SetActive(false);
        
        // Turn off all detail panels first
        chatPanel.SetActive(false);
        vergaderenPanel.SetActive(false);
        bestandenPanel.SetActive(false);
        outlookPanel.SetActive(false);

        // Turn on the selected target panel
        targetPanel.SetActive(true);
    }
}