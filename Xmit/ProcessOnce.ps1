$web = New-Object System.Net.WebClient
$str = $web.DownloadString("http://localhost:53280/api/Oracle/InitiateCrhDailyProcessing?limit=false&CompanyID=2")
$str