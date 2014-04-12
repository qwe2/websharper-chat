(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,document,WebsharperChat,Client,WebSocket,window,WebSharper,Html,Default,List,EventsPervasives,String;
 Runtime.Define(Global,{
  WebsharperChat:{
   Client:{
    Append:function(data)
    {
     document.getElementById("chatbox").appendChild(Client.Render(data,"black").Body);
    },
    Connect:function(href)
    {
     return Client.SetEventHandlers(new WebSocket(href));
    },
    Main:function()
    {
     var ws,textbox,arg00;
     ws=Client.Connect("ws://"+window.location.host+"/chat");
     textbox=Default.Input(List.ofArray([Default.Text(""),Default.Attr().NewAttr("id","message-box"),Default.Attr().Class("form-control"),Default.Attr().NewAttr("type","text")]));
     arg00=function()
     {
      return function(_char)
      {
       return _char.CharacterCode===13?Client.SendText(ws,textbox):null;
      };
     };
     EventsPervasives.Events().OnKeyPress(arg00,textbox);
     return Default.Div(List.ofArray([Default.Div(List.ofArray([Default.Attr().NewAttr("id","chatbox")])),textbox]));
    },
    Render:function(text,color)
    {
     var arg10;
     arg10="color: "+color;
     return Default.P(List.ofArray([Default.Text(text),Default.Attr().NewAttr("style",arg10)]));
    },
    SendText:function(ws,textbox)
    {
     ws.send(textbox.get_Value());
     return textbox.set_Value("");
    },
    SetEventHandlers:function(ws)
    {
     ws.onmessage=function(data)
     {
      return Client.Append(String(data.data));
     };
     ws.onerror=function()
     {
      Client.Render("Error","red");
     };
     return ws;
    }
   },
   Controls:{
    EntryPoint:Runtime.Class({
     get_Body:function()
     {
      return Client.Main();
     }
    })
   }
  }
 });
 Runtime.OnInit(function()
 {
  document=Runtime.Safe(Global.document);
  WebsharperChat=Runtime.Safe(Global.WebsharperChat);
  Client=Runtime.Safe(WebsharperChat.Client);
  WebSocket=Runtime.Safe(Global.WebSocket);
  window=Runtime.Safe(Global.window);
  WebSharper=Runtime.Safe(Global.IntelliFactory.WebSharper);
  Html=Runtime.Safe(WebSharper.Html);
  Default=Runtime.Safe(Html.Default);
  List=Runtime.Safe(WebSharper.List);
  EventsPervasives=Runtime.Safe(Html.EventsPervasives);
  return String=Runtime.Safe(Global.String);
 });
 Runtime.OnLoad(function()
 {
  return;
 });
}());
