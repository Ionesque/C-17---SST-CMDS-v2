using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This handles screen scaling and positioning based on screen resolution/size.
/// History so I don't forget later:
/// -Intial version tried using dynamic camera/UI adjustments
/// -Unity's UI just wouldn't play nice, coded buttons as colliders/quads
/// -Fixed camera/UI positions based on common screen ratios
/// </summary>
[ExecuteInEditMode]
public class ScreenScaler : MonoBehaviour
{
    public Transform camera;
    public GameObject UI_PhoneNarrow;
    public GameObject UI_IPAD_Narrow;
    public GameObject UI_IPAD_Wide;
    public GameObject UI_Wide;

    Vector3[] camPosition = new Vector3[4]
    {
        new Vector3(1.88f, 3.46f, -25.82f),
        new Vector3(1.82f, 2.05f, -16.9f),
        new Vector3(-0.2f, 2.12f, -14.18f),
        new Vector3(0.44f, 1.01f, -14.41f)
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        if (screenRatio < 0.72)
        {
            camera.localPosition = camPosition[0];
            UI_PhoneNarrow.SetActive(true);
            UI_IPAD_Narrow.SetActive(false);
            UI_IPAD_Wide.SetActive(false);
            UI_Wide.SetActive(false);
        }
        else  if(screenRatio < 1.1)
        {
            camera.localPosition = camPosition[1];
            UI_PhoneNarrow.SetActive(false);
            UI_IPAD_Narrow.SetActive(true);
            UI_IPAD_Wide.SetActive(false);
            UI_Wide.SetActive(false);
        }
        else if(screenRatio < 1.4)
        {
            camera.localPosition = camPosition[2];
            UI_PhoneNarrow.SetActive(false);
            UI_IPAD_Narrow.SetActive(false);
            UI_IPAD_Wide.SetActive(true);
            UI_Wide.SetActive(false);
        }
        else
        {
            camera.localPosition = camPosition[3];
            UI_PhoneNarrow.SetActive(false);
            UI_IPAD_Narrow.SetActive(false);
            UI_IPAD_Wide.SetActive(false);
            UI_Wide.SetActive(true);
        }

    }

    /* - Dynamic camera LERP value code. No longer used for this, but I want to hold on to it for ther uses later.
    float adjustedValue(float LowValue, float HighValue, float InValue)
    {
        float multiplier = 1 / (HighValue - LowValue);
        float rawValue = InValue - LowValue;
        float calcValue = rawValue * multiplier;
        if (calcValue > 1.0f) calcValue = 1.0f;
        else if (calcValue < 0.0f)
        {
            calcValue = 0.0f;
        }
        return calcValue;
    }*/
}
