using UnityEngine;

public class MainMenuActiviator : MonoBehaviour
{

    [SerializeField] private GameObject mainMenuUI;

    public void GetMenu()
    {
        mainMenuUI.SetActive(true);
    }
}
