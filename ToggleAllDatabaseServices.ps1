$dbSvcNames = 'MySQL80', 'postgresql-x64-12', 'MSSQLSERVER', 'OracleServiceAKSHAYSDEMOD'
foreach($dbSvc in $dbSvcNames) {
    $getSvcInfo = Get-Service $dbSvc
    if($getSvcInfo.Status.ToString().Equals('Stopped')) {
        if($getSvcInfo.StartType.ToString().Equals('Disabled')) {
            $msg = $dbSvc + ' is disabled. Changing it to manual startup type.'
            echo $msg
            Set-Service $dbSvc -StartupType Manual
        }
        Start-Service $dbSvc
        $msg = $dbSvc + ' has been started'
        echo $msg
    } elseif($getSvcInfo.Status.ToString().Equals('Running')) {
        Stop-Service $dbSvc
        $msg = $dbSvc + ' has been stopped.'
        echo $msg
        if(!$getSvcInfo.StartType.ToString().Equals('Disabled')) {
            $msg = $dbSvc + ' is ' + $getSvcInfo.StartType.ToString() + '. Changing it to disabled startup type.'
            echo $msg
            Set-Service $dbSvc -StartupType Disabled
        }
    }
}
