using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Linq;


public class DAO : MonoBehaviour
{

    void Start() {
        StartCoroutine ( LoadData() );
         StartCoroutine ( SaveData() );  // テスト用でSaveDataメソッドを呼び出す
         StartCoroutine ( BurstMark() );
        // TODO : ①ボタンを押したらSaveDataメソッドを呼び出す
        //        ②バーストポイントが規定以上になったらBurstMarkメソッドを呼び出す
        //        ③引数ありメソッドで与える引数が固定値になっているので画面や入力値を入れる
        //        ④DBから取得したデータをもとにマークくんを描画する
    }

    // ボタン押下でSaveするようにする

    /**
    * データを取得.
    */
    IEnumerator LoadData() { 
        Debug.Log("[Method] LoadData");

        // selectAllMark
        IEnumerator executeAllMarkData = selectAllMark();
        yield return StartCoroutine(executeAllMarkData);
        MarkData[] allMarkData = (MarkData[])executeAllMarkData.Current;
    }

    /**
    * データを保存.
    */
    IEnumerator SaveData() {
        Debug.Log("[Method] SaveData");
        // セーブ前に最新情報を取得する.
        // selectMark
        IEnumerator executeMarkData = selectMark(1);    // 引数にはidを渡す
        yield return StartCoroutine(executeMarkData);
        MarkData markData = (MarkData)executeMarkData.Current;

        // データ更新・セーブ
        // insertComment
        StartCoroutine(insertComment(1, "残業やだ！", 3));             // 引数にはmark_id, comment, anger_pointを渡す

        // updateMark
        StartCoroutine(updateMark(1, 70, 70, 6, false));             // 引数にはid, width, height, total_anger_point, burst_flagを渡す     
    }

    /**
    * マークくんを爆発させる.
    */
    IEnumerator BurstMark() {
        Debug.Log("[Method] BurstMark");
        // selectAllComment
        IEnumerator executeSelectAllComment = selectAllComment();
        yield return StartCoroutine(executeSelectAllComment);
        AngerData[] allComment = (AngerData[])executeSelectAllComment.Current;

        // 新しいマークくんを追加.
        // insertMark
        StartCoroutine(insertMark(50, 50));             // 引数にはwidthとheightを渡す
    }

    /**
    * マークくん全取得.
    */
    IEnumerator selectAllMark() {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/server.php?method=selectAllMark");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ProtocolError) {
            // レスポンスコードを見て処理
            Debug.Log($"[Error] Failed to execute [selectAllMark]. Response Code : "+request.responseCode);
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError) {
            // エラーメッセージを見て処理
            Debug.Log($"[Error] Failed to execute [selectAllMark]. Message : "+request.error);
        }
        else{
            Debug.Log($"[Success] Succeeded to execute [selectAllMark]!!!");
            string downloadHandler = request.downloadHandler.text;

            // JSON配列 → C#配列への変換
            MarkData[] markData= JsonHelper.FromJson<MarkData>(downloadHandler);
            yield return markData;
        }
    }

    /**
    * 特定のマークくん情報取得.
    */
    IEnumerator selectMark(int id) {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/server.php?method=selectMark&id="+id);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ProtocolError) {
            // レスポンスコードを見て処理
            Debug.Log($"[Error] Failed to execute [selectMark] with id = "+id+". Response Code : "+request.responseCode);
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError) {
            // エラーメッセージを見て処理
            Debug.Log($"[Error] Failed to execute [selectMark] with id = "+id+". Message : "+request.error);
        }
        else{
            Debug.Log($"[Success] Succeeded to execute [selectMark] with id = "+id+"!!!");
            string downloadHandler = request.downloadHandler.text;

            MarkData markData = JsonUtility.FromJson<MarkData>(downloadHandler);
            yield return markData;
        }
    }

    /**
    * コメント全取得.
    */
    IEnumerator selectAllComment() {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/server.php?method=selectAllComment");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ProtocolError) {
            // レスポンスコードを見て処理
            Debug.Log($"[Error] Failed to execute [selectAllComment]. Response Code : "+request.responseCode);
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError) {
            // エラーメッセージを見て処理
            Debug.Log($"[Error] Failed to execute [selectAllComment]. Message : "+request.error);
        }
        else{
            Debug.Log($"[Success] Succeeded to execute [selectAllComment]!!!");
            string downloadHandler = request.downloadHandler.text;

            // JSON配列 → C#配列への変換
            AngerData[] angerData= JsonHelper.FromJson<AngerData>(downloadHandler);
            yield return angerData;
        }
    }

    /**
    * マークくん追加.
    */
    IEnumerator insertMark(int width, int height) {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/server.php?method=insertMark&width="+width+"&height="+height);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ProtocolError) {
            // レスポンスコードを見て処理
            Debug.Log($"[Error] Failed to execute [insertMark] with (width, height) = ("+width+", "+height+"). Response Code : "+request.responseCode);
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError) {
            // エラーメッセージを見て処理
            Debug.Log($"[Error] Failed to execute [insertMark] with (width, height) = ("+width+", "+height+"). Message : "+request.error);
        }
        else{
            Debug.Log($"[Success] Succeeded to execute [insertMark] with (width, height) = ("+width+", "+height+")!!!");

            yield return 0;
        }
    }

    /**
    * コメント追加.
    */
    IEnumerator insertComment(int mark_id, string comment, int anger_point) {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/server.php?method=insertComment&mark_id="+mark_id+"&comment="+comment+"&anger_point="+anger_point);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ProtocolError) {
            // レスポンスコードを見て処理
            Debug.Log($"[Error] Failed to execute [insertComment] with mark_id = "+mark_id+". Response Code : "+request.responseCode);
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError) {
            // エラーメッセージを見て処理
            Debug.Log($"[Error] Failed to execute [insertComment] with mark_id = "+mark_id+". Message : "+request.error);
        }
        else{
            Debug.Log($"[Success] Succeeded to execute [insertComment] with mark_id = "+mark_id+"!!!");

            yield return 0;
        }
    }

    /**
    * マークくん更新.
    */
    IEnumerator updateMark(int id, int width, int height, int total_anger_point, bool burst_flag) {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/server.php?method=updateMark&width="+width+"&height="+height+"&total_anger_point="+total_anger_point+"&burst_flag="+burst_flag);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ProtocolError) {
            // レスポンスコードを見て処理
            Debug.Log($"[Error] Failed to execute [updateMark] with id = "+id+". Response Code : "+request.responseCode);
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError) {
            // エラーメッセージを見て処理
            Debug.Log($"[Error] Failed to execute [updateMark] with id = "+id+". Message : "+request.error);
        }
        else{
            Debug.Log($"[Success] Succeeded to execute [updateMark] with id = "+id+"!!!");

            yield return 0;
        }
    }
}

[System.Serializable]
public class MarkData
{
    public string method;
    public int id;
    public DateTime create_at;
    public int width;
    public int height;
    public int anger_point;
    public bool burst_flag;
}

[Serializable]
public class MarkDataList{
    public MarkData[] markDataList;
}

[System.Serializable]
public class AngerData
{
    public string method;
    public int id;
    public DateTime create_at;
    public int mark_table_id;
    public string comment;
    public int anger_point;
}

/**
* JSON配列→C#配列への変換
*/
public static class JsonHelper
{
    // 指定したstringをRootオブジェクトを持たないJSON配列と仮定してデシリアライズ
    public static T[] FromJson<T>(string json)
    {
        // ルート要素があれば変換できるので
        // 入力されたJSONに対して(★)の行を追加する
        //
        // e.g.
        // ★ {
        // ★     "array":
        //        [
        //            ...
        //        ]
        // ★ }
        //
        string dummy_json = $"{{\"{DummyNode<T>.ROOT_NAME}\": {json}}}";

        // ダミーのルートにデシリアライズしてから中身の配列を返す
        var obj = JsonUtility.FromJson<DummyNode<T>>(dummy_json);
        return obj.array;
    }

    // // 指定した配列やリストなどのコレクションをRootオブジェクトを持たないJSON配列に変換
    // public static string ToJson<T>(IEnumerable<T> collection)
    // {
    //     string json = JsonUtility.ToJson(new DummyNode<T>(collection)); // ダミールートごとシリアル化する
    //     int start = DummyNode<T>.ROOT_NAME.Length + 4;
    //     int len = json.Length - start - 1;
    //     return json.Substring(start, len); // 追加ルートの文字を取り除いて返す
    // }

    // 内部で使用するダミーのルート要素
    [Serializable]
    private struct DummyNode<T>
    {
        // JSONに付与するダミールートの名称
        public const string ROOT_NAME = nameof(array);
        // 疑似的な子要素
        public T[] array;
        // コレクション要素を指定してオブジェクトを作成する
        public DummyNode(IEnumerable<T> collection) => this.array = collection.ToArray();
    }
}