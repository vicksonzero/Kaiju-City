using System;
using UnityEngine;
using System.Collections;
using System.IO;

public class Screenshot : MonoBehaviour
{
    static Screenshot instance;

    // ctrl-shift-k
    public void TakeScreenshotInEditor()
    {
        TakeScreenshot();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            TakeScreenshot();
        }
    }

    public static void TakeScreenshot()
    {
        TakeScreenshot(sprite =>
        {
            // encode the Texture2D to a PNG
            // you might want to change this to JPG for way less file size but slightly worse quality
            // if you do don't forget to also change the file extension below
            var bytes = sprite.texture.EncodeToPNG();

            // finally write the file e.g. to the StreamingAssets folder
            var timestamp = System.DateTime.Now;
            var timestampString =
                $"_{timestamp.Year}-{timestamp.Month:00}-{timestamp.Day:00}_{timestamp.Hour:00}-{timestamp.Minute:00}-{timestamp.Second:00}";
            var filePath =
#if UNITY_EDITOR
                Path.Combine(Application.persistentDataPath, "Screenshot" + timestampString + ".png");
#else
                Path.Combine(Application.persistentDataPath, "Screenshot" + timestampString + ".png");
#endif


            File.WriteAllBytes(filePath, bytes);
            Debug.Log($"Screenshot written to {filePath}");
        });
    }


    public static void TakeScreenshot(System.Action<Sprite> callback)
    {
        if (instance == null)
        {
            instance = new GameObject("ScreenshotGO").AddComponent<Screenshot>();
        }

        instance.StartCoroutine(RecordFrame(tex =>
        {
            callback(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0)));
        }));
    }

    static IEnumerator RecordFrame(System.Action<Texture2D> callback)
    {
        yield return new WaitForEndOfFrame();
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        callback(texture);
    }
}