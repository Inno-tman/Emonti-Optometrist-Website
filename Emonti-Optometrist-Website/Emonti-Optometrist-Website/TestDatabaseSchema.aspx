<%@ Page Title="Test Database Schema" Language="C#" AutoEventWireup="true" CodeBehind="TestDatabaseSchema.aspx.cs" Inherits="Emonti_Optometrist_Website.TestDatabaseSchema" %>

<!DOCTYPE html>
<html>
<head>
    <title>Database Schema Test</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        h1 { color: #2c5aa0; }
        .success { color: green; padding: 10px; background: #d4edda; border: 1px solid #c3e6cb; margin: 10px 0; }
        .error { color: red; padding: 10px; background: #f8d7da; border: 1px solid #f5c6cb; margin: 10px 0; }
        .info { padding: 10px; background: #d1ecf1; border: 1px solid #bee5eb; margin: 10px 0; }
        table { border-collapse: collapse; width: 100%; margin: 20px 0; }
        th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
        th { background-color: #2c5aa0; color: white; }
        tr:nth-child(even) { background-color: #f2f2f2; }
    </style>
</head>
<body>
    <form runat="server">
        <h1>Database Schema Diagnostics</h1>
        
        <asp:Panel ID="pnlResults" runat="server">
            <asp:Literal ID="litResults" runat="server" />
        </asp:Panel>
        
        <asp:Button ID="btnTestConnection" runat="server" Text="Test Database Connection" OnClick="btnTestConnection_Click" />
        <asp:Button ID="btnGetSchema" runat="server" Text="Get Customer Table Schema" OnClick="btnGetSchema_Click" />
        <asp:Button ID="btnTestInsert" runat="server" Text="Test Insert" OnClick="btnTestInsert_Click" />
    </form>
</body>
</html>

