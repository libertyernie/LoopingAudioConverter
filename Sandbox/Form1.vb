Imports BrawlLib.LoopSelection
Imports MSFContainerLib

Public Class Form1
    Private MSF As MSF

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using dialog As New OpenFileDialog
            dialog.Filter = "MSF files|*.msf"
            If dialog.ShowDialog(Me) = DialogResult.OK Then
                Dim data = IO.File.ReadAllBytes(dialog.FileName)
                MSF = MSF.Parse(data)
                If Not data.SequenceEqual(MSF.Export()) Then
                    MsgBox("nope")
                End If
                ListBox1.Items.Clear()
                Dim header = MSF.Header
                ListBox1.Items.Add($"tag: {header.tag}")
                ListBox1.Items.Add($"codec: {header.codec}")
                ListBox1.Items.Add($"channel_count: {header.channel_count}")
                ListBox1.Items.Add($"data_size: {header.data_size}")
                ListBox1.Items.Add($"sample_rate: {header.sample_rate}")
                ListBox1.Items.Add($"flags: {header.flags.Flags}")
                ListBox1.Items.Add($"loop_start: {header.loop_start}")
                ListBox1.Items.Add($"loop_length: {header.loop_length}")
                ListBox1.Items.Add("")
                ListBox1.Items.Add($"Total filesize: {data.Length}")
            End If
        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MSF IsNot Nothing Then
            Dim newMsf = MSF.FromAudioSource(MSF)
            Using dialog2 As New SaveFileDialog
                dialog2.Filter = "MSF files|*.msf"
                If dialog2.ShowDialog(Me) = DialogResult.OK Then
                    IO.File.WriteAllBytes(dialog2.FileName, newMsf.Export())
                End If
            End Using
        End If
    End Sub
End Class
