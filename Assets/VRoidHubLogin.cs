using UnityEngine;
using UnityEngine.UI;
using VRoidSDK;
using VRoidSDK.OAuth;

public class VRoidHubLogin : MonoBehaviour
{
    [SerializeField] private Button login;
    [SerializeField] private Button register;
    [SerializeField] private GameObject codeFieldContent;
    [SerializeField] private InputField codeInputField;
    [SerializeField] private SDKConfiguration sdkConfiguration;
    [SerializeField] private Text resultText;

    private BrowserAuthorize browserAuthorize;

    private void Awake()
    {
        Authentication.Instance.Init(sdkConfiguration.AuthenticateMetaData);
        codeFieldContent.gameObject.SetActive(false);
        browserAuthorize = BrowserAuthorize.GenerateInstance(sdkConfiguration);
        login.onClick.AddListener(OnLoginButtonClicked);
        register.onClick.AddListener(OnRegisterCodeEnter);
    }

    private void OnRegisterCodeEnter()
    {
        browserAuthorize.RegisterCode(codeInputField.text);
    }

    private void AuthSuccessed()
    {
        Debug.Log("認証成功!");
        resultText.text = "result: success";
        Destroy(browserAuthorize.gameObject);
        codeFieldContent.gameObject.SetActive(false);
        login.gameObject.SetActive(false);
    }

    private void OnLoginButtonClicked()
    {
        Authentication.Instance.AuthorizeWithExistAccount(isAuthSuccess =>
            {
                if (isAuthSuccess)
                {
                    AuthSuccessed();
                }
                else
                {
                    codeFieldContent.SetActive(true);
                    browserAuthorize.OpenBrowser(AfterBrowserAuthorize);
                }
            },
            e =>
            {
                codeFieldContent.SetActive(true);
                browserAuthorize.OpenBrowser(AfterBrowserAuthorize);
            });
    }

    private void AfterBrowserAuthorize(bool isSuccess)
    {
        if (isSuccess)
        {
            AuthSuccessed();
        }
        else
        {
            codeFieldContent.SetActive(false);
        }
    }
}