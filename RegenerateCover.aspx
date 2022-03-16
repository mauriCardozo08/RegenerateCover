<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegenerateCover.aspx.cs" Inherits="RegenerateCover_RegenerateCover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.11.0/themes/smoothness/jquery-ui.css" />
    <link rel="stylesheet" href="Styles/Loading.css" />
    <link href="Styles/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
</head>
<body class="container-fluid">


    <div class="row" style="border-bottom: solid 1px grey">
        <h2 class="col-xs-12 text-center">Regenerar Carátulas</h2>
    </div>

    <form id="form1" runat="server" class="row text-center aling-items-center" style="margin-top: 40px">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </div>
        
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                 <div class="modal2">
                    <div class="center2">
                        <img alt="" src="Images/squares.gif" />
                    </div>
                 </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label runat="server" Style="color: black; font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 10pt;">Ingrese un número de expediente:&nbsp;&nbsp;</asp:Label>
                <asp:TextBox runat="server" ID="nroExpediente" Width="15%" Height="25px" placeholder="Ej. 0001/2017"></asp:TextBox>
                <asp:Button runat="server" ID="btnSubmit2" OnClientClick="return validateAndHidde()" OnClick="SubmitRegenerate_Click" Text="Regenerar" />
                
                <div id="messageContainer" runat="server" class="row text-center aling-items-center" style="margin-top: 40px;">
                    <h3 runat="server" id="alertError" class="alert alert-danger text-center col-xs-8" role="alert" style="margin-left: 16.66666667%; display:none;"></h3>
                    <h3 runat="server" id="alertSucces" class="alert alert-success text-center col-xs-8" role="alert" style="margin-left: 16.66666667%; display: none;">La carátula ha sido regenerada.</h3>
                    <h3 id="alertErrorClient" class="alert alert-danger text-center col-xs-8" role="alert" style="margin-left: 16.66666667%; display:none;"> Ingrese un numero de expediente.</h3>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

       
    </form>

    <script type="text/javascript">
        function validateAndHidde() {
            let txtValue = document.getElementById('<%=nroExpediente.ClientID%>').value;
            let alertError = document.getElementById('<%=alertError.ClientID%>');
            let alertSucces = document.getElementById('<%=alertSucces.ClientID%>');
            alertSucces.style.display = "none";

            if (txtValue == "") {
                alertError.innerHTML = "Ingrese un numero de expediente.";
                alertError.style.display = "block";
                return false
            } else {
                alertError.style.display = "none";
                
            }
        }
    </script>
</body>
</html>
