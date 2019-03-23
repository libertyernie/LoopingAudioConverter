Imports BrawlLib.LoopSelection
Imports MSFEncoderLib

Public Class Form1
    Private MSF As MSFFile

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using dialog As New OpenFileDialog
            dialog.Filter = "MSF files|*.msf"
            If dialog.ShowDialog(Me) = DialogResult.OK Then
                Dim data = IO.File.ReadAllBytes(dialog.FileName)
                MSF = New MSFFile(data)
                ListBox1.Items.Clear()
                Dim header = MSF.Header
                ListBox1.Items.Add($"tag: {header.tag}")
                ListBox1.Items.Add($"codec: {header.codec}")
                ListBox1.Items.Add($"channel_count: {header.channel_count}")
                ListBox1.Items.Add($"data_size: {header.data_size}")
                ListBox1.Items.Add($"sample_rate: {header.sample_rate}")
                ListBox1.Items.Add($"flags: {header.flags}")
                ListBox1.Items.Add($"loop_start: {header.loop_start}")
                ListBox1.Items.Add($"loop_end: {header.loop_end}")
                ListBox1.Items.Add("")
                ListBox1.Items.Add($"Total filesize: {data.Length}")
            End If
        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MSF IsNot Nothing Then
            Using dialog As New BrstmConverterDialog(MSF)
                dialog.ShowDialog(Me)
            End Using
        End If
    End Sub
End Class
