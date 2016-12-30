Imports MathLab.Scripting
Imports Microsoft.VisualBasic.Data.Bootstrapping

Public Class FormRunModelAssembly

    Dim model As Type
    Dim parameters As ParameterProxy

    Private Sub OpenToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem1.Click
        Using file As New OpenFileDialog With {
           .Filter = "Application Module(*.dll)|*.dll|.NET Application(*.exe)|*.exe"
       }
            If file.ShowDialog = DialogResult.OK Then
                If LoadModel(file.FileName) Then
                    '  Call config.models.AddFileHistory(file.FileName)
                    '  Call config.Save()

                    '  Call TextBox1.AppendText("Load dynamics model: " & file.FileName & vbCrLf)
                    Text = file.FileName & " - Run Dynamics Model"
                End If
            End If
        End Using
    End Sub

    Public Function LoadModel(path$) As Boolean
        Using loader As New FormLoadModel() With {
            .DllFile = path
        }
            If loader.ShowDialog = DialogResult.OK Then
                model = loader.Model
                Text = path.ToFileURL & "!" & model.FullName

                'For Each ctrl As Control In FlowLayoutPanel1.Controls
                '    If ctrl.Equals(ToolStrip1) Then
                '        Continue For
                '    End If
                '    Call FlowLayoutPanel1.Controls.Remove(ctrl)
                'Next
                'For Each ctrl As Control In FlowLayoutPanel2.Controls
                '    Call FlowLayoutPanel2.Controls.Remove(ctrl)
                'Next

                'For Each x In vars.Values
                '    Call Controls.Remove(x)
                '    Call x.Dispose()
                'Next

                Dim vars$() = MonteCarlo.Model.GetVariables(model).ToArray
                Dim proxy As ParameterProxy

                ScatterPlot1.SetVariables(vars)
                vars = vars.Join(MonteCarlo.Model.GetParameters(model)).ToArray
                proxy = ParameterProxy.Creates(vars)
                PropertyGrid1.SelectedObject = proxy
                parameters = proxy

                'For Each var$ In MonteCarlo.Model.GetVariables(model)
                '    Dim pic As New PictureBox With {
                '        .BackgroundImageLayout = ImageLayout.Zoom,
                '        .Size = New Size(200, 100)
                '    }
                '    vars(var) = pic
                '    FlowLayoutPanel2.Controls.Add(vars(var))

                '    AddHandler pic.Click, Sub(picBox, arg)
                '                              currentSelect = DirectCast(picBox, PictureBox)
                '                              PictureBox1.BackgroundImage = currentSelect.BackgroundImage
                '                          End Sub
                'Next

                'For Each var$ In MonteCarlo.Model.GetParameters(model) _
                '    .Join(MonteCarlo.Model.GetVariables(model))

                '    Dim lb As New Label With {
                '        .Text = var & ": "
                '    }
                '    Dim text As New TextBox With {
                '        .Name = var,
                '        .Tag = lb
                '    }

                '    Call FlowLayoutPanel1.Controls.Add(lb)
                '    Call FlowLayoutPanel1.Controls.Add(text)

                '    defines(var) = 0
                '    inputs(var) = text

                '    AddHandler text.TextChanged, Sub(txt, args)
                '                                     Dim txtBox = DirectCast(txt, TextBox)
                '                                     defines(txtBox.Name) = Val(txtBox.Text)
                '                                     DirectCast(txtBox.Tag, Label).Text = $"{txtBox.Name}:= {defines(txtBox.Name)}"
                '                                 End Sub
                'Next

                Return True
            End If
        End Using

        Return False
    End Function

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim defines As Dictionary(Of String, Double) = parameters.GetParameters
        Dim n = Val(tb_n.Text)
        Dim a = Val(tb_a.Text)
        Dim b = Val(tb_b.Text)

        ToolStripProgressBar1.Value = 0
        'TextBox1.AppendText($"a:={A}, b:={b}, n:={n}" & vbCrLf)
        'TextBox1.AppendText(defines.GetJson & vbCrLf)
        'Application.DoEvents()
        Dim out = MonteCarlo.Model.RunTest(model, defines, defines, n, a, b)
        Call ScatterPlot1.Plot(out)
    End Sub
End Class