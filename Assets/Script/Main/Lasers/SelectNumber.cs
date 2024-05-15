using UnityEngine;
// LINQ の使用
using System.Linq;

// ここからエディター上でのみ有効
#if UNITY_EDITOR
using UnityEditor;

// エディター拡張クラス
[CustomEditor(typeof(SelectNumber))]
public class selectnumberedEditor : Editor
{// Editor クラスを継承
    // selectnumber クラスの変数を扱うために宣言
    SelectNumber selectnumber;

    void OnEnable()
    {// 最初に実行
        // selectnumber クラスに target を代入
        selectnumber = (SelectNumber)target;
    }

    public override void OnInspectorGUI()
    {// Inspector に表示
        // これ以降の要素に関してエディタによる変更を記録
        EditorGUI.BeginChangeCheck();

        // SerializedObject の内容を更新
        serializedObject.Update();
        // Inspector のコンポーネントに表示する項目名
        var text = "List";
        // ツールチップのテキスト
        var tooltip = "Popup list \"Selectnum\" will be made out of this.";
        // ツールチップ入りラベルの作成
        var label = new GUIContent(text, tooltip);
        // 配列の中身も表示する
        var includeChildren = true;
        // リストを取得
        var property = serializedObject.FindProperty(nameof(selectnumber.list));
        // 配列フィールドの作成
        EditorGUILayout.PropertyField(property, label, includeChildren);
        // SerializedObject の変更を適用
        serializedObject.ApplyModifiedProperties();

        // インデックス番号を付与するデリゲート
        System.Func<string, int, string> selector = (string name, int number) => $"{number}: \r{name}";
        // ドロップダウンの項目にインデックス番号を付与
        var list = selectnumber.list.Select(selector).ToList();
        // 仕切り
        var divider = string.Empty;
        // 未選択状態
        var unselected = "(Unselected)";
        // リストに仕切りを追加
        list.Add(divider);
        // 未選択状態を追加
        list.Add(unselected);
        // Inspector のコンポーネントに表示する項目名
        text = "Selectnum";
        // ツールチップのテキスト
        tooltip = "Select one item.";
        // ツールチップ入りラベルの作成
        label = new GUIContent(text, tooltip);
        // 初期値として表示する項目のインデックス番号
        var selectedIndex = selectnumber.list.Length == 0 ? -1
            : selectnumber.index < 0 ? selectnumber.list.Length + 1
            : selectnumber.index;
        // プルダウンメニューに登録する文字列配列
        var displayOptions = list.ToArray();
        // プルダウンメニューの作成
        var index = selectnumber.list.Length > 0 ? EditorGUILayout.Popup(label, selectedIndex, displayOptions)
            : selectedIndex;

        if (EditorGUI.EndChangeCheck())
        {// 操作を Undo に登録
            // selectnumber クラスの変更を記録
            var objectToUndo = selectnumber;
            // Undo メニューに表示する項目名
            var name = "selectnumber";
            // 記録準備
            Undo.RecordObject(objectToUndo, name);
            // インデックス番号を登録
            selectnumber.index = index;
        }

        // 未選択状態のインデックス番号を -1 とする
        selectnumber.index = index > selectnumber.list.Length ? -1
            : index;

        if ((selectnumber.index != selectedIndex) && (selectnumber.index >= 0))
        {// 選択した項目のインデックス番号と項目名をログに出力
            Debug.Log(selectnumber.index);
            Debug.Log(selectnumber.list[selectnumber.index]);
        }
    }
}
// ここまでエディター上でのみ有効
#endif

// 同一オブジェクトへの複数追加を禁止
[DisallowMultipleComponent]
public class SelectNumber : MonoBehaviour

{// エディター拡張の中身
    // リスト
    public string[] list = { };
    // 初期値は“未選択”
    public int index = -1;

    //public int GetindexCount()
    //{
    //    return index;
    //}
}