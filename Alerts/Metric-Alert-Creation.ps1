function CreateorUpdateAlert($token,$subscriptionId,$machineName,$resourceGroupName,$alertName,$actionGroupName,$apiVersion,$alertDescription,$operatorName,$thresholdValue,$actionResourceGroupName)
{

$Body= "{
    'location': 'global',
    'type': 'Microsoft.Insights/metricAlerts',
    'name': 'CPU Usage',
    'id': '/subscriptions/"+$subscriptionId+"/resourceGroups/"+$resourceGroupName+"/providers/microsoft.insights/metricAlerts/CPU%20Usage',
    'tags': {},
    'properties': {
        'description': '"+$alertDescription+"',
        'severity': 3,
        'enabled': true,
        'scopes': [
            '/subscriptions/"+$subscriptionId+"/resourceGroups/"+$resourceGroupName+"/providers/Microsoft.Compute/virtualMachines/"+$machineName+"'],
        'evaluationFrequency': 'PT5M',
        'windowSize': 'PT5M',
        'templateType': 8,
        'templateSpecificParameters': {},
        'criteria': {
            'allOf': [
                {
                    'name': 'Metric1',
                    'metricNamespace': 'Microsoft.Compute/virtualMachines',
                    'metricName': 'Percentage CPU',
                    'dimensions': [],
                    'operator': '"+$operatorName+"',
                    'threshold': "+$thresholdValue+",
                    'timeAggregation': 'Average',
                    'monitorTemplateType': 8
                }
            ],
            'odata.type': 'Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria'
        },
        'actions': [
            {
                'actionGroupId': '/subscriptions/"+$subscriptionId+"/resourcegroups/"+$actionResourceGroupName+"/providers/microsoft.insights/actiongroups/"+$actionGroupName+"',
                'webHookProperties': {}
            }
        ]
    }
}"

    $response = PostAlertData $token  $subscriptionId  $resourceGroupName  $alertName  $apiVersion  $Body
    return $response
}

