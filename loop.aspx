<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="loop.aspx.cs" Inherits="loop" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" href="style.css" type="text/css"/>

</head>
<body>
    <div class="w3-row-padding"> 
    <form runat="server" class="yourform" id="form1" language="JavaScript" name="FrontPage_Form1"> 
    <div class="Frame">
     
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
            <ContentTemplate>         
                <p class="centered">&nbsp;</p>
       
                    <p class="centered"><img alt="" src="media/16_<%=arrayIndex%>.png" border="1"></p>
                
                <p class="centered">What is the first thing that comes to mind about the advertisement?</p>   

                    <asp:textbox runat="server" name="q15av" ID="q15av"> </asp:textbox>

                   <p class="centered">How do you feel about <u><%=items%></u>?                                    
                </p>
                 <div class="control-group">
                    <asp:RadioButtonList Id="q15b" name="q15b" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="radiobuttonrow">
                        <asp:ListItem text="1" Value="1"></asp:ListItem>
                        <asp:ListItem text="2" Value="2"></asp:ListItem>
                        <asp:ListItem text="3" Value="3"></asp:ListItem>
                        <asp:ListItem text="4" Value="4"></asp:ListItem>
                        <asp:ListItem text="5" Value="5"></asp:ListItem>
                        <asp:ListItem text="6" Value="6"></asp:ListItem>
                        <asp:ListItem text="7" Value="7"></asp:ListItem>
                        <asp:ListItem text="8" Value="8"></asp:ListItem>
                        <asp:ListItem text="9" Value="9"></asp:ListItem>
                        <asp:ListItem text="10" Value="10"></asp:ListItem>
                    </asp:RadioButtonList>
                     </div>

                <p class="centered">Which times of day do you like to eat?</p>
                <div class="control-group">
                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" CssClass="checkbutton"> 
                        <asp:ListItem Text=""></asp:ListItem>
                        <asp:ListItem Text=""></asp:ListItem>
                        <asp:ListItem Text=""></asp:ListItem>
                        <asp:ListItem Text=""></asp:ListItem>
                        <asp:ListItem Text=""></asp:ListItem>
                        <asp:ListItem Text=""></asp:ListItem>
                    </asp:CheckBoxList>
                  <label for="CheckBoxList1"></label>
             
                      </div>
            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Cont" EventName="Click"/>   
                    <asp:AsyncPostBackTrigger ControlID="Back" EventName="Click"/>            
                </Triggers>
        </asp:UpdatePanel>
        <div class="footer">
                <div class="submitbuttons">
                  <asp:Button ID="Cont" runat="server" OnClick="Forward_Click" class="button" Text="Next" />
                  <asp:Button ID="Back" runat="server" OnClick="Back_Click" class="button" Text="Back" />
                </div>
         
          
             
          
            </div>
    <asp:TextBox runat="server" name="text2" ID="text2"> </asp:TextBox>
    </div>
    </form>
        </div>
</body>
</html>
