<?php
$val = json_decode($HTTP_RAW_POST_DATA, true);

$url = 'https://build-api.cloud.unity3d.com/api/v1/orgs/coutlass-supreme/projects/cloud2/buildtargets/default-linux-desktop-universal/builds/'. $val["buildNumber"] .'/share';

$result=requestCloudApi($url, 'POST'); 

$val = json_decode($result, true);

$url='https://build-api.cloud.unity3d.com/api/v1/shares/'.$val["shareid"];

$result=requestCloudApi($url, 'GET'); 

$val = json_decode($result, true);
//$val = json_encode($result);

file_put_contents('shareUrl.txt', $val["links"]["download_primary"]["href"]);


function requestCloudApi($url, $method) {
    $requestHeaders = array(
    'Content-type: application/x-www-form-urlencoded',
    'Authorization: Basic 40fc582f4eaab80de49f109ceee9a6d9',
    'Content-Length: 0'
    );  
    
    $context = stream_context_create(
                array(
                    'http' => array(
                        'method'  => $method,
                        'header'  => implode("\r\n", $requestHeaders),
                        'content' => '',
                    )
                )
            );

    $result = file_get_contents($url, false, $context);

    if ($result === FALSE) {
        return null;
    }else{
       return $result; 
    } 
}


?>