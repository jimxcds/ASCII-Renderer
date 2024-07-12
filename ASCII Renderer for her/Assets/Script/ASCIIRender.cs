using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ASCIIRender : MonoBehaviour
{
    public RenderTexture mRenderTexture;
    public int rtWidth = 256;
    public int rtHeight = 144;
    private Texture2D _m2DTexture;

    public AnimationCurve mCurve;

    public Font textFont;
    public int fontSize;
    private string _asciiArt;
    private GUIStyle _asciiArtStyle;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize _m2DTexture with the same dimensions as the RenderTexture
        mRenderTexture.width = rtWidth;
        mRenderTexture.height = rtHeight;
        _m2DTexture = new Texture2D(rtWidth, rtHeight, TextureFormat.RGB24, false);
    }

    // Update is called once per frame
    void Update()
    {
        // Set the active RenderTexture
        RenderTexture.active = mRenderTexture;

        // Read pixels from the active RenderTexture
        _m2DTexture.ReadPixels(new Rect(0, 0, mRenderTexture.width, mRenderTexture.height), 0, 0);
        _m2DTexture.Apply(); // Apply changes to the Texture2D

        // Parse the texture to ASCII
        _asciiArt = ParseTexture(_m2DTexture);

        // Reset the active RenderTexture
        RenderTexture.active = null;
    }

    private void OnGUI()
    {
        if (_asciiArtStyle == null)
        {
            _asciiArtStyle = new GUIStyle(GUI.skin.label);
            _asciiArtStyle.fontSize = fontSize;
            _asciiArtStyle.font = textFont;
        }
        // Render the ASCII art using IMGUI
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), _asciiArt, _asciiArtStyle);
    }

    private string ParseTexture(Texture2D texture)
    {
        StringBuilder sb = new StringBuilder();

        for (int y = texture.height - 1; y >= 0; y--)
        {
            for (int x = 0; x < texture.width; x++)
            {
                var color = texture.GetPixel(x, y);
                float grayScale = mCurve.Evaluate(color.grayscale);
                sb.Append(GetCharacterFromGrayScale(grayScale));
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private readonly char[] _mSortedCharacters = { ' ', '.', ':', '-', '=', '+', '*', '#', 'P', '@' };

    private char GetCharacterFromGrayScale(float grayScale)
    {
        int index = Mathf.Clamp((int)(grayScale * (_mSortedCharacters.Length - 1)), 0, _mSortedCharacters.Length - 1);
        return _mSortedCharacters[index];
        //return _mSortedCharacters[(int)(grayScale * _mSortedCharacters.Length)];
    }

    private void InitializeTextures(int width, int height)
    {
        mRenderTexture = new RenderTexture(width, height, 24);
        mRenderTexture.Create();
    }
}
