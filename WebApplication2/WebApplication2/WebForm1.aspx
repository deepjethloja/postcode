<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication2.WebForm1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyAI0_9NNDaUO14lzOn3RuOZsY9L5lkiUOc&sensor=false"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        <asp:Label ID="Label1" runat="server" Text="Enter a Post Code"></asp:Label>
            <asp:TextBox ID="Code" runat="server"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Find Address" OnClick="button_Click" />
    
    <div>
         <asp:Panel ID="pnlData" runat="server" Width="100%" Visible="true"> 
                       <asp:ListBox ID="showList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SelectedIndexChanged"
                        Rows="20" Visible="true" Font-Names="Arial" Font-Size="10px" Width="100%"></asp:ListBox>
                                 
            </asp:Panel>
         
           <!--<span id="spanId" runat="server" style="font-size:14pt; border:none" ></span>-->
        </div>
            </div>
         <asp:Label ID="Label2" runat="server" Text="First Name:"></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox><br/>
         <asp:Label ID="Label3" runat="server" Text="Last Name:"></asp:Label>
            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox><br/>
         <asp:Label ID="Label4" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox><br/>
         <asp:Label ID="Label5" runat="server" Text="Organisation Name:"></asp:Label>
            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox><br/>
         <asp:Label ID="Label6" runat="server" Text="Address"></asp:Label>
              <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
         <asp:Label ID="Label7" runat="server" Text="Town:"></asp:Label>
            <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox><br/>
          <asp:Label ID="Label8" runat="server" Text="County"></asp:Label>
            <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
          <asp:Label ID="Label9" runat="server" Text="PostCode"></asp:Label>
            <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
         <asp:Button ID="Button2" runat="server" Text="ADD" OnClick="AddToDatabase" />
        
        <asp:Button ID="btnSearch" runat="server" Text="Show On Map" OnClick="FindCoordinates" />
   
    <asp:Panel ID="pnlScripts" runat="server" Visible="false">
        <div id="dvMap" style="width: 300px; height: 300px">
        </div>
        <script type="text/javascript">
        var markers = [
        <asp:Repeater ID="rptMarkers" runat="server">
        <ItemTemplate>
                    {
                    "title": '<%# Eval("Address") %>',
                    "lat": '<%# Eval("Latitude") %>',
                    "lng": '<%# Eval("Longitude") %>',
                    "description": '<%# Eval("Address") %>'
                }
        </ItemTemplate>
        <SeparatorTemplate>
            ,
        </SeparatorTemplate>
        </asp:Repeater>
        ];
        </script>
        <script type="text/javascript">
            window.onload = function () {
                var mapOptions = {
                    center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                    zoom: 13,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                var infoWindow = new google.maps.InfoWindow();
                var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
                for (i = 0; i < markers.length; i++) {
                    var data = markers[i]
                    var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                    var marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map,
                        title: data.title
                    });
                    (function (marker, data) {
                        google.maps.event.addListener(marker, "click", function (e) {
                            infoWindow.setContent(data.description);
                            infoWindow.open(map, marker);
                        });
                    })(marker, data);
                }
            }
        </script>
    </asp:Panel>
    </form>
</body>
</html>


