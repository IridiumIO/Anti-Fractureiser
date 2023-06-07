Imports System.Diagnostics
Imports Microsoft.Win32

Class MainWindow


    Dim AppDataPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
    Dim edgePath As String = AppDataPath & "\Local\Microsoft Edge"

    Dim badPaths As String() = {
    edgePath & "\.ref",
    edgePath & "\client.jar",
    edgePath & "\lib.dll",
    edgePath & "\libWebGL64.jar",
    edgePath & "\run.bat",
    AppDataPath & "\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\run.bat"
}

    Function CheckForAppDataBadActors() As Boolean

        Dim res As Boolean = False
        Dim successfullyremoved = True
        For Each path In badPaths
            ConPrint("Checking " & path)
            If IO.File.Exists(path) Then
                ConPrint("Malicious file found! Attempting to remove " & path)
                res = True

                Try
                    IO.File.Delete(path)
                Catch ex As UnauthorizedAccessException
                    successfullyremoved = False
                    ConPrint($"UnauthorizedAccessException: Error deleting the file {path}.{vbCrLf}It is possible that the file is currently being used.")
                End Try

            End If
        Next

        If CheckForRegistryKey() Then res = True

        ConPrint(vbCrLf)
        If Not res Then
            ConPrint("Infected files were not detected. Please make sure you still run the detector linked in Stage 0.")
        Else
            If successfullyremoved Then
                ConPrint("Infected files were detected and successfully removed. Please make sure you still run the detector linked in Stage 0.")
            Else
                ConPrint("Infected files were detected and COULD NOT BE REMOVED. For now you will have to follow the link in the right sidebar to find additional steps.")

            End If

        End If
        Return res

    End Function

    Function CheckForRegistryKey() As Boolean

        Dim keyPath As String = "Software\Microsoft\Windows\CurrentVersion\Run"
        Dim valueName As String = "t"

        If Registry.CurrentUser.OpenSubKey(keyPath) IsNot Nothing AndAlso Registry.CurrentUser.OpenSubKey(keyPath).GetValue(valueName) IsNot Nothing Then
            ConPrint("Malicious registry entry found. Deleting..")
            Registry.CurrentUser.OpenSubKey(keyPath, True).DeleteValue(valueName, False)
            ConPrint("Deleted registry entry")
            Return True
        End If

    End Function


    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ConPrint(vbCrLf & "Checking...")
        If CheckForAppDataBadActors() Then
            MsgBox("Infected files were detected. Read the output console to see if they were deleted successfully", , "Anti-Fractureiser")
        Else
            MsgBox("Infected files were not detected. Please make sure you still run the detector linked in Stage 0.", , "Anti-Fractureiser")
        End If
    End Sub

    Private Sub Hyperlink_RequestNavigate(sender As Object, e As RequestNavigateEventArgs)
        Dim url As String = e.Uri.ToString()
        Process.Start(New ProcessStartInfo() With {.FileName = url, .UseShellExecute = True})
        e.Handled = True
    End Sub










    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        ' Create the netsh command to add a firewall rule
        ConPrint(vbCrLf & "Creating Firewall rules...")
        Dim command As String = $"advfirewall firewall add rule name=""Block Fractureiser Malware IP"" dir=in action=block remoteip=85.217.144.130"
        Dim command2 As String = $"advfirewall firewall add rule name=""Block Fractureiser Malware IP"" dir=out action=block remoteip=85.217.144.130"

        ' Execute the netsh command
        ExecuteCommand(command)
        ExecuteCommand(command2)
        ConPrint("Firewall Rules Created")

        ConPrint("Adding Hosts Rule...")
        If AddHostEntry("files-8ie.pages.dev", "0.0.0.0") Then
            ConPrint("Hosts rule created")
        Else
            ConPrint("Failed to write Hosts rule. Are you running as Administrator?")
        End If
    End Sub



    Private Sub ExecuteCommand(command As String)

        Dim processInfo As New ProcessStartInfo()
        processInfo.FileName = "netsh"
        processInfo.Arguments = command
        processInfo.CreateNoWindow = True
        processInfo.UseShellExecute = True
        processInfo.Verb = "runas"

        Dim process As Process = Process.Start(processInfo)
        process.WaitForExit()
    End Sub


    Public Function AddHostEntry(hostname As String, ipAddress As String) As Boolean
        Try
            ' Specify the path to the hosts file
            Dim hostsFilePath As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers\etc\hosts")

            ' Check if the hosts file exists
            If IO.File.Exists(hostsFilePath) Then
                ' Open the hosts file in append mode
                Using writer As New IO.StreamWriter(hostsFilePath, True)
                    ' Write the new host entry to the file
                    writer.WriteLine(ipAddress & " " & hostname)
                End Using

                Console.WriteLine("Host entry added successfully.")
            Else
                Console.WriteLine("Hosts file not found.")
            End If
            Return True
        Catch ex As Exception
            ' Handle any exceptions that occur during the process
            MsgBox("Error adding host entry: " & ex.Message.ToString & " Please ensure you are running this program as Administrator.")
            Return False
        End Try
    End Function


    Private Function ConPrint(str As String)

        OutCon.AppendText(vbCrLf)
        OutCon.AppendText(str)
        OutCon.ScrollToEnd()
    End Function

End Class
