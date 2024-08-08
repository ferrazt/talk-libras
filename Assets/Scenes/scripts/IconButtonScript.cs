using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class IconButtonScript : MonoBehaviour
{
    private Animator animator;
    private bool IsOn = false;
    public ToggleButtonScript toggleButtonScript;
    public AudioClient audioClient;

    void Start()
    {
        animator = GetComponent<Animator>();
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(Toggle);
    }

    public async void Toggle()
    {
        IsOn = !IsOn;
        animator.SetBool("IsOn", IsOn);

        if (toggleButtonScript != null)
        {
            toggleButtonScript.SetToggle(IsOn);
        }

        if (audioClient != null)
        {
            if (IsOn)
            {
                await audioClient.StartTranscription();
            }
            else
            {
                await audioClient.StopTranscription();
            }
        }
    }

    public void SetToggle(bool state)
    {
        IsOn = state;
        animator.SetBool("IsOn", IsOn);
    }
}
