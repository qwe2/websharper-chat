(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,document,jQuery,WebsharperChat,ChatClient,WebSocket,window,WebSharper,Html,Default,List,EventsPervasives,Operators,Strings,JSON,String,Formlet,Controls,Data,Enhance,Formlet1,FormButtonConfiguration,ClAuth,Remoting,Control,FSharpEvent,Concurrency,Formlet2,Base,Result;
 Runtime.Define(Global,{
  WebsharperChat:{
   ChatClient:{
    Append:function(elem)
    {
     var cb;
     cb=document.getElementById("chatbox").appendChild(elem.Body).parentNode;
     jQuery(cb).scrollTop(jQuery(cb).height());
     return;
    },
    Connect:function(href)
    {
     return ChatClient.SetEventHandlers(new WebSocket(href));
    },
    Main:function()
    {
     var ws,textbox,arg00;
     ws=ChatClient.Connect("ws://"+window.location.host+"/chatsocket");
     textbox=Default.Input(List.ofArray([Default.Text(""),Default.Attr().NewAttr("id","message-box"),Default.Attr().Class("form-control"),Default.Attr().NewAttr("type","text")]));
     arg00=function()
     {
      return function(_char)
      {
       return _char.CharacterCode===13?ChatClient.SendText(ws,textbox):null;
      };
     };
     EventsPervasives.Events().OnKeyPress(arg00,textbox);
     return Operators.add(Default.Div(List.ofArray([Default.Attr().Class("container")])),List.ofArray([Default.Div(List.ofArray([Default.Attr().NewAttr("id","chatbox")])),textbox]));
    },
    RenderError:function(msg)
    {
     return Default.P(List.ofArray([Default.Text(msg),Default.Attr().Class("bg-danger")]));
    },
    RenderMsg:function(data)
    {
     var arg10;
     arg10=List.ofArray([Default.Text(data.Username+": ")]);
     return Operators.add(Default.P(List.ofArray([Default.Tags().NewTag("strong",arg10)])),List.ofArray([Default.Text(data.Msg),Default.Attr().Class("bg-info")]));
    },
    SendText:function(ws,textbox)
    {
     var text;
     text=Strings.Trim(textbox.get_Value());
     if(text!=="")
      {
       ws.send(text);
      }
     return textbox.set_Value("");
    },
    SetEventHandlers:function(ws)
    {
     ws.onmessage=function(d)
     {
      return ChatClient.Append(ChatClient.RenderMsg(JSON.parse(String(d.data))));
     };
     ws.onerror=function()
     {
      return ChatClient.Append(ChatClient.RenderError("Something went wrong."));
     };
     return ws;
    }
   },
   ClAuth:{
    LoginForm:function(redirectUrl)
    {
     var arg10,x,uName,arg101,x1,pw,loginF,_builder_;
     arg10=Controls.Input("");
     x=Data.Validator().IsNotEmpty("Enter Username",arg10);
     uName=Enhance.WithTextLabel("Username",x);
     arg101=Controls.Password("");
     x1=Data.Validator().IsNotEmpty("Enter Password",arg101);
     pw=Enhance.WithTextLabel("Password",x1);
     loginF=Data.$(Data.$(Formlet1.Return(function(n)
     {
      return function(pw1)
      {
       return[n,pw1];
      };
     }),uName),pw);
     _builder_=Formlet1.Do();
     return Enhance.WithFormContainer(_builder_.Delay(function()
     {
      var inputRecord,submitConf,inputRecord1;
      inputRecord=FormButtonConfiguration.get_Default();
      submitConf=Runtime.New(FormButtonConfiguration,{
       Label:{
        $:1,
        $0:"Login"
       },
       Style:inputRecord.Style,
       Class:inputRecord.Class
      });
      inputRecord1=FormButtonConfiguration.get_Default();
      return _builder_.Bind(Enhance.WithCustomSubmitAndResetButtons(submitConf,Runtime.New(FormButtonConfiguration,{
       Label:{
        $:1,
        $0:"Reset"
       },
       Style:inputRecord1.Style,
       Class:inputRecord1.Class
      }),loginF),Runtime.Tupled(function(_arg1)
      {
       var f;
       f=function(loggedIn)
       {
        if(loggedIn)
         {
          window.location=redirectUrl;
          return Formlet1.Return(null);
         }
        else
         {
          return ClAuth.WarningPanel("Login failed");
         }
       };
       return _builder_.ReturnFrom(ClAuth.WithLoadingPane(Remoting.Async("WebsharperChat:0",[_arg1[0],_arg1[1]]),f));
      }));
     }));
    },
    WarningPanel:function(label)
    {
     var _builder_;
     _builder_=Formlet1.Do();
     return _builder_.Delay(function()
     {
      return _builder_.Bind(Formlet1.OfElement(function()
      {
       return Operators.add(Default.Div(List.ofArray([Default.Attr().Class("warningPanel")])),List.ofArray([Default.Text(label)]));
      }),function()
      {
       return _builder_.ReturnFrom(Formlet1.Never());
      });
     });
    },
    WithLoadingPane:function(a,f)
    {
     return Formlet1.Replace(Formlet1.BuildFormlet(function()
     {
      var elem,state;
      elem=Default.Div(List.ofArray([Default.Attr().Class("loadingPane")]));
      state=FSharpEvent.New();
      Concurrency.Start(Concurrency.Delay(function()
      {
       return Concurrency.Bind(a,function(_arg11)
       {
        state.event.Trigger(Runtime.New(Result,{
         $:0,
         $0:_arg11
        }));
        return Concurrency.Return(null);
       });
      }));
      return[elem,function()
      {
      },state.event];
     }),f);
    }
   },
   Controls:{
    EntryPoint:Runtime.Class({
     get_Body:function()
     {
      return ChatClient.Main();
     }
    })
   },
   LoginControl:Runtime.Class({
    get_Body:function()
    {
     return ClAuth.LoginForm(this.redirectUrl);
    }
   })
  }
 });
 Runtime.OnInit(function()
 {
  document=Runtime.Safe(Global.document);
  jQuery=Runtime.Safe(Global.jQuery);
  WebsharperChat=Runtime.Safe(Global.WebsharperChat);
  ChatClient=Runtime.Safe(WebsharperChat.ChatClient);
  WebSocket=Runtime.Safe(Global.WebSocket);
  window=Runtime.Safe(Global.window);
  WebSharper=Runtime.Safe(Global.IntelliFactory.WebSharper);
  Html=Runtime.Safe(WebSharper.Html);
  Default=Runtime.Safe(Html.Default);
  List=Runtime.Safe(WebSharper.List);
  EventsPervasives=Runtime.Safe(Html.EventsPervasives);
  Operators=Runtime.Safe(Html.Operators);
  Strings=Runtime.Safe(WebSharper.Strings);
  JSON=Runtime.Safe(Global.JSON);
  String=Runtime.Safe(Global.String);
  Formlet=Runtime.Safe(WebSharper.Formlet);
  Controls=Runtime.Safe(Formlet.Controls);
  Data=Runtime.Safe(Formlet.Data);
  Enhance=Runtime.Safe(Formlet.Enhance);
  Formlet1=Runtime.Safe(Formlet.Formlet);
  FormButtonConfiguration=Runtime.Safe(Enhance.FormButtonConfiguration);
  ClAuth=Runtime.Safe(WebsharperChat.ClAuth);
  Remoting=Runtime.Safe(WebSharper.Remoting);
  Control=Runtime.Safe(WebSharper.Control);
  FSharpEvent=Runtime.Safe(Control.FSharpEvent);
  Concurrency=Runtime.Safe(WebSharper.Concurrency);
  Formlet2=Runtime.Safe(Global.IntelliFactory.Formlet);
  Base=Runtime.Safe(Formlet2.Base);
  return Result=Runtime.Safe(Base.Result);
 });
 Runtime.OnLoad(function()
 {
  return;
 });
}());
