#import files
. .\Metric-Alert-Creation.ps1
. .\Helper-Functions.ps1

#Login to Azure
Login-AzureRMAccount

#Get AD Token
$token = getToken



#variables
$subscriptionId='49a5a4f7-705a-412c-b868-05750f228cdf'
$resourceGroupName = 'image2docker'
$apiVersion='api-version=2018-03-01'


#Create Metric Alert
$machineName ='Cloud-2016'
$alertName ='CPU Usage'
$actionGroupName = 'alertemailgrp'
$alertDescription='CPU Usage alert'
$operatorName='GreaterThanOrEqual'
$thresholdValue='7'
$actionResourceGroupName='rgalertscollection-ram'

CreateorUpdateAlert $token $subscriptionId $machineName $resourceGroupName $alertName $actionGroupName $apiVersion $alertDescription $operatorName $thresholdValue $actionResourceGroupName



