using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelPreviewWindow : EditorWindow
{
    [MenuItem("Kaomi Bear Factory/Level Preview")]
    static void ShowWindow()
    {
        // Get existing open window or if none, make a new one:
        LevelPreviewWindow window = (LevelPreviewWindow)EditorWindow.GetWindow(typeof(LevelPreviewWindow));
        window.titleContent.text = "Level Preview";
        window.Show();
    }

    private LevelData _level;

    private IEnumerable<Request> _requests;

    private Sprite _cardboardBoxSprite;
    private Sprite _giftBoxSprite;

    private Vector2 _scrollPos;

    void Awake()
    {
        this._cardboardBoxSprite = (Sprite) AssetDatabase.LoadAssetAtPath("Assets/UI/Boxes/CardboardBox.png", typeof(Sprite));
        this._giftBoxSprite = (Sprite) AssetDatabase.LoadAssetAtPath("Assets/UI/Boxes/GiftBox.png", typeof(Sprite));
        Selection.selectionChanged += this.Repaint;
    }



    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        Debug.Log(Selection.activeObject is LevelData);
        if (Selection.activeObject != this._level && Selection.activeObject is LevelData)
        {
            this._level = (LevelData) Selection.activeObject;
            this._requests = null;
        }
        //this._level = (LevelData) EditorGUILayout.ObjectField("Level Data", this._level, typeof(LevelData), false);

        if (this._level != null)
        {
            if (this._level.seed == 0 && GUILayout.Button("New Random"))
            {
                this._requests = null;
            }

            if (this._requests == null)
            {
                this._requests = this._level?.GetRequests();
            }
        }

        if (this._requests != null)
        {
            EditorGUILayout.Space();

            float height = 90;

            this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos);

            foreach (var request in this._requests)
            {
                GUILayout.BeginHorizontal(GUILayout.Height(height + 10));
                this._DrawSprite(request.customer.customerPortrait, height);
                this._DrawRequestProduct(request, height);
                this._DrawSprite(this._GetDeliveryBoxSprite(request), height);
                GUILayout.Label(request.maximumTime + " seconds", GUILayout.ExpandHeight(true));
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
    }

    private void _DrawRequestProduct(Request request, float size)
    {
        Rect rect = GUILayoutUtility.GetAspectRect(1f, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(size));

        var oldColor = GUI.color;
        foreach (var piece in request.GetPieces())
        {
            GUI.color = piece.GetColor();
            Sprite sprite = piece.GetSprite();
            GUI.DrawTexture(rect, sprite.texture, ScaleMode.ScaleToFit);
        }
        GUI.color = oldColor;
    }

    private void _DrawCustomerPortrait(CustomerData customer, float size)
    {
        Sprite customerPortrait = customer.customerPortrait;
        
    }

    private void _DrawSprite(Sprite sprite, float size)
    {
        Rect rect = GUILayoutUtility.GetAspectRect(1f, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(size));
        GUI.DrawTexture(rect, sprite.texture, ScaleMode.ScaleToFit);
    }


    private Sprite _GetDeliveryBoxSprite(Request request)
    {
        return request.deliveryBoxType == DeliveryBoxType.Cardboard 
            ? this._cardboardBoxSprite 
            : this._giftBoxSprite;
    }
}