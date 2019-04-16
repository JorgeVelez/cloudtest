<?php
	$myfile = fopen("shareUrl.txt", "r") or die("Unable to open file!");
	
	$url = fread($myfile,filesize("shareUrl.txt"));
	fclose($myfile);
	
	exit(header("Location: " . $url));
?>