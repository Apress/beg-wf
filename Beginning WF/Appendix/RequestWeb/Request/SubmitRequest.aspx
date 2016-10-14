<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubmitRequest.aspx.cs" Inherits="SubmitRequest" MasterPageFile="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style3
        {
        height: 35px;
        width: 678px;
    }
    .style4
    {
        width: 678px;
    }
    .style5
    {
        width: 453px;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table>
        <tr>
            <td>
                <img src="../images/px_trans.gif" width="150" height="1" border="0" />
            </td>
            <td>
                <img src="../images/px_trans.gif" width="200" height="1" border="0" />
            </td>
        </tr>
        <tr>
            <td align="left" class="SectionTitle">
                Submit a Request
            </td>
            <td></td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <hr color="#cccccc" size="1" />
            </td>
        </tr>
        <tr id="instruction line">
            <td></td>
            <td align="center">
                <asp:Label ID="lblInstruction" CssClass="Instruction" Visible="true" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="Label" valign="top">
                <asp:Label ID="lblName" runat="server">Name:</asp:Label>
            </td>
            <td align="left" valign="top">
                <asp:TextBox ID="txtName" runat="server" Width="500"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" class="Label" valign="top">
                <asp:Label ID="lblEmail" runat="server">Email Address:</asp:Label>
            </td>
            <td align="left" valign="top">
                <asp:TextBox ID="txtEmail" runat="server" Width="500"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" class="Label" valign="top">
                <asp:Label ID="lblCategory" runat="server">Category:</asp:Label>
            </td>
            <td align="left" valign="top">
                <asp:DropDownList ID="cbCategory" runat="server" style="margin-left: 0px" >
                    <asp:ListItem>General</asp:ListItem>
                    <asp:ListItem>Help</asp:ListItem>
                    <asp:ListItem>Product Info</asp:ListItem>
                    <asp:ListItem>Feedback</asp:ListItem>
                    <asp:ListItem>Return</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" class="Label" valign="top">
                <asp:Label ID="lblComments" runat="server">Comments:</asp:Label>
            </td>
            <td align="left" valign="top">
                <asp:TextBox ID="txtComment" runat="server" Width="500" TextMode="MultiLine" Height="300"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td align="left" valign="top" colspan="2">
                <asp:ImageButton ID="btnAddItem" ImageUrl="~/images/additem.png" 
                Text="Add Item" runat="server" style="margin-left: 4px" Width="74px" 
                onclick="btnAddItem_Click" />
            </td>
        </tr>
    </table>
</asp:Content>