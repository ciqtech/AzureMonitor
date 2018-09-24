
function PostAlertData($token,$subscriptionId,$resourceGroupName,$alertName,$apiVersion,$Body)
{

$TestURI= "https://management.azure.com/subscriptions/"+$subscriptionId+"/resourceGroups/"+$resourceGroupName+"/providers/Microsoft.Insights/metricAlerts/CPU%20Usage?"+$apiVersion

Write-Host 'test'
Write-Host $TestURI
$params = @{
    ContentType = 'application/json'
    Headers = @{ 'authorization'="Bearer $($Token.access_token)"}
    Method = 'Put'
    Body = $Body
    URI = $TestURI

    }

    Write-Host $Body
    $response = Invoke-RestMethod @params
return $response
}

function getToken()
{
$tennantid        = 'fd480176-a260-4395-9ac6-25ee7b89694c'
$SubscriptionId   = '49a5a4f7-705a-412c-b868-05750f228cdf'
$ApplicationID    = 'b7e5c17b-cef5-4de4-b4e6-8e56c7900af6'
$ApplicationKey   = 'yZ8TBy7mYfq2yly6GJZYgbfEE+FWVJmTROfOmNrNShw='
$TokenEndpoint = {https://login.windows.net/{0}/oauth2/token} -f $tennantid 
$ARMResource = "https://management.core.windows.net/";

$Body = @{
        'resource'= $ARMResource
        'client_id' = $ApplicationID
        'grant_type' = 'client_credentials'
        'client_secret' = $ApplicationKey
}

$params = @{
    ContentType = 'application/x-www-form-urlencoded'
    Headers = @{'accept'='application/json'}
    Body = $Body
    Method = 'Post'
    URI = $TokenEndpoint
}

$token = Invoke-RestMethod @params
return $token
}