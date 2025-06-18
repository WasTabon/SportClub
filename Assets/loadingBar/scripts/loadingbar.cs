using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class loadingbar : MonoBehaviour {

    private RectTransform rectComponent;
    private Image imageComp;
    public float speed = 0.0f;
    private TextMeshProUGUI textComp;
    
    private string[] loadingFrames = { "Processing", "Processing.", "Processing..", "Processing..." };
    private float textFrameDuration = 0.3f;
    private float textTimer = 0f;
    private int currentFrame = 0;
    
    private void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;

        Transform parent = transform.parent;
        foreach (Transform child in parent)
        {
            if (child != transform)
            {
                textComp = child.GetComponent<TextMeshProUGUI>();
                break;
            }
        }

        if (textComp != null)
        {
            textComp.text = loadingFrames[0];
        }
    }

    private void Update()
    {
        if (imageComp.fillAmount < 1f)
        {
            imageComp.fillAmount += Time.deltaTime * speed;
            
            textTimer += Time.deltaTime;
            if (textTimer >= textFrameDuration)
            {
                textTimer = 0f;
                currentFrame = (currentFrame + 1) % loadingFrames.Length;
                if (textComp != null)
                {
                    textComp.text = loadingFrames[currentFrame];
                }
            }
        }
        else
        {
            imageComp.fillAmount = 0.0f;
            currentFrame = 0;
            textTimer = 0f;
            if (textComp != null)
            {
                textComp.text = loadingFrames[0];
            }
        }
    }

    public void ResetBar()
    {
        imageComp.fillAmount = 0.0f;
        currentFrame = 0;
        textTimer = 0f;
        if (textComp != null)
        {
            textComp.text = loadingFrames[0];
        }
    }
    
}
