<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessRequest.aspx.cs" Inherits="ProcessRequest" MasterPageFile="~/Site.master" %>

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
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../images/px_trans.gif" width="200" height="1" border="0" />
            </td>
            <td>
                <img src="../images/px_trans.gif" width="200" height="1" border="0" />
            </td>
            <td>
                <img src="../images/px_trans.gif" width="200" height="1" border="0" />
            </td>
        </tr>
        <tr>
            <td align="left" class="SectionTitle">
                Process a Request
            </td>
            <td colspan="2"></td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <hr color="#cccccc" />
            </td>
        </tr>
        <tr>
            <td class="SectionInstruction">
                <asp:Label ID="lblRequests" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="WorkArea" valign="top" style="background-color:#cccccc; height:13px; width:400px; vertical-align:middle; padding-left: 10px;">
                Available Queues
            </td>
        </tr>
        <tr>
            <td colspan="3" class="WorkArea" valign="top">
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="QueueGrid" runat="server" 
                                AutoGenerateColumns="False" 
                                Width="600px" Height="50px" 
                                SelectedRowStyle-Font-Bold="true" 
                                SelectedRowStyle-BackColor="Moccasin"   
                                OnSelectedIndexChanged="QueueSelected" CellPadding="4" OnRowCommand="RowCommand" 
                                AutoGenerateSelectButton="True"
                                DataKeyNames="Key" GridLines="None">
                                <RowStyle Height="20px" />
                                <Columns>
                                    <asp:CommandField ControlStyle-Width="50" />
                                    <asp:BoundField HeaderText="ID" DataField="Key" Visible="false" />
                                    <asp:BoundField HeaderText="Queue" DataField="SubQueueName" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderStyle Font-Bold="True" Font-Underline="true" Font-Size="12pt" ForeColor="#003366" Width="250" />
                                    <ItemStyle Font-Bold="True" Font-Size="12pt" ForeColor="Black" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="QC" DataField="QC" HeaderStyle-HorizontalAlign="Left" >
                                    <HeaderStyle Font-Bold="True" Font-Size="12pt" ForeColor="#003366" Font-Underline="true" 
                                        Width="50px" />
                                    <ItemStyle Font-Bold="False" Font-Size="12pt" ForeColor="Black" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="# Requests" DataField="Count" DataFormatString="{0:N}" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderStyle Font-Bold="True" Font-Size="12pt" ForeColor="#003366" Font-Underline="true" 
                                        Width="100px" />
                                    <ItemStyle Font-Bold="False" Font-Size="12pt" ForeColor="Black" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Oldest" DataField="Oldest" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Right">
                                    <HeaderStyle Font-Bold="True" Font-Size="12pt" ForeColor="#003366" Font-Underline="true" 
                                        Width="50px" />
                                    <ItemStyle Font-Bold="False" Font-Size="12pt" ForeColor="Black" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>

<SelectedRowStyle Font-Bold="True"></SelectedRowStyle>
                            </asp:GridView>
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
        </table>
        <asp:Panel ID="pnlSelection" runat="server">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <br />
                    <br />
                    Select one of the following Requests
                </td>
            </tr>
           <tr>
            <td colspan="3" class="WorkArea" valign="top" style="background-color:#cccccc; height:13px; width:400px; vertical-align:middle; padding-left: 10px;">
                <asp:Label ID="lblRequestInfo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="WorkArea" valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:GridView ID="RequestGrid" runat="server" 
                                AutoGenerateColumns="False" 
                                Width="600px" Height="50px" 
                                SelectedRowStyle-Font-Bold="true" 
                                OnSelectedIndexChanged="RequestSelected"
                                CellSpacing="0" CellPadding="4" OnRowCommand="RowCommand" 
                                AutoGenerateSelectButton="True"
                                DataKeyNames="RequestKey" GridLines="None">
                                <RowStyle Height="20px" />
                                <Columns>
                                    <asp:CommandField ControlStyle-Width="50" />
                                    <asp:BoundField HeaderText="ID" DataField="RequestKey" Visible="false" />
                                    <asp:BoundField HeaderText="Type" DataField="RequestType" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle Font-Bold="True" Font-Size="12pt" ForeColor="#003366" Font-Underline="true" />
                                        <ItemStyle Font-Bold="False" Font-Size="12pt" ForeColor="Black" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Name" DataField="UserName" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle Font-Bold="True" Font-Size="12pt" ForeColor="#003366" Font-Underline="true" />
                                        <ItemStyle Font-Bold="False" Font-Size="12pt" ForeColor="Black" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="eMail" DataField="UserEmail" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle Font-Bold="True" Font-Size="12pt" ForeColor="#003366" Font-Underline="true" />
                                        <ItemStyle Font-Bold="False" Font-Size="12pt" ForeColor="Black" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Created" DataField="CreateDate" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Right">
                                        <HeaderStyle Font-Bold="True" Font-Size="12pt" ForeColor="#003366" Font-Underline="true" />
                                        <ItemStyle Font-Bold="False" Font-Size="12pt" ForeColor="Black" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Comment" DataField="Comment" Visible="false">
                                        <HeaderStyle Font-Bold="True" Font-Size="12pt" ForeColor="#003366" Font-Underline="true" />
                                        <ItemStyle Font-Bold="False" Font-Size="12pt" ForeColor="Black" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <hr color="#cccccc" style="width: 90%; size="1px" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="3" valign="top" style="border: 1px solid #003366">
                            
                    		</td>
                    	</tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top">
                    
                    
                    
                    
                </td>
            </tr>
        </table>
    </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlRequests" Visible="false" runat="server">
    <table border="1" bordercolor="#003366" width="600" cellpadding="0" cellspacing="0">
        <tr bgcolor="#003366" height="15">
    		<td cellpadding="15 " class="WorkArea">
    			<font color="#ffffff">&nbsp;Process this Request : </font><asp:Label ID="lblQC" runat="server" Width="100px" ForeColor="White"></asp:Label>
    		</td>
    	</tr>
    	<tr>
    		<td valign="top">
    			<table cellpadding="10" width="400">
                    <tr>
                        <td align="right" width="100" class="Label">
                            <asp:Label ID="Label1" runat="server" Width="100px"></asp:Label><br />
                        </td>
                        <td></td>
                    </tr>
    				<tr>
    					<td align="right" width="100" class="Label" valign="top">
    						Queue Type:
    					</td>
    					<td align="left" width="300" class="StandardContent" valign="top">
    						<asp:Label ID="lblType" runat="server" Width="70px"></asp:Label>
    					</td>
    				</tr>
    				<tr>
    					<td align="right" class="Label" valign="top">
    						Comment:
    					</td>
    					<td align="left" class="StandardContent" valign="top">
    						<asp:TextBox ID="txtComment" runat="server" Width="400px" 
                                Wrap="true" Height="80px" ReadOnly="True"></asp:TextBox>
    					</td>
    				</tr>
    				<tr>
    					<td align="right" class="Label" valign="top">
    						Name:
    					</td>
    					<td align="left" class="StandardContent" valign="top">
    						<asp:Label ID="lblName" runat="server" Width="100px" ReadOnly="True"></asp:Label>
    					</td>
    				</tr>
    				<tr>
    					<td align="right" class="Label" valign="top">
    						Email:
    					</td>
    					<td align="left" class="StandardContent" valign="top">
    						<asp:Label ID="lblEmail" runat="server" Width="150px" ReadOnly="True"></asp:Label>
    					</td>
    				</tr>
    				<tr>
    					<td align="right" class="Label" valign="top">
    						Action Taken:
    					</td>
    					<td align="left" class="StandardContent" valign="top">
    						<asp:TextBox ID="txtActionTaken" runat="server" Width="400px" 
                                Wrap="true" Height="80px"></asp:TextBox>
    					</td>
    				</tr>
    				<tr>
    					<td align="right" class="Label" valign="top">
    						Route Next:
    					</td>
    					<td align="left" valign="top">
    						 <asp:DropDownList ID="cbNextQueue" runat="server" style="margin-left: 0px" >
                                <asp:ListItem>None</asp:ListItem>
                                <asp:ListItem>General</asp:ListItem>
                                <asp:ListItem>Service</asp:ListItem>
                                <asp:ListItem>Product</asp:ListItem>
                                <asp:ListItem>Marketing</asp:ListItem>
                            </asp:DropDownList>
    					</td>
    				</tr>
    				<tr>
    					<td colspan="2">
    						<hr size="1" color="#cccccc" width=95%" />
    					</td>
    				</tr>
    				<tr>
    					<td colspan="2" align="right" valign="top">
    						<asp:Button ID="Button1" runat="server" Text="Complete" OnClick="CompleteRequest"/>
                            <asp:Button ID="Button2" runat="server" Text="Cancel" OnClick="UnassignRequest"/>
    					</td>
    				</tr>
    			</table>
            </td>
        </table>
</asp:Panel>
</asp:Content>