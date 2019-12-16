<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="regestraionandlogin.WebForm1" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource1" Height="338px" Width="657px">
                <series>
                    <asp:Series ChartType="Pie" IsValueShownAsLabel="True" IsXValueIndexed="True" Legend="Legend1" Name="Series1" XValueMember="StockName" YValueMembers="price">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </chartareas>
                <legends>
                    <asp:Legend Docking="Left" Name="Legend1" Title="Stocks" TitleAlignment="Near">
                    </asp:Legend>
                </legends>
            </asp:Chart>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyDatabaseConnectionString %>" SelectCommand="SELECT [StockName], [price] FROM [Stock]"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
