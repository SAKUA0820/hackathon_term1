<?php

// 必要なもの
// ・brewよりphpをインストール&環境変数設定 (https://zenn.dev/ryuu/articles/setup-php-brew)
// ・brewよりmysqlをインストール&環境変数設定
// ・mysqlサーバ立ち上げ&ddlによりテーブル作成
// ・phpサーバ立ち上げ
// Talend API Testerなどを使ってjsonを送ってみる

// 作ったテーブルについて
// データベース名・ユーザ名・パスについては以下defineを参照
// テーブル定義は ../DB/ddl.sql を参照

define('DB_DATABASE', 'hackathon1');
define('DB_USERNAME', 'root');
// define('DB_PASSWORD', 'hackathon1');
define('DB_PASSWORD', 'nemui');  //←高橋自宅の場合
define('PDO_DSN', 'mysql:dbhost=localhost;dbname='.DB_DATABASE);

// ////
// // POST
// function CheckPostNumeric($key) {
//     if (is_numeric($_POST[$key])) {
//         return (int)$_POST[$key];
//     }
//     return 0;
// }

// function CheckPostString($key) {
//     if (is_string($_POST[$key])) {
//         return $_POST[$key];
//     }
//     return "";
// }

// function CheckPostJson($key) {
//     if (is_string($key)) {
//         return json_decode($_POST[$key]);
//     }
//     return null;
// }

// ////
// // JSON
// function CheckJsonNumeric($val) {
//     if (is_numeric($val)) {
//         return (int)$val;
//     }
//     return 0;
// }

// function CheckJsonString($str) {
//     if (is_string($str)) {
//         return $str;
//     }
//     return "";
// }
// ////
// class Test {
//     public function ToJson() {
//         $this->name = CheckJsonString($this->name);
//         $this->value = CheckJsonNumeric($this->value);
//         return json_encode($this);
//     }
// }
// $postTest = CheckPostJson('test');
// header('Content-type:application/json');

// json受け取り
$json = file_get_contents('php://input');

// レスポンスに送られてきたjson返す
header("Access-Control-Allow-Origin: *");
echo $json;

$params = json_decode($json, true);
print_r($params[0]['method']);

// DBアクセス 
    try {
        $db = new PDO(PDO_DSN, DB_USERNAME, DB_PASSWORD);
        $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

        // selectAllMark (送られてきたjsonのmethodの値がselectAllMarkだった場合)
        // TODO: 返すレスポンス返す形式合わせる
        if($params[0]['method'] == 'selectAllMark') {
            $stmt = $db->prepare("select * from mark_list");
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            var_dump($result);

            return;
        }

        // selectMark (送られてきたjsonのmethodの値がselectMarkだった場合)
        // TODO: 返すレスポンス返す形式合わせる
        if($params[0]['method'] == 'selectMark') {
            $stmt = $db->prepare("select * from mark_list where id = :id");
            $stmt->bindValue(':id', $params[0]['id'], PDO::PARAM_INT);
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            var_dump($result);

            return;
        }

        // selectAllComment (送られてきたjsonのmethodの値がselectAllCommentだった場合)
        // TODO: 返すレスポンス返す形式合わせる
        if($params[0]['method'] == 'selectAllComment') {
            $stmt = $db->prepare("select * from irritation_history");
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            var_dump($result);

            return;
        }

        // insertMark (送られてきたjsonのmethodの値がselectAllCommentだった場合)
        // TODO: 返すレスポンス返す形式合わせる
        if($params[0]['method'] == 'insertMark') {
            $stmt = $db->prepare("insert into mark_list (width, height) value (:width, :height)");
            $stmt->bindValue(':width', /*$postTest->width*/$params[0]['width'], PDO::PARAM_INT);
            $stmt->bindValue(':height', /*$postTest->height*/$params[0]['height'], PDO::PARAM_INT);
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            var_dump($result);

            return;
        }

        // insertComment (送られてきたjsonのmethodの値がinsertCommentだった場合)
        // TODO: 返すレスポンス返す形式合わせる
        if($params[0]['method'] == 'insertComment') {
            $stmt = $db->prepare("insert into irritation_history (mark_id, comment, anger_point) value (:mark_id, :comment, :anger_point)");
            $stmt->bindValue(':mark_id', $params[0]['mark_id'], PDO::PARAM_INT);
            $stmt->bindValue(':comment', $params[0]['comment'], PDO::PARAM_STR);
            $stmt->bindValue(':anger_point', $params[0]['anger_point'], PDO::PARAM_INT);
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            var_dump($result);

            return;
        }

        // updateMark (送られてきたjsonのmethodの値がupdateMarkだった場合)
        // TODO: 返すレスポンス返す形式合わせる
        if($params[0]['method'] == 'updateMark') {
            $stmt = $db->prepare("update mark_list set width = :width, height = :height, total_anger_point = :total_anger_point, burst_flag = :burst_flag where id = :id");
            $stmt->bindValue(':width', $params[0]['width'], PDO::PARAM_INT);
            $stmt->bindValue(':height', $params[0]['height'], PDO::PARAM_INT);
            $stmt->bindValue(':total_anger_point', $params[0]['total_anger_point'], PDO::PARAM_INT);
            $stmt->bindValue(':burst_flag', $params[0]['burst_flag'], PDO::PARAM_INT);
            $stmt->bindValue(':id', $params[0]['id'], PDO::PARAM_INT);
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            var_dump($result);

            return;
        }

        // insert or update (送られてきたjsonのmethodの値がsaveだった場合)
        // if($params[0]['method'] == 'save') {
        //     $stmt = $db->prepare("insert into test (name, height, width) value (:name, :height, :width) on duplicate key update height = values(height), width = values(width)");
        //     $stmt->bindValue(':name', /*$postTest->name*/$params[0]['name'], PDO::PARAM_STR);
        //     $stmt->bindValue(':height', /*$postTest->height*/$params[0]['height'], PDO::PARAM_INT);
        //     $stmt->bindValue(':width', /*$postTest->width*/$params[0]['width'], PDO::PARAM_INT);
        //     $stmt->execute();

        //     $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
        //     var_dump($result);

        //     return;
        // }

        $db = null;
    } catch (PDOException $e) {
    //  $db->rollback();
        echo "接続失敗\n";
        echo $e->getMessage();
        exit;
    }
?>