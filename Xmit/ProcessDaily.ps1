$web = New-Object System.Net.WebClient
$str = $web.DownloadString("http://localhost:53280/api/Oracle/InitiateCrhDailyProcessing?limit=true&CompanyID=2")
$str