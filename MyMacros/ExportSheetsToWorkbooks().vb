Sub ExportSheetsToWorkbooks()
    Dim srcWb   As Workbook
    Dim ws      As Worksheet
    Dim newWb   As Workbook
    Dim destPath As String
    Dim safeName As String

    '----- 0) The workbook to split is simply the one you’re in ----------
    Set srcWb = Application.ActiveWorkbook
    If srcWb Is Nothing Then
        MsgBox "No active workbook."
        Exit Sub
    End If

    '----- 1) Ask where to save the files --------------------------------
    With Application.fileDialog(msoFileDialogFolderPicker)
        .Title = "Pick a folder for the split workbooks"
        If .Show <> -1 Then Exit Sub       ' user cancelled
        destPath = .SelectedItems(1) & Application.PathSeparator
    End With

    Application.ScreenUpdating = False

    '----- 2) Loop each visible sheet and save it ------------------------
    For Each ws In srcWb.Worksheets
        If ws.Visible = xlSheetVisible Then
            ws.Copy                                  ' puts copy in a new workbook
            Set newWb = ActiveWorkbook

            safeName = CleanFileName(ws.Name)
            newWb.SaveAs _
                Filename:=destPath & safeName & ".xlsx", _
                FileFormat:=xlOpenXMLWorkbook         ' .xlsx
            newWb.Close SaveChanges:=False
        End If
    Next ws

    Application.ScreenUpdating = True
    MsgBox "? Sheets exported to " & destPath
End Sub

'---- Helper: replace illegal characters and trim to 31 chars ----------
Function CleanFileName(s As String) As String
    Dim badChars As Variant, ch As Variant
    badChars = Array("\", "/", ":", "*", "?", """", "<", ">", "|")
    For Each ch In badChars
        s = Replace(s, ch, "_")
    Next ch
    CleanFileName = Left(s, 31)
End Function


