using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyInput {

    private static Vector3 mousePos = Vector3.zero;
    public static Vector3 mousePosition
    {
        get
        {
            return mousePos;
        }
        set
        {
            mousePos = value;
        }
    }

    private static bool mousePre = true;
    public static bool mousePresent
    {
        get
        {
            return mousePre;
        }
        set
        {
            mousePre = value;
        }
    }

    private static bool touchSup = false;
    public static bool touchSupported
    {
        get
        {
            return touchSup;
        }
        set
        {
            touchSup = value;
        }
    }

    private static int touchCou;
    public static int touchCount
    {
        get
        {
            return 0;
        }
    }

    private static Vector2 mouseScroll = Vector2.zero;
    public static Vector2 mouseScrollDelta
    {
        get
        {
            return mouseScroll;
        }
        set
        {
            mouseScroll = value;
        }
    }

    private static bool[] mouseButtonDown = { false, false, false };
    private static bool[] mouseButtonUp = { false, false, false };
    public static void SetMouseButtonDown(int i)
    {
        mouseButtonDown[i] = true;
        mouseButtonUp[i] = false;
    }

    public static void SetMouseButtonUp(int i)
    {
        mouseButtonDown[i] = false;
        mouseButtonUp[i] = true;
    }

    public static bool GetMouseButtonUp(int i)
    {
        bool result = mouseButtonUp[i];
        return result;
    }

    public static bool GetMouseButtonDown(int i)
    {
        bool result = mouseButtonDown[i];
        return result;
    }

    public static void ClearMouseButton()
    {
        for(int i=0;i<mouseButtonDown.Length;i++)
        {
            mouseButtonDown[i] = false;
            mouseButtonUp[i] = false;
        }
    }


    protected static string m_HorizontalAxis = "Horizontal";

    /// <summary>
    /// Name of the vertical axis for movement (if axis events are used).
    /// </summary>
    protected static string m_VerticalAxis = "Vertical";

    /// <summary>
    /// Name of the submit button.
    /// </summary>
    protected static string m_SubmitButton = "Submit";

    /// <summary>
    /// Name of the submit button.
    /// </summary>
    protected static string m_CancelButton = "Cancel";

    private static Dictionary<string, bool> buttonType = new Dictionary<string, bool>();
    public static bool GetButtonDown(string buttonName)
    {
        bool result = buttonType[buttonName];
        if (result)
            buttonType[buttonName] = false;
        return result;
    }

    public static void SetButtonDown(string buttonName)
    {
        buttonType[buttonName] = true;
    }

    private static Dictionary<string, float> axisType = new Dictionary<string, float>();
    public static float GetAxisRaw(string axisName)
    {
        float result = axisType[axisName];
        return result;
    }

    public static void SetAxisRaw(string axisName, float i)
    {
        axisType[axisName] = i;
    }

    public static void Init()
    {
        Debug.Log("lucky buttontype " + buttonType.Count);
        //unchange
        buttonType.Add(m_HorizontalAxis, false);
        buttonType.Add(m_VerticalAxis, false);
        buttonType.Add(m_SubmitButton, false);
        buttonType.Add(m_CancelButton, false);

        axisType.Add(m_HorizontalAxis, 0);
        axisType.Add(m_VerticalAxis, 0);

        mousePre = true;
        touchSup = false;
        mouseScrollDelta = Vector2.zero;

        //change
        mousePos = Vector3.zero;
    }
}
