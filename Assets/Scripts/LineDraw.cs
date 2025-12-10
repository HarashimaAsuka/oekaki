using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineDraw : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [Header("Line Settings")]
    public Material lineMaterial;          // 白黒模様用（Wrap: Repeat）
    public float lineWidth = 0.1f;
    public float textureRepeatRate = 1f;

    private float fixedZ = -1f;

    void Start()
    {
        // ここでゲームを停止して OK！
        Time.timeScale = 0;
    }

    // EventTrigger: PointerDown
    public void OnPointerDown(BaseEventData data)
    {
        var p = data as PointerEventData;
        Vector3 pos = GetWorldPos(p);

        // 新規 LineRenderer 作成
        var obj = new GameObject("Line");
        lineRenderer = obj.AddComponent<LineRenderer>();

        // LineRenderer 設定
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = true;
        lineRenderer.numCapVertices = 5;
        lineRenderer.textureMode = LineTextureMode.Tile;

        // 始点セット
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, pos);
    }

    // EventTrigger: Drag
    public void OnDrag(BaseEventData data)
    {
        if (lineRenderer == null) return;

        var p = data as PointerEventData;
        Vector3 pos = GetWorldPos(p);

        int index = lineRenderer.positionCount;
        lineRenderer.positionCount = index + 1;
        lineRenderer.SetPosition(index, pos);

        // テクスチャ繰り返し
        float length = CalculateLineLength();
        lineRenderer.material.mainTextureScale = new Vector2(length * textureRepeatRate, 1);
    }

    // EventTrigger: PointerUp
    public void OnPointerUp(BaseEventData data)
    {
        if (lineRenderer == null) return;

        // コライダー付与
        var col = lineRenderer.gameObject.AddComponent<EdgeCollider2D>();
        var points = new List<Vector2>();

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 p = lineRenderer.GetPosition(i);
            points.Add(new Vector2(p.x, p.y));
        }

        col.SetPoints(points);
        Time.timeScale = 1;
    }

    // ==== 重要：TimeScale=0でも正しくワールド座標を取得 ====
    private Vector3 GetWorldPos(PointerEventData p)
    {
        Vector3 pos = p.pointerCurrentRaycast.worldPosition;

        // worldPosition が 0,0,0 になるバグ対策
        if (pos == Vector3.zero)
        {
            pos = Camera.main.ScreenToWorldPoint(
                new Vector3(p.position.x, p.position.y, 10f)
            );
        }

        pos.z = fixedZ;
        return pos;
    }

    // 線の長さ計算
    private float CalculateLineLength()
    {
        float len = 0f;
        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            len += Vector3.Distance(
                lineRenderer.GetPosition(i - 1),
                lineRenderer.GetPosition(i)
            );
        }
        return len;
    }
}







// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;

// public class LineDraw : MonoBehaviour
// {
//     LineRenderer lineRenderer;

//     // LineRenderer用のマテリアル
//     public Material material;

//     void Start()
//     {
//         // 物理計算の時間の進みを０にする
//         Time.timeScale = 0;
//     }

//     // EventTriggerのPointerDownイベントに割り当てる
//     public void OnPointerDown(BaseEventData data)
//     {
//         var p = data as PointerEventData;
//         var pos = p.pointerCurrentRaycast.worldPosition;

//         // GameObjectを生成
//         var obj = new GameObject();
//         // 生成したGameObjectにLineRendererコンポーネントを割り当てる
//         this.lineRenderer = obj.AddComponent<LineRenderer>();
//         // LineRendererの各種設定を変更
//         this.lineRenderer.startWidth = 0.1f;//線の太さ
//         this.lineRenderer.endWidth = 0.1f;//線の太さ
//         // this.lineRenderer.startColor = new Color(1, 0, 0);//色変更
//         // this.lineRenderer.endColor = new Color(1, 0, 0);//色変更
//         this.lineRenderer.useWorldSpace = true;
//         this.lineRenderer.material = this.material;
//         // 始点を追加
//         this.lineRenderer.positionCount = 1;
//         this.lineRenderer.SetPosition(0, pos);
//     }

//     // EventTriggerのDragイベントに割り当てる
//     public void OnDrag(BaseEventData data)
//     {
//         var p = data as PointerEventData;
//         var pos = p.pointerCurrentRaycast.worldPosition;

//         // ドラッグ操作時のマウスカーソル位置をLineRendererに追加
//         var index = this.lineRenderer.positionCount;
//         this.lineRenderer.positionCount = index + 1;
//         this.lineRenderer.SetPosition(index, pos);
//     }

//     // EventTriggerのPointerUpイベントに割り当てる
//     public void OnPointerUp(BaseEventData data)
//     {
//         var collider = this.lineRenderer.gameObject.AddComponent<EdgeCollider2D>();
//         var points = new List<Vector2>();
//         for (int i = 0; i < this.lineRenderer.positionCount; ++i)
//         {
//             var p = this.lineRenderer.GetPosition(i);
//             points.Add(new Vector2(p.x, p.y));
//         }
//         collider.SetPoints(points);

//         // 物理計算の時間の進みを元に戻す
//         Time.timeScale = 1;
//     }
// }