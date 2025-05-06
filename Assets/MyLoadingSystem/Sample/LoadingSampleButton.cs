using UnityEngine;
using UnityEngine.UI;
using LoadingSystem; // ’Ç‰Á‚µ‚Ä‚­‚¾‚³‚¢

public class LoadingSampleButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string YourNextSceneName = "SampleScene";

    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick); 
        }
        else
        {
            Debug.LogError("Button not assigned!");
        }
    }

    public void OnButtonClick()
    {

        LoadingSceneManager.Instance.LoadScene(YourNextSceneName);

    }
}
