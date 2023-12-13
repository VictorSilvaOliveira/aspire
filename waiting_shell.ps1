While ((Get-Process $args[0] -ea SilentlyContinue) -eq $NULL)
{
    echo 'waiting'
    Start-Sleep -Seconds 1
}
echo 0