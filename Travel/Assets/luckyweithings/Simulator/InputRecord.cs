using UnityEngine;
using System.IO;


public enum MouseType
{
    LeftDown,
    LeftUp,
    RightDown,
    RightUp,
    Move
}

public enum TouchType
{
    Mouse,
    Touch
}

public class InputRecord : MonoBehaviour {

    struct Point
    {
        public int x;
        public int y;

        public override string ToString()
        {
            return "x " + x + " y " + y;
        }
    }

    private StreamWriter sw;
    private bool move = false;

    private float startx;
    private bool once = true;

    private int height;
    private int width;

    private static string m_HorizontalAxis = "Horizontal";
    private static string m_VerticalAxis = "Vertical";
    private static string m_SubmitButton = "Submit";
    private static string m_CancelButton = "Cancel";

#if UNITY_EDITOR

    UnityEditor.EditorWindow gameView;

#endif

    private bool leftup = false;

    private void Start()
    {
        sw = FileManager.GetStreamWriter((FileManager.GetFilePath("Input.rec")));
        
        if (sw == null)
            return;

        Debug.Log("luckyhigh Input Record start");

#if UNITY_EDITOR

        // 获得gameView窗口
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        gameView = UnityEditor.EditorWindow.GetWindow(T, true, "Game") as UnityEditor.EditorWindow;

        // 获得gameView中渲染窗口的分辨率
        var prop = gameView.GetType().GetProperty("currentGameViewSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var gvsize = prop.GetValue(gameView, new object[0] { });
        var gvSizeType = gvsize.GetType();
        
        height = (int)gvSizeType.GetProperty("height", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0] { });
        width = (int)gvSizeType.GetProperty("width", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0] { });

#elif UNITY_ANDROID

        height = Screen.currentResolution.height;
        width = Screen.currentResolution.width;
#endif

        StreamWriter ssww = FileManager.GetStreamWriter((FileManager.GetFilePath("Resolution.rec")));
        ssww.WriteLine(height + " x " + width);
        ssww.Close();
    }

    void Update () {

        if (sw == null)
            return;

#if UNITY_EDITOR

        if(Input.GetMouseButtonDown(0))
        {
            WriteMouseMessage(MouseType.LeftDown);
            move = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            WriteMouseMessage(MouseType.LeftUp);
            move = false;
            leftup = true;
        }

        if (move)
        {
            if(!Input.GetMouseButtonDown(0))
            {
                WriteMouseMessage(MouseType.Move);
            }
                
        }

        if(leftup)
        {
            if(!Input.GetMouseButtonUp(0))
            {
                WriteMouseMessage(MouseType.Move);
                leftup = false;
            }
        }

#elif UNITY_ANDROID
        
        if(Input.touchCount==1 && (Input.GetTouch(0).phase==TouchPhase.Began))
        {
            WriteTouchMessage(MouseType.LeftDown, 0);
        }

        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            WriteTouchMessage(MouseType.Move, 0);
        }

        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            WriteTouchMessage(MouseType.LeftUp, 0);
        }

#endif
    }

    private void WriteTouchMessage(MouseType type, int num = 0)
    {
        Vector2 pos = Input.GetTouch(num).position;

        float time = Time.realtimeSinceStartup;
        float xprecent = pos.x / (float)width;
        float yprecent = pos.y / (float)height;

        string message = time + " " + (int)type + " " + xprecent + " " + yprecent;

        sw.WriteLine(message);
        sw.Flush();
    }

#if UNITY_EDITOR
    private void WriteMouseMessage(MouseType type)
    {
        float time = Time.realtimeSinceStartup;

        Vector3 pos = Input.mousePosition;
        

        if (pos.x < 0 || pos.y < 0)
            return;
        /*
        float realh = gameView.position.height - 17;
        float realw = gameView.position.width;

        float expectw = realh * width / height;

        float xprecent = pos.x / (float)expectw;
        float yprecent = pos.y / (float)realh;
        */

        Debug.Log("lucky mouse " + pos);

        float xprecent = pos.x / (float)width;
        float yprecent = pos.y / (float)height;

        string message = time + " " + (int)type + " " + xprecent + " " + yprecent;
        sw.WriteLine(message);
        sw.Flush();
    }
#endif

    private void OnDestroy()
    {
        if(sw != null)
        {
            sw.Close();
        }

    }
}
