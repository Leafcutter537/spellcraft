using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
