Imports System.Data.SqlClient
Public Class Form1
    Dim conn As New SqlConnection
    Dim z As Integer
    Dim log As Integer = 0
    Dim str As String
    Dim ds As New DataSet
    Dim da As SqlDataAdapter
       Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        'TODO: This line of code loads data into the 'HarkousMarketDataSet.EMPLOYEE' table. You can move, or remove it, as needed.
        Me.EMPLOYEETableAdapter1.Fill(Me.HarkousMarketDataSet.EMPLOYEE)
        
        conn = New SqlClient.SqlConnection
        conn.ConnectionString = "Data Source=LEB-PC;Initial Catalog=HarkousMarket;Integrated Security=True"
        conn.Open()
        
        ' MsgBox("Connection to supermarket database is successful GOODLUCK!!")
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim dr As SqlDataReader
        Dim cmd As New SqlCommand("select eid,ename from employee", conn)
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        While dr.Read()
            sopempcobo.Items.Add(dr("eid").ToString.Trim & "   " & dr("ename"))
        End While
        dr.Close()
        conn.Close()
    End Sub

    'print records from the customers table in the big txtbox
    Private Sub viewcust_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewcust.Click      

        custListBox.Items.Clear()
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim dr As SqlDataReader
            Dim cmd As New SqlCommand()
            With cmd
                .CommandText = "select cid,cname,ctel,caddress from customers"
                .Connection = conn
            End With
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim str As String
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While dr.Read
                str = dr("cid") & "    " & dr("cname") & "            " & dr("ctel") & "                           " & dr("caddress").ToString.Trim
                custListBox.Items.Add(str)
            End While
            dr.Close()
            conn.Close()
        End If
    End Sub
    'add account
    Private Sub addacc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addacc.Click
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If accid.Text = "" Or acccidcombo.SelectedItem = "" Or accurrcombo.SelectedItem = "" Or accbal.Text = "" Then
                MsgBox("Enter the informations correctly")
            Else
                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                ds = New DataSet
                Dim sql As String
                sql = "select * from account"
                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                da = New SqlClient.SqlDataAdapter(sql, conn)
                da.Fill(ds, "account")
                Dim nrows, x, y As Integer
                nrows = ds.Tables("account").Rows.Count
                For i = 0 To nrows - 1
                    If accid.Text = ds.Tables("account").Rows(i).Item("accid").ToString.Trim Then
                        x = 1
                    End If
                Next
                If x = 1 Then
                    MsgBox("THis ID exists!! Please enter another ACCOUNT ID..")
                Else




                    'fill("account")
                    Dim str1, str2, str3, sql2 As String
                    Dim val As Double
                    str1 = accid.Text

                    str2 = accurrcombo.SelectedItem
                    str3 = acccidcombo.SelectedItem
                    val = CDbl(accbal.Text)

                    Dim da3 As New SqlDataAdapter
                    sql2 = "insert into account values('" & str1 & "','" & str2 & "','" & str3 & "','" & val & "')"

                    Dim cmd As New SqlCommand(sql2, conn)
                    If ConnectionState.Closed Then
                        conn.Open()
                    End If
                    cmd.ExecuteNonQuery()
                    If ConnectionState.Closed Then
                        conn.Open()
                    End If
                    da.Update(ds, "account")
                    MsgBox("this NEW ACCOUNT was added successfully")
                End If
            End If
        End If
        conn.Close()
    End Sub
    'view account
    Private Sub viewacc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewacc.Click 
        acclistbox.Items.Clear()
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            Dim dr As SqlDataReader
            Dim cmd As New SqlCommand()
            With cmd
                .CommandText = "select accid,currtype,cid,accbalance from account"
                .Connection = conn
            End With
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim str As String
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While dr.Read
                str = dr("accid") & "                  " & dr("cid") & "                       " & dr("currtype") & "                           " & dr("accbalance")
                acclistbox.Items.Add(str)
            End While
            dr.Close()
        End If
        conn.Close()

    End Sub
    'ADD ITEM
    Private Sub additem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles additem.Click
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If iid.Text = "" Or itype.Text = "" Or iname.Text = "" Or ipricetxt.Text = "" Or iqaunttxt.Text = "" Then
                MsgBox("Check that you enter all the fields of ITEM information correctly!!")
            Else
                Dim cb As New SqlClient.SqlCommandBuilder(da)
                ds = New DataSet
                Dim sql As String = "select * from items"

                da = New SqlClient.SqlDataAdapter(sql, conn)
                da.Fill(ds, "items")

                Dim str1, str2, str3, str4, sql2 As String
                Dim a As Double
                str1 = iid.Text
                str2 = iname.Text
                str3 = itype.Text
                str4 = iqaunttxt.Text
                a = CDbl(ipricetxt.Text)
                sql2 = "insert into items values('" & str1 & "','" & str2 & "','" & str3 & "','" & a & "','" & str4 & "')"
                Dim cmd As New SqlCommand(sql2, conn)
                cmd.ExecuteNonQuery()
                da.Update(ds, "items")
                MsgBox("This item was added successfully")
            End If
        End If
        conn.Close()
    End Sub
    ' view item
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        itemListBox.Items.Clear()      
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim dr As SqlDataReader
        Dim cmd As New SqlCommand()
        With cmd
            .CommandText = "select itid,itname,ittype,iprice,iqauntity from items"
            .Connection = conn
        End With
        Dim str As String
       
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        While dr.Read
            str = dr("itid") & "      " & dr("itname") & "     " & dr("ittype") & "      " & dr("iqauntity") & "                    " & dr("iprice")
            itemListBox.Items.Add(str)
        End While
        dr.Close()
        conn.Close()
    End Sub
    'Deposition of amountdep into the balance of an account 
    Private Sub makedep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles makedep.Click
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If depid.Text = "" Or depcid.Text = "" Or depamnt.Text = "" Or accidComboBox1.Text = "" Then
                MsgBox("Please enter all the fields correctly!!")
            Else
                Dim x As Integer = 0
                Dim str5 As String
                Dim dr3 As SqlDataReader
                str5 = depcid.Text
                Dim cmd3 As New SqlCommand("select * from customers", conn)
                dr3 = cmd3.ExecuteReader()
                While dr3.Read()
                    If dr3.Item("cid").ToString.Trim = str5.ToString.Trim Then
                        x = 1
                    End If
                End While
                dr3.Close()

                If x <> 1 Then
                    MsgBox("A customer with this ID doesnot exist")
                Else
                    Dim cb As New SqlClient.SqlCommandBuilder(da)
                    Dim sql As String = "select * from deposite"
                    ds = New DataSet
                    da = New SqlClient.SqlDataAdapter(sql, conn)
                    da.Fill(ds, "deposite")
                    Dim str1, sql2, str2, d, str3 As String
                    Dim val As Double
                    d = System.DateTime.Today.ToString
                    str1 = depid.Text
                    str2 = depcid.Text
                    str3 = accidComboBox1.SelectedItem
                    val = CDbl(depamnt.Text)
                    sql2 = "insert into deposite values ('" & str1 & "','" & str2 & "','" & str3 & "'," & val & ",'" & d & "')"
                    Dim cmd As New SqlCommand(sql2, conn)

                    cmd.ExecuteNonQuery()
                    da.Update(ds, "deposite")

                    Dim da2 As New SqlDataAdapter
                    val = CDbl(depamnt.Text)
                    da2 = New SqlDataAdapter("select * from account", conn)
                    da2.Fill(ds, "account")
                    Dim sql3 As String = "begin tran update account set accbalance = accbalance + " & val & " where accid='" & accidComboBox1.SelectedItem & "' commit tran"

                    Dim cmd2 As New SqlCommand(sql3, conn)
                    cmd2.ExecuteNonQuery()
                    da.Update(ds, "account")
                    MsgBox("deposition of " & val & " to " & str3.Trim & " was made suc made sucssefully ")
                End If
            End If
        End If
        conn.Close()
    End Sub
    Private Sub viewemp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewemp.Click 
        empListBox.Items.Clear()
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            Dim dr As SqlDataReader
            Dim cmd As New SqlCommand()
            With cmd
                .CommandText = "select eid,ename,etel,eaddress from employee"
                .Connection = conn
            End With
            Dim str As String
            dr = cmd.ExecuteReader()
            While dr.Read()
                str = dr("eid") & "      " & dr("ename") & "          " & dr("etel") & "              " & dr("eaddress")
                empListBox.Items.Add(str)
            End While

            dr.Close()
        End If
        conn.Close()
    End Sub

    'Private Sub choscurr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    sopcurrcombo.Items.Clear()
    '    If conn.State = ConnectionState.Closed Then
    '        conn.Open()
    '    End If
    '    If sopempcobo.SelectedItem = "" Or sopempcobo.SelectedItem = "" Then
    '        MsgBox("Please LOGIN first!!")
    '    Else
    '        Dim dr As SqlDataReader
    '        Dim cmd As New SqlCommand("select currtype from account where account.cid='" & Me.sopcidtxt.Text & "'", conn)
    '        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '        While dr.Read()
    '            sopcurrcombo.Items.Add(dr("currtype"))
    '        End While
    '        dr.Close()
    '    End If
    '    conn.Close()
    'End Sub

    '    Private Sub chosemp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chosemp.Click
    '       sopempcobo.Items.Clear()
    '      If conn.State = ConnectionState.Closed Then
    '         conn.Open()
    '    End If
    '
    '   Dim dr As SqlDataReader
    '  Dim cmd As New SqlCommand("select eid,ename from employee", conn)
    '     dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '    While dr.Read()
    '            sopempcobo.Items.Add(dr("eid").ToString.Trim & "   " & dr("ename"))
    '       End While
    '      dr.Close()
    '     conn.Close()
    'End Sub



    'click to add items
    Private Sub makeinvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles makeinvoice.Click

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If sopidtxt.Text = "" Then
                MsgBox("Enter an id for this sale operation")
            Else

                If RadioButton1.Checked = False AndAlso RadioButton2.Checked = False Then
                    MsgBox("Choose the payment way first")
                Else

                    ds = New DataSet
                    da = New SqlDataAdapter("select * from saleoperation", conn)
                    da.Fill(ds, "saleoperation")
                    Dim nrows, i, x As Integer
                    nrows = ds.Tables("saleoperation").Rows.Count
                    For i = 0 To nrows - 1
                        If sopidtxt.Text = ds.Tables("saleoperation").Rows(i).Item("sopid").ToString.Trim Then
                            x = 1
                            Exit For
                        End If
                    Next
                    If x = 1 Then
                        MsgBox("The ID chosen for this SaleOperaion exists!!Please choose any other random ID!!")
                    Else
                        Dim str, str2, str3, str4, str5, str6, str7, sql As String
                        str = sopidtxt.Text
                        str2 = posidcombo.Text
                        ' str3 = sopcurrcombo.SelectedItem
                        str4 = sopempcobo.SelectedItem.ToString.Substring(0, 2)
                        str5 = sopaccCombo.SelectedItem
                        '  str6 = sopcidtxt.Text



                        If RadioButton1.Checked Then

                            If str5 = "" Then
                                MsgBox("Check that you enter the ID of ACCOUNT!!")
                            Else
                                Dim dr3 As SqlDataReader
                                str5 = sopaccCombo.SelectedItem
                                Dim cmd3 As New SqlCommand("select * from account", conn)
                                dr3 = cmd3.ExecuteReader()
                                While dr3.Read()
                                    If dr3.Item("accid").ToString.Trim = str5.ToString.Trim Then
                                        str7 = dr3.Item("cid")
                                        Exit While
                                    End If
                                End While
                                dr3.Close()


                                Dim dr As SqlDataReader
                                Dim cmd As New SqlCommand("select currtype from account where cid='" & str7 & "'", conn)
                                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                                While dr.Read()
                                    str3 = dr("currtype")
                                End While
                                dr.Close()



                                sql = "insert into saleoperation values('" & str & "','" & str2 & "','" & str3 & "','" & str4 & "','" & str5 & "','" & str7 & "','" & System.DateTime.Today.ToString & "','" & 0 & "','" & 0 & "')"
                                Dim cmd2 As SqlCommand
                                cmd2 = New SqlCommand(sql, conn)
                                If conn.State = ConnectionState.Closed Then
                                    conn.Open()
                                End If
                                cmd2.ExecuteNonQuery()
                                da.Update(ds, "saleoperation")
                                da.Fill(ds, "saleoperation")
                                MsgBox("Add the BARCODE of each item and click buton ADD each time...")
                                makeinvoice.Enabled = False
                                conn.Close()
                                z = 1
                            End If
                        Else
                            If conn.State = ConnectionState.Closed Then
                                conn.Open()
                            End If
                            sql = "insert into saleoperation values('" & str & "','" & str2 & "','L.L.','" & str4 & "','X','X','" & System.DateTime.Today.ToString & "','" & 0 & "','" & 0 & "')"
                            If ConnectionState.Closed Then
                                conn.Open()
                            End If
                            Try

                            
                            ds = New DataSet
                            da = New SqlDataAdapter("select * from saleoperation", conn)
                            da.Fill(ds, "saleoperation")
                            Dim cmd4 As New SqlCommand(sql, conn)
                            If ConnectionState.Closed Then
                                conn.Open()
                            End If
                            cmd4.ExecuteNonQuery()
                            da.Update(ds, "saleoperation")
                            da.Fill(ds, "saleoperation")
                            MsgBox("Add the BARCODE of each item and click add each time")
                            makeinvoice.Enabled = False

                                z = 1
                            Catch ex As Exception

                            End Try
                        End If
                    End If
                End If
            End If
        End If
        conn.Close()
    End Sub

    Private Sub addqauntitems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addqauntitems.Click
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If sopidtxt.Text = "" Then
                MsgBox("Enter an id for this sale operation")
            Else
                If RadioButton1.Checked = False AndAlso RadioButton2.Checked = False Then
                    MsgBox("Choose the payment way first")
                Else
                    Dim sql2 As String
                    Dim cmd3 As New SqlCommand
                    Dim pr As Double
                    Dim n As Integer
                    ds = New DataSet
                    Dim da3 As New SqlDataAdapter("select * from items", conn)
                    da3.Fill(ds, "items")
                    n = ds.Tables("items").Rows.Count
                    Dim sql3 As String
                    str = sopidtxt.Text
                    Dim da2 As SqlDataAdapter
                    da2 = New SqlDataAdapter("select * from saleditemslist", conn)
                    da2.Fill(ds, "saleditems")
                    If sopitemtxt.Text = "" Or qauntitemstxt.Text = "" Then
                        MsgBox("Please enter the items !!!")
                    Else

                        sql3 = "begin tran update items set items.iqauntity = items.iqauntity -" & CDbl(qauntitemstxt.Text) & " where items.itid= '" & sopitemtxt.Text & "' commit tran"
                        Dim cmd4 As New SqlCommand(sql3, conn)
                        cmd4.ExecuteNonQuery()
                        da3.Update(ds, "items")
                        For i = 0 To n - 1
                            If Me.sopitemtxt.Text.ToString = ds.Tables("items").Rows(i).Item("itid").ToString.Substring(0, 2) Then
                                pr = ds.Tables("items").Rows(i).Item("iprice")
                                'ds.Tables("items").Rows(i).Item("iqauntity") -= CDbl(qauntitemstxt.Text)


                            End If
                        Next
                        sql2 = "insert into saleditemslist values('" & str & "','" & sopitemtxt.Text & "'," & CDbl(Me.qauntitemstxt.Text) & "," & pr & ")"
                        cmd3 = New SqlCommand(sql2, conn)
                        If ConnectionState.Closed Then
                            conn.Open()
                        End If

                        cmd3.ExecuteNonQuery()
                        da2.Update(ds, "saleditems")

                    End If
                    makeinvoice.Enabled = True


                    qauntitemstxt.Clear()
                    sopitemtxt.Clear()
                End If
            End If
        End If

        conn.Close()
    End Sub



    'Private Sub sopchosecid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    sopcidtxt.Clear()
    '    If conn.State = ConnectionState.Closed Then
    '        conn.Open()
    '    End If
    '    If sopempcobo.SelectedItem = "" Or sopempcobo.SelectedItem = "" Then
    '        MsgBox("Please LOGIN first!!")
    '    Else
    '        If sopaccCombo.SelectedItem = "" Then
    '            MsgBox("Choose the ACCOUNT ID first the obtain CUSTOMER ID!!")
    '        Else
    '            Dim dr3 As SqlDataReader
    '            Dim str5 As String = sopaccCombo.SelectedItem
    '            Dim cmd As New SqlCommand("select * from account", conn)
    '            dr3 = cmd.ExecuteReader()
    '            While dr3.Read()
    '                If dr3.Item("accid").ToString.Trim = str5.ToString.Trim Then
    '                    sopcidtxt.Text = dr3.Item("cid")
    '                End If
    '            End While
    '            dr3.Close()
    '        End If
    '    End If
    '    conn.Close()
    'End Sub
    'fatooora
    Private Sub viewbill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewbill.Click
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If sopidtxt.Text = "" Then

                MsgBox("please enter a valid id for this sale operaion!")
            Else
                If RadioButton1.Checked = False AndAlso RadioButton2.Checked = False Then
                    MsgBox("Please choose the PAYMENT WAY first")
                Else
                    If z <> 1 Then
                        MsgBox("Please Add items first!!! check fields")
                    Else
                        If conn.State = ConnectionState.Closed Then
                            conn.Open()
                        End If
                        ds = New DataSet
                        Dim da As SqlDataAdapter
                        da = New SqlDataAdapter("select * from saleditemslist", conn)
                        da.Fill(ds, "saleditemslist")
                        Dim n, i As Integer
                        Dim total As Double
                        total = 0
                        n = ds.Tables("saleditemslist").Rows.Count
                        For i = 0 To n - 1
                            If sopidtxt.Text.Trim = ds.Tables("saleditemslist").Rows(i).Item("sopid").Trim Then
                                total += (ds.Tables("saleditemslist").Rows(i).Item("saledprice") * ds.Tables("saleditemslist").Rows(i).Item("saledquantity"))
                            End If
                        Next
                        Dim da2 As New SqlDataAdapter("select * from saleoperation", conn)
                        da2.Fill(ds, "saleoperation")
                        Dim sql As String
                        If RadioButton1.Checked Then
                            sql = "begin tran update saleoperation set amountacc = amountacc + " & CDbl(total) & " where saleoperation.sopid='" & sopidtxt.Text & "' commit tran"
                        Else
                            sql = "begin tran update saleoperation set amountcash = amountcash + " & CDbl(total) & " where saleoperation.sopid='" & sopidtxt.Text & "' commit tran"
                        End If

                        Dim cmd2 As New SqlCommand(sql, conn)
                        cmd2.ExecuteNonQuery()
                        da2.Update(ds, "saleoperation")
                        MsgBox(" total amount=" & total)
                        'view items bel fetoora  
                        Dim sql3 As String
                        If RadioButton1.Checked Then
                            sql3 = "select * from account where accid = '" & Me.sopaccCombo.SelectedItem.ToString.Trim & "'"
                        Else
                            sql3 = "select * from account where accid = 'X'"
                        End If
                        If RadioButton1.Checked Then
                            Dim da4 As New SqlDataAdapter(sql3, conn)
                            da4.Fill(ds, "account")



                            ''''''''''''

                            Dim str7 As String
                            Dim dr6 As SqlDataReader
                            Dim acc As String = sopaccCombo.SelectedItem
                            Dim cmd6 As New SqlCommand("select * from account", conn)
                            dr6 = cmd6.ExecuteReader()
                            While dr6.Read()
                                If dr6.Item("accid").ToString.Trim = acc.ToString.Trim Then
                                    str7 = dr6.Item("cid").ToString.Trim
                                End If
                            End While
                            dr6.Close()



                            Dim curr As String
                            Dim dr2 As SqlDataReader
                            Dim cmd8 As New SqlCommand("select currtype from account where account.cid='" & str7 & "'", conn)
                            dr2 = cmd8.ExecuteReader(CommandBehavior.CloseConnection)

                            While dr2.Read()
                                curr = dr2("currtype").ToString.Trim
                            End While

                            If curr = "$" Then
                                total = (total * 1.0) / 1500
                            End If
                            If curr = "EURO" Then
                                total = (total * 1.0) / 2200
                            End If
                            dr2.Close()

                            Dim sql4 As String = "begin tran update account set account.accbalance = account.accbalance - " & total & " where account.accid='" & Me.sopaccCombo.SelectedItem.ToString.Substring(0, 2) & "' commit tran"
                            Dim cmd4 As New SqlCommand(sql4, conn)
                            If conn.State = ConnectionState.Closed Then
                                conn.Open()
                            End If
                            cmd4.ExecuteNonQuery()
                            da4.Update(ds, "account")
                        End If

                        Dim sql2 As String = "select itname,saledquantity from items,saleditemslist where items.itid=saleditemslist.itid and saleditemslist.SOPID='" & sopidtxt.Text & "'"
                        Dim da3 As New SqlDataAdapter(sql2, conn)
                        da3.Fill(ds, "itemNQ")

                        Dim nrows As Integer
                        nrows = ds.Tables("itemNQ").Rows.Count
                        str = ""
                        For i = 0 To nrows - 1
                            str = str & ds.Tables("itemNQ").Rows(i).Item("itname").ToString.Trim & "(" & ds.Tables("itemNQ").Rows(i).Item("saledquantity").ToString.Trim & ")" & "--"
                        Next

                        'Dim dr As SqlDataReader
                        'Dim cmd As New SqlCommand(sql2, conn)
                        '    Dim str As String = ""
                        '    Dim a As Integer = 0
                        '    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        'While dr.Read()
                        '        a = a + 1
                        '    End While
                        '    Dim array(a) As Integer
                        '    i = 0
                        '    While dr.Read()
                        '        array(i) = dr.Item("saledquantity")
                        '        i = i + 1
                        '    End While
                        '    i = 0
                        '    While dr.Read()
                        '        str = str & dr.Item("itname") & "(" & array(i) & ")" & "-"
                        '        i = i + 1
                        '    End While
                        '    dr.Close() 
                        Dim cur As String
                        Dim ccid As String
                        If RadioButton1.Checked Then

                            Dim str2 As String
                            Dim dr3 As SqlDataReader
                            Dim str5 As String = sopaccCombo.SelectedItem
                            Dim cmd As New SqlCommand("select * from account", conn)
                            dr3 = cmd.ExecuteReader()
                            While dr3.Read()
                                If dr3.Item("accid").ToString.Trim = str5.ToString.Trim Then
                                    str2 = dr3.Item("cid").ToString.Trim
                                    ccid = str2
                                End If
                            End While
                            dr3.Close()




                            Dim dr As SqlDataReader
                            Dim cmd7 As New SqlCommand("select currtype from account where account.cid='" & str2 & "'", conn)
                            dr = cmd7.ExecuteReader(CommandBehavior.CloseConnection)
                            Dim currtype As String
                            While dr.Read()
                                currtype = dr("currtype").ToString.Trim
                            End While

                            If RadioButton2.Checked Then
                                cur = "L.L."
                            Else
                                cur = currtype
                            End If
                            dr.Close()
                        End If
                        Dim sqlp As String = "select * from pointofsale"
                        Dim dap As New SqlDataAdapter(sqlp, conn)

                        dap.Fill(ds, "pos")
                        Dim sql5 As String = "begin tran update pointofsale set pointofsale.totalbalance = pointofsale.totalbalance + " & total & " where pointofsale.posno=" & "'" & posidcombo.SelectedItem.ToString & "' commit tran"
                        Dim cmd5 As New SqlCommand(sql5, conn)
                        If conn.State = ConnectionState.Closed Then
                            conn.Open()
                        End If

                        cmd5.ExecuteNonQuery()
                        dap.Update(ds, "pos")

                        If RadioButton2.Checked Then


                            ccid = "XXXXX"
                        End If
                        'i = 0

                        'While (dr.Read())
                        '    str = str & dr.Item("itname") & "(" & array(i) & ")" & "-"
                        '    i = i + 1
                        'End While

                        Dim strbill As String = "      SUPER MARKET AL-MOUSAWI" & vbCrLf & "      ====================" & vbCrLf & "Date:" & System.DateTime.Today.ToString & vbCrLf & vbCrLf & "Point Of Sale:" & posidcombo.Text.Trim & vbCrLf & vbCrLf & "Employee Name: " & sopempcobo.SelectedItem.ToString.Substring(2).Trim & vbCrLf & vbCrLf & "Customer ID: " & ccid & vbCrLf & vbCrLf & "Items Saled: " & vbCrLf & str & vbCrLf & vbCrLf & "Total PRICE= " & total & " " & cur & vbCrLf & vbCrLf & "             Thank U 4 your visit!!"
                        billtxtbox.Text = strbill
                        conn.Close()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub updatecust_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles updatecust.Click
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else

            If cid.Text = "" Or (cid.Text <> "" AndAlso cname.Text <> "" AndAlso ctel.Text <> "" AndAlso cadd.Text <> "") Then

                MsgBox("Enter the customer id,and then field to be updated")
            Else

                ds = New DataSet
                Dim str As String = "select * from customers"
                Dim da As New SqlDataAdapter(str, conn)
                da.Fill(ds, "customers")
                Dim sql As String
                If ctel.Text = "" AndAlso cadd.Text = "" Then
                    sql = "update customers set cname= '" & cname.Text & "' where cid= '" & cid.Text & "'"
                End If
                If cname.Text = "" AndAlso cadd.Text = "" Then
                    sql = "update customers set ctel = '" & ctel.Text & "' where cid= '" & cid.Text & "'"
                End If
                If cname.Text = "" AndAlso ctel.Text = "" Then
                    sql = "update customers set cadd= '" & cadd.Text & "' where cid= '" & cid.Text & "'"
                End If

                Dim cmd As New SqlCommand(sql, conn)
                conn.Open()
                cmd.ExecuteNonQuery()
                da.Update(ds, "customers")
                MsgBox("Update was made successfully!!")
                conn.Close()
            End If
        End If
    End Sub


    'BEST customer


    Private Sub bestcust_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bestcust.Click
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            Dim dr As SqlDataReader
            Dim cmd As New SqlCommand()
            With cmd
                .CommandText = "select cid,SUM(AMOUNTACC)as s	from SALEOPERATION	where CID != 'X'	group by CID	order by s asc "
                .Connection = conn
            End With
            Dim str, str2 As String
            str = ""
            str2 = ""
            Dim am, x As Double
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While dr.Read()
                str = dr("cid")
                am = dr("s")
                x = 1
            End While
            dr.Close()
            conn.Close()
            Dim dr2 As SqlDataReader
            Dim cmd2 As New SqlCommand()
            With cmd2
                .CommandText = "select cname from customers where cid = " & "'" & str & "'"
                .Connection = conn
            End With
            conn.Open()
            dr2 = cmd2.ExecuteReader()
            If x <> 1 Then
                MsgBox("No sale operation done for our CUSTOMERS!!")
            Else
                dr2.Read()
                str2 = dr2("cname")
                MsgBox("The best customer in the super market is: " & str2.ToUpper & vbCrLf & "He pay " & am & "L.L")

            End If
            dr2.Close()
        End If
        conn.Close()

    End Sub

    'WORST customer

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim dr As SqlDataReader
        Dim cmd As New SqlCommand()
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            With cmd
                .CommandText = "select cid,SUM(AMOUNTACC)as s	from SALEOPERATION	where CID != 'X'	group by CID	order by s desc "
                .Connection = conn
            End With
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim str, str2 As String
            str = ""
            str2 = ""
            Dim am, x As Double
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While dr.Read()
                str = dr("cid")
                am = dr("s")
                x = 1
            End While
            conn.Close()
            Dim dr2 As SqlDataReader
            Dim cmd2 As New SqlCommand()
            With cmd2
                .CommandText = "select cname from customers where cid = " & "'" & str & "'"
                .Connection = conn
            End With
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            dr2 = cmd2.ExecuteReader()
            If x <> 1 Then
                MsgBox("No sale opearion done for CUSTOMERS!!")
            Else
                dr2.Read()
                str2 = dr2("cname")
                MsgBox("The worst customer in the super market is: " & str2.ToUpper & vbCrLf & "He pay " & am & " L.L")
                dr2.Close()
            End If
            conn.Close()
            dr.Close()
        End If
    End Sub

    Private Sub addcust_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addcust.Click
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If cid.Text = "" Or cname.Text = "" Or cadd.Text = "" Or ctel.Text = "" Then
                MsgBox("check that u enter all the fields of NEW customer correctly.")
            Else
                Dim cb As New SqlClient.SqlCommandBuilder(da)
                ds = New DataSet
                Dim sql As String
                sql = "select * from customers"
                da = New SqlClient.SqlDataAdapter(sql, conn)
                da.Fill(ds, "customers")
                Dim str1, str2, str3, str4, sql3 As String
                str1 = cid.Text
                str2 = cname.Text
                str3 = ctel.Text
                str4 = cadd.Text
                Dim x, y As Integer
                Dim nrows As Integer
                nrows = ds.Tables("customers").Rows.Count
                For i = 0 To nrows - 1
                    If cid.Text = ds.Tables("customers").Rows(i).Item("cid").ToString.Trim Then
                        x = 1

                    ElseIf ctel.Text = ds.Tables("customers").Rows(i).Item("ctel").ToString.Trim Or ctel.Text.Length <> 8 Then
                        y = 1
                    End If
                Next
                If x = 1 Then
                    MsgBox("Invalid id! Please enter another customer id")
                Else
                    If y = 1 Then
                        MsgBox("Invalid phone number!!")
                    Else

                        sql3 = "insert into customers values('" & str1 & "','" & str2 & "','" & str3 & "','" & str4 & "')"
                        If ConnectionState.Closed Then
                            conn.Open()
                        End If
                        Dim cmd3 As New SqlCommand(sql3, conn)
                        cmd3.ExecuteNonQuery()
                        da.Update(ds, "customers")
                        MsgBox("This Customer was added successfully to the customers of our super market")
                        conn.Close()
                        cid.Clear()
                        ctel.Clear()
                        cadd.Clear()
                        cname.Clear()
                        acccidcombo.Items.Clear()
                        Dim dr As SqlDataReader
                        Dim cmd As New SqlCommand()
                        With cmd
                            .CommandText = "select cid from customers"
                            .Connection = conn
                        End With
                        If conn.State = ConnectionState.Closed Then
                            conn.Open()
                        End If
                        Dim str As String
                        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        While dr.Read()
                            str = dr("cid")
                            acccidcombo.Items.Add(str)
                        End While
                        dr.Close()

                    End If
                End If
            End If
        End If

    End Sub



    Private Sub login_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles login.Click
        If sopempcobo.SelectedItem = "" Or posidcombo.SelectedItem = "" Then
            MsgBox("Please Enter emplyee's name and the number of point of sale.")
        Else
            MsgBox("WElCOME " & sopempcobo.SelectedItem.ToString.Substring(2).Trim & "!!!")
            log = 1
            accidComboBox1.Items.Clear()
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            supcombo.Items.Clear()
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim drr As SqlDataReader
            Dim cmdd As New SqlCommand("select sid,sname from suppliers", conn)
            drr = cmdd.ExecuteReader(CommandBehavior.CloseConnection)
            While drr.Read()
                supcombo.Items.Add(drr("sid").ToString.Trim & "  " & drr("sname"))
            End While
            drr.Close()
            conn.Close()

            If sopempcobo.SelectedItem = "" Or sopempcobo.SelectedItem = "" Then
                MsgBox("Please LOGIN first!!")
            Else
                Dim dr As SqlDataReader
                Dim cmd As New SqlCommand()
                With cmd
                    .CommandText = "select cid from customers"
                    .Connection = conn
                End With
                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim str As String
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                While dr.Read()
                    str = dr("cid")
                    acccidcombo.Items.Add(str)
                End While
                dr.Close()
            End If
            conn.Close()
            'view accid in sop
            sopaccCombo.Items.Clear()
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            If sopempcobo.SelectedItem = "" Or sopempcobo.SelectedItem = "" Then
                MsgBox("Please LOGIN first!!")
            Else
                Dim dr2 As SqlDataReader
                Dim cmd As New SqlCommand("select * from account", conn)
                dr2 = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                While dr2.Read()
                    sopaccCombo.Items.Add(dr2("accid"))
                End While
            End If
            conn.Close()
            'view accounts in deposite
            accidComboBox1.Items.Clear()
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            If sopempcobo.SelectedItem = "" Or sopempcobo.SelectedItem = "" Then
                MsgBox("Please LOGIN first!!")
            Else
                Dim dr3 As SqlDataReader
                Dim cmd As New SqlCommand()
                With cmd
                    .CommandText = "select accid from account"
                    .Connection = conn
                End With
                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim str As String
                dr3 = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                While dr3.Read()
                    str = dr3("accid")
                    accidComboBox1.Items.Add(str)
                End While
                dr3.Close()
            End If
            conn.Close()

        End If

    End Sub


    'obtain the currency of the account to deposite using it 

    Private Sub depocuur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles depocuur.Click
        If posidcombo.SelectedItem = "" Or sopempcobo.SelectedItem = "" Then
            MsgBox("Please LOGIN first!!")
        Else
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            depcurrcombo.Items.Clear()
            Dim dr As SqlDataReader
            Dim str5 As String = sopaccCombo.SelectedItem
            Dim cmd As New SqlCommand("select * from account", conn)
            dr = cmd.ExecuteReader()
            If accidComboBox1.SelectedItem = "" Then
                MsgBox("Please choose the ACCOUNT ID first" & vbCrLf & "to obatain its CURRENCY typye ")
            Else
                While dr.Read()
                    If dr.Item("accid").ToString.Trim = accidComboBox1.SelectedItem.ToString.Trim Then
                        str = dr.Item("currtype")
                        depcurrcombo.Items.Add(str)
                    End If
                End While
                conn.Close()

            End If
            dr.Close()
        End If
    End Sub


    'TOLTAL balance of  super market
    Private Sub totalbalance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles totalbalance.Click
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            str = "select sum(totalbalance) from pointofsale "
            Dim dr2 As SqlDataReader
            Dim cmd As New SqlCommand(str, conn)
            dr2 = cmd.ExecuteReader()
            dr2.Read()
            str = dr2(0)
            MsgBox("The Total balance of the super market is: " & str & "L.L")
            conn.Close()
        End If
    End Sub

    'MOST ITEM saled
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            str = "select itid,sum(SALEDQUANTITY ) as s      from SALEDITEMSLIST group by ITID order by s desc"
            Dim dr As SqlDataReader
            Dim cmd As New SqlCommand(str, conn)
            dr = cmd.ExecuteReader()
            dr.Read()
            str = dr.Item(0)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            dr.Close()
            Dim da As New SqlDataAdapter("select * from items", conn)
            da.Fill(ds, "items")
            Dim da2 As New SqlDataAdapter("select itname from items where items.itid='" & str.Trim & "'", conn)
            da2.Fill(ds, "bestitem")
            str = ds.Tables("bestitem").Rows(0).Item(0).ToString.Trim
            MsgBox("The most saled item is : " & str.ToUpper)
            conn.Close()
        End If
    End Sub


    Private Sub clearsop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clearsop.Click
        sopidtxt.Clear()
        billtxtbox.Clear()

        makeinvoice.Enabled = True
    End Sub


    'Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
    '    If RadioButton2.Checked Then

    '        sopchosecid.Enabled = False
    '        choscurr.Enabled = False
    '    End If
    'End Sub

    'Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
    '    If RadioButton1.Checked Then

    '        sopchosecid.Enabled = True
    '        choscurr.Enabled = True
    '    End If
    'End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            If cid.Text = "" Then
                MsgBox("enter the CUSTOMER ID to be deleted!!")
            Else

                ds = New DataSet
                Dim str2, sql2 As String
                str = "select * from customers"
                str2 = "select * from account "
                Dim da2 As New SqlDataAdapter(str2, conn)
                da2.Fill(ds, "account")
                Dim da As New SqlDataAdapter(str, conn)
                da.Fill(ds, "customers")
                Dim nrows, i, x, y As Integer
                y = 0
                nrows = ds.Tables("customers").Rows.Count
                For i = 0 To nrows - 1
                    If cid.Text = ds.Tables("customers").Rows(i).Item("cid").ToString.Trim Then
                        x = 1
                    End If
                Next
                If x <> 1 Then
                    MsgBox("There is no customer with this ID!!")
                Else
                    nrows = ds.Tables("account").Rows.Count
                    For i = 0 To nrows - 1
                        If cid.Text = ds.Tables("account").Rows(i).Item("cid").ToString.Trim Then
                            y = 1

                        End If
                    Next
                    If y <> 0 Then
                        sql2 = "delete from account where account.cid='" & cid.Text & "'"
                        Dim cmd As New SqlCommand(sql2, conn)
                        cmd.ExecuteNonQuery()
                        da2.Update(ds, "account")
                    End If
                    Dim sql As String = "delete from customers where customers.cid='" & cid.Text & "'"
                    Dim cmd2 As New SqlCommand(sql, conn)
                    cmd2.ExecuteNonQuery()
                    da.Update(ds, "customers")
                    MsgBox("Customer with id " & cid.Text & " has been deleted !!")

                End If
            End If
            conn.Close()
        End If

    End Sub

    'customers with no accounts

    Private Sub Button5_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            str = " select customers.cname,customers.cid from customers where customers.cid in ( select customers.cid from customers except select account.cid from ACCOUNT )"

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            ds = New DataSet
            Dim da As New SqlDataAdapter(str, conn)
            da.Fill(ds, "custnoacc")
            Dim i, nrows, x As Integer
            nrows = ds.Tables("custnoacc").Rows.Count
            str = ""
            For i = 0 To nrows - 1
                str = str & ds.Tables("custnoacc").Rows(i).Item(0).ToString.Trim & vbCrLf

            Next
            MsgBox("The customers with no accounts are : " & str.ToUpper & vbCrLf)
            conn.Close()
        End If
    End Sub

    'LEAST ITEM SALedd
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If log <> 1 Then
            MsgBox("Please LOGIN first!!")
        Else
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            str = "select itid,sum(SALEDQUANTITY ) as s      from SALEDITEMSLIST group by ITID order by s asc"
            Dim dr As SqlDataReader
            Dim cmd As New SqlCommand(str, conn)
            dr = cmd.ExecuteReader()
            dr.Read()
            str = dr("itid")
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            dr.Close()
            Dim da As New SqlDataAdapter("select * from items", conn)
            da.Fill(ds, "items")
            Dim da2 As New SqlDataAdapter("select itname from items where items.itid='" & str.Trim & "'", conn)
            da2.Fill(ds, "leastitem")
            str = ds.Tables("leastitem").Rows(0).Item(0).ToString.Trim
            MsgBox("The least saled item is : " & str.ToUpper)
            conn.Close()
        End If
    End Sub


    Private Sub popsupplier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        supcombo.Items.Clear()
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim dr As SqlDataReader
        Dim cmd As New SqlCommand("select sid,sname from suppliers", conn)
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        While dr.Read()
            supcombo.Items.Add(dr("sid").ToString.Trim & "  " & dr("sname"))
        End While
        dr.Close()
        conn.Close()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton2.CheckedChanged
        sopaccCombo.Enabled = False

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton1.CheckedChanged
        sopaccCombo.Enabled = True

    End Sub

    Private Sub TabPage4_Click(sender As Object, e As EventArgs) Handles TabPage4.Click

    End Sub

    Private Sub posidcombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles posidcombo.SelectedIndexChanged

    End Sub

    Private Sub TabPage9_Click(sender As Object, e As EventArgs) Handles TabPage9.Click

    End Sub

    Private Sub TabPage5_Click(sender As Object, e As EventArgs) Handles TabPage5.Click

    End Sub

    Private Sub billtxtbox_TextChanged(sender As Object, e As EventArgs) Handles billtxtbox.TextChanged

    End Sub

    Private Sub TabPage6_Click(sender As Object, e As EventArgs) Handles TabPage6.Click

    End Sub
End Class