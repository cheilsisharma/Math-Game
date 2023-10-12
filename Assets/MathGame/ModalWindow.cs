using UnityEngine;
using UnityEngine.SceneManagement;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] private GameObject modalPanel;
    [SerializeField] private GameObject modalPanel1;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject controls;

    public void ShowModalWithInstructions()
    {
        modalPanel.SetActive(true);
        modalPanel1.SetActive(false);
        settings.SetActive(false);
    }

    public void ShowEscapeModal()
    {
        modalPanel1.SetActive(true);
        settings.SetActive(false);
        controls.SetActive(false);
    }

    public void ShowSettings()
    {
        settings.SetActive(true);
        modalPanel1.SetActive(false);
    }

    public void ShowControls()
    {
        controls.SetActive(true);
        modalPanel1.SetActive(false);
    }

    public void HideModal()
    {
        modalPanel.SetActive(false);
        modalPanel1.SetActive(false);
    }

    public void OnContinueButtonClicked()
    {
        HideModal();
    }

  
    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowEscapeModal();
        }
    }

    public void ExitMainGame()
    {
        SceneManager.LoadScene("MainGameUI");
    }
}
