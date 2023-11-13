using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionScreen : MonoBehaviour
{
    public static TransitionScreen Instance;
    
    [SerializeField] private Transform transitionPanel;
    [SerializeField] private Ease easeShow = Ease.InBack;
    [SerializeField] private Ease easeHide = Ease.OutBack;
    
    [SerializeField] private float transitionDuration = .38f;
    [SerializeField] private float transitionDelay = .12f;
    
    private float hidePanelPosX;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Destroying duplicate TransitionScreen object - only one is allowed per scene!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        hidePanelPosX = transitionPanel.localPosition.x;
        SceneManager.sceneLoaded += HideTransitionPanel;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HideTransitionPanel;
    }

    
    public void OpenScene(int sceneBuildIndex)
    {
        StartCoroutine(LoadScene(sceneBuildIndex));
    }
    
    private IEnumerator LoadScene(int sceneBuildIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);
        asyncLoad.allowSceneActivation = false;
        ShowTransitionPanel();
        yield return new WaitForSeconds(transitionDelay + transitionDuration);
        asyncLoad.allowSceneActivation = true;
    }
    
    private void ShowTransitionPanel()
    {
        transitionPanel.localPosition = new Vector3(hidePanelPosX, 0, 0);
        transitionPanel.DOLocalMoveX(0, transitionDuration).SetEase(easeShow);
    }

    private void HideTransitionPanel(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;//Loading scene
        
        transitionPanel.localPosition = Vector3.zero;
        transitionPanel.DOLocalMoveX(-hidePanelPosX, transitionDuration)
            .SetEase(easeHide)
            .SetDelay(transitionDelay);
    }
}