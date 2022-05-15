using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This handles screen scaling and positioning based on screen resolution/size.
/// </summary>
[ExecuteInEditMode]
public class ScreenScaler : MonoBehaviour
{
    public Text dbgText;
    public Transform camera;
    public GameObject UI_Narrow;
    public GameObject UI_Wide;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        float startVal_x = -1.57f;
        float endVal_x = 1.8f;

        float startVal_y = 0.25f;
        float endVal_y = 0.25f;

        float startVal_z = -27.08f;
        float endVal_z = -11.03f;

        float screenRatio = (float)Screen.width / (float)Screen.height;

        float calcValue = screenRatio;
        calcValue -= 1.333f;
        calcValue *= 3.0f;

        if (calcValue > 1.0f) calcValue = 1.0f;
        else if (calcValue < 0.0f)
        {
            calcValue = 0.0f;
        }

        float camX = Mathf.Lerp(endVal_x, startVal_x, calcValue);

        // Y Calcs
        float camZ = adjustedValue(.445f, 1.111f, screenRatio);
        camZ = Mathf.Lerp(startVal_z, endVal_z, camZ);

        camera.position = new Vector3(camX, 0.25f, camZ);

        string dbgStr = "Width: " + Screen.width + "\n" +
            "Height: " + Screen.height + "\n" +
            "Ratio: " + ((float)Screen.width / (float)Screen.height) + "\n" +    // 2.05 = 18.5:9  1.777 = 16.9, 1.6 = 16:10, 1.333 = 4:3 
        "Cam X: " + camX + "\n" +
        "CalcValue: " + calcValue + "\n";
        dbgText.text = dbgStr;
    }

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
    }
}
