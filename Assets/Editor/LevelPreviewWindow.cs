using UnityEngine;
using UnityEditor;
using System.Linq;

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

    private LevelData _level = null;

    private Request[] _requests = null;

    private Sprite _cardboardBoxSprite;
    private Sprite _giftBoxSprite;

    private Vector2 _scrollPos;

    void Awake()
    {
        this._cardboardBoxSprite = (Sprite) AssetDatabase.LoadAssetAtPath("Assets/UI/Boxes/CardboardBox.png", typeof(Sprite));
        this._giftBoxSprite = (Sprite) AssetDatabase.LoadAssetAtPath("Assets/UI/Boxes/GiftBox.png", typeof(Sprite));
        Selection.selectionChanged += this._OnSelectionChanged;

        this._OnSelectionChanged();
    }

    private void _OnSelectionChanged()
    {
        LevelData selected = (Selection.activeObject is LevelData) ? (LevelData) Selection.activeObject : null;
        this._SetLevel(selected);
        this.Repaint();
    }

    private void _SetLevel(LevelData levelData)
    {
        this._level = levelData;
        this._requests = levelData?.GetRequests().ToArray();
    }

    void OnGUI()
    {
        if (this._level == null)
        {
            EditorGUILayout.HelpBox("Select a level to preview from the project window", MessageType.Info);
            return;
        }

        if (this._level.seed == 0 && GUILayout.Button("New Random"))
        {
            this._SetLevel(this._level);
        }

        if (this._requests != null)
        {
            EditorGUILayout.Space();

            this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos);
            this._ShowRequests();
            EditorGUILayout.EndScrollView();
        }
    }

    private void _ShowRequests()
    {
        if (! this._requests.Any())
        {
            EditorGUILayout.HelpBox("No requests generated for this level. Check the level settings are valid.", MessageType.Warning);
            return;
        }

        float height = 90;
        foreach (var request in this._requests)
        {
            GUILayout.BeginHorizontal(GUILayout.Height(height + 10));
            this._DrawSprite(request.customer.customerPortrait, height);
            this._DrawRequestProduct(request, height);
            this._DrawSprite(this._GetDeliveryBoxSprite(request), height);
            GUILayout.Label(request.maximumTime + " seconds", GUILayout.ExpandHeight(true));
            GUILayout.EndHorizontal();
        }
    }

    private void _DrawRequestProduct(Request request, float size)
    {
        Rect rect = GUILayoutUtility.GetAspectRect(1f, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(size));

        var oldColor = GUI.color;
        foreach (var piece in request.pieces)
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