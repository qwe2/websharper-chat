(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,document,jQuery,WebSharper,Html,Default,List,WebsharperChat,ChatClient,WebSocket,window,EventsPervasives,Operators,JavaScript,Arrays,Strings,JSON,String,ClAuth,Piglets,Piglet1,Pervasives,Validation,Remoting,Controls;
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
    AppendUser:function(name)
    {
     var n;
     n=Default.LI(List.ofArray([Default.Text(name)]));
     document.getElementById("userli").appendChild(n.Body);
     return;
    },
    Connect:function(href)
    {
     return ChatClient.SetEventHandlers(new WebSocket(href));
    },
    HandleMessage:function(_arg1)
    {
     var name,name1;
     if(_arg1.$==0)
      {
       return ChatClient.Append(ChatClient.RenderMsg(_arg1.$0,_arg1.$1));
      }
     else
      {
       if(_arg1.$==2)
        {
         return ChatClient.ShowUserList(ChatClient.RenderUserlist(_arg1.$0));
        }
       else
        {
         if(_arg1.$==3)
          {
           if(_arg1.$0)
            {
             name=_arg1.$1;
             ChatClient.Append(ChatClient.RenderInfo(name+" has connected"));
             return ChatClient.AppendUser(name);
            }
           else
            {
             name1=_arg1.$1;
             ChatClient.Append(ChatClient.RenderInfo(name1+" has disconnected"));
             return ChatClient.RemoveUser(name1);
            }
          }
         else
          {
           return ChatClient.Append(ChatClient.RenderError(_arg1.$0));
          }
        }
      }
    },
    Main:function(logout)
    {
     var ws,textbox,arg00;
     ws=ChatClient.Connect("ws://"+window.location.host+"/chatsocket");
     textbox=Default.Input(List.ofArray([Default.Text(""),Default.Attr().NewAttr("id","message-box"),Default.Attr().Class("form-control col-md-12"),Default.Attr().NewAttr("type","text")]));
     arg00=function()
     {
      return function(_char)
      {
       return _char.CharacterCode===13?ChatClient.SendText(ws,textbox):null;
      };
     };
     EventsPervasives.Events().OnKeyPress(arg00,textbox);
     return Operators.add(Default.Div(List.ofArray([Default.Attr().Class("container")])),List.ofArray([Default.A(List.ofArray([Default.Attr().NewAttr("href",logout),Default.Attr().Class("btn btn-primary"),Default.Text("Log out")])),Operators.add(Default.Div(List.ofArray([Default.Attr().Class("row styled_container")])),List.ofArray([Default.Div(List.ofArray([Default.Attr().NewAttr("id","chatbox"),Default.Attr().Class("col-md-10")])),Default.Div(List.ofArray([Default.Attr().NewAttr("id","userlist"),Default.Attr().Class("col-md-2")]))])),Operators.add(Default.Div(List.ofArray([Default.Attr().Class("row")])),List.ofArray([textbox]))]));
    },
    RemoveUser:function(name)
    {
     jQuery("#userli").children().each(function()
     {
      var n;
      n=jQuery(this);
      JavaScript.Log(n.text());
      return n.text()===name?void n.remove():null;
     });
    },
    RenderError:function(msg)
    {
     return Default.P(List.ofArray([Default.Text(msg),Default.Attr().Class("bg-danger")]));
    },
    RenderInfo:function(msg)
    {
     return Default.P(List.ofArray([Default.Text(msg),Default.Attr().Class("bg-warning")]));
    },
    RenderMsg:function(user,msg)
    {
     var arg10;
     arg10=List.ofArray([Default.Text(user+": ")]);
     return Operators.add(Default.P(List.ofArray([Default.Tags().NewTag("strong",arg10)])),List.ofArray([Default.Text(msg),Default.Attr().Class("bg-info")]));
    },
    RenderUserlist:function(lst)
    {
     return Operators.add(Default.UL(List.ofArray([Default.Attr().NewAttr("id","userli")])),Arrays.map(function(x)
     {
      return Default.LI(List.ofArray([Default.Text(x)]));
     },lst));
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
      return ChatClient.HandleMessage(JSON.parse(String(d.data)));
     };
     ws.onerror=function()
     {
      return ChatClient.Append(ChatClient.RenderError("Something went wrong."));
     };
     return ws;
    },
    ShowUserList:function(lst)
    {
     var ul;
     ul=jQuery("#userlist");
     ul.children().each(function()
     {
      jQuery(this).remove();
     });
     ul.append(lst.Body);
     return;
    }
   },
   ChatControl:Runtime.Class({
    get_Body:function()
    {
     return ChatClient.Main(this.logout);
    }
   }),
   ClAuth:{
    LoginForm:function(redirectUrl)
    {
     var _arg10_;
     _arg10_=ClAuth.LoginPiglet();
     return Piglet1.Render(function(x)
     {
      return function(y)
      {
       return function(submit)
       {
        return ClAuth.RenderLoginForm(x,y,submit);
       };
      };
     },Piglet1.Run(Runtime.Tupled(function()
     {
      window.location=redirectUrl;
      return redirectUrl;
     }),_arg10_));
    },
    LoginPiglet:Runtime.Field(function()
    {
     var _arg20_;
     _arg20_=Piglet1.WithSubmit(Pervasives.op_LessMultiplyGreater(Pervasives.op_LessMultiplyGreater(Piglet1.Return(function(x)
     {
      return function(y)
      {
       return[x,y];
      };
     }),Validation.Is(function(value)
     {
      return Validation.NotEmpty(value);
     },"Enter Username",Piglet1.Yield(""))),Validation.Is(function(value)
     {
      return Validation.NotEmpty(value);
     },"Enter password",Piglet1.Yield(""))));
     return Validation.Is(Runtime.Tupled(function(_arg1)
     {
      return Remoting.Call("WebsharperChat:0",[_arg1[0],_arg1[1]]);
     }),"Invalid username or password.",_arg20_);
    }),
    RenderLoginForm:function(x,y,submit)
    {
     var uni,pwi,btn,arg10,arg101;
     uni=Controls.input("text",function(x1)
     {
      return x1;
     },function(x1)
     {
      return x1;
     },x);
     uni["HtmlProvider@32"].AddClass(uni.Body,"form-control");
     pwi=Controls.input("text",function(x1)
     {
      return x1;
     },function(x1)
     {
      return x1;
     },y);
     pwi["HtmlProvider@32"].AddClass(pwi.Body,"form-control");
     pwi["HtmlProvider@32"].SetAttribute(pwi.Body,"type","password");
     btn=Controls.Submit(submit);
     btn["HtmlProvider@32"].AddClass(btn.Body,"btn btn-primary");
     jQuery(pwi.Body).keyup(function(e)
     {
      if(e.which===13)
       {
        e.preventDefault();
        jQuery(btn.Body).click();
        return;
       }
      else
       {
        return null;
       }
     });
     arg10=List.ofArray([Default.Attr().NewAttr("for",uni.get_Id()),Default.Attr().Class("col-sm-2 control-label"),Default.Text("Username")]);
     arg101=List.ofArray([Default.Attr().NewAttr("for",pwi.get_Id()),Default.Attr().Class("col-sm-2 control-label"),Default.Text("Password")]);
     return Operators.add(Default.Div(List.ofArray([Default.Attr().Class("form-horizontal container"),Default.Attr().NewAttr("style","margin-top: 2em; width: 50%; min-width: 200px")])),List.ofArray([Operators.add(Default.Div(List.ofArray([Default.Attr().Class("form-group")])),List.ofArray([Default.Tags().NewTag("label",arg10),Operators.add(Default.Div(List.ofArray([Default.Attr().Class("col-sm-10")])),List.ofArray([uni]))])),Operators.add(Default.Div(List.ofArray([Default.Attr().Class("form-group")])),List.ofArray([Default.Tags().NewTag("label",arg101),Operators.add(Default.Div(List.ofArray([Default.Attr().Class("col-sm-10")])),List.ofArray([pwi]))])),Operators.add(Default.Div(List.ofArray([Default.Attr().Class("form-group")])),List.ofArray([Operators.add(Default.Div(List.ofArray([Default.Attr().Class("col-sm-offset-2 col-sm-10")])),List.ofArray([btn]))])),Controls.ShowErrors(submit,function(errors)
     {
      return List.map(function(msg)
      {
       return Operators.add(Default.P(List.ofArray([Default.Attr().NewAttr("style","color: red")])),List.ofArray([Default.Text(msg)]));
      },errors);
     },Default.Div(List.ofArray([Default.Attr().NewAttr("id","errors")])))]));
    }
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
  WebSharper=Runtime.Safe(Global.IntelliFactory.WebSharper);
  Html=Runtime.Safe(WebSharper.Html);
  Default=Runtime.Safe(Html.Default);
  List=Runtime.Safe(WebSharper.List);
  WebsharperChat=Runtime.Safe(Global.WebsharperChat);
  ChatClient=Runtime.Safe(WebsharperChat.ChatClient);
  WebSocket=Runtime.Safe(Global.WebSocket);
  window=Runtime.Safe(Global.window);
  EventsPervasives=Runtime.Safe(Html.EventsPervasives);
  Operators=Runtime.Safe(Html.Operators);
  JavaScript=Runtime.Safe(WebSharper.JavaScript);
  Arrays=Runtime.Safe(WebSharper.Arrays);
  Strings=Runtime.Safe(WebSharper.Strings);
  JSON=Runtime.Safe(Global.JSON);
  String=Runtime.Safe(Global.String);
  ClAuth=Runtime.Safe(WebsharperChat.ClAuth);
  Piglets=Runtime.Safe(WebSharper.Piglets);
  Piglet1=Runtime.Safe(Piglets.Piglet1);
  Pervasives=Runtime.Safe(Piglets.Pervasives);
  Validation=Runtime.Safe(Piglet1.Validation);
  Remoting=Runtime.Safe(WebSharper.Remoting);
  return Controls=Runtime.Safe(Piglets.Controls);
 });
 Runtime.OnLoad(function()
 {
  ClAuth.LoginPiglet();
  return;
 });
}());
