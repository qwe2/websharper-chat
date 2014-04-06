(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,WebSharper,Html,Default,List,websharper_chat,Client,EventsPervasives,Concurrency,Remoting;
 Runtime.Define(Global,{
  websharper_chat:{
   Client:{
    Main:function()
    {
     var input,label,x,arg00;
     input=Default.Input(List.ofArray([Default.Text("")]));
     label=Default.Div(List.ofArray([Default.Text("")]));
     x=Default.Button(List.ofArray([Default.Text("Click")]));
     arg00=function()
     {
      return function()
      {
       return Client.Start(input.get_Value(),function(out)
       {
        return label.set_Text(out);
       });
      };
     };
     EventsPervasives.Events().OnClick(arg00,x);
     return Default.Div(List.ofArray([input,label,x]));
    },
    Start:function(input,k)
    {
     return Concurrency.Start(Concurrency.Delay(function()
     {
      return Concurrency.Bind(Remoting.Async("websharper_chat:0",[input]),function(_arg1)
      {
       return Concurrency.Return(k(_arg1));
      });
     }));
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
  WebSharper=Runtime.Safe(Global.IntelliFactory.WebSharper);
  Html=Runtime.Safe(WebSharper.Html);
  Default=Runtime.Safe(Html.Default);
  List=Runtime.Safe(WebSharper.List);
  websharper_chat=Runtime.Safe(Global.websharper_chat);
  Client=Runtime.Safe(websharper_chat.Client);
  EventsPervasives=Runtime.Safe(Html.EventsPervasives);
  Concurrency=Runtime.Safe(WebSharper.Concurrency);
  return Remoting=Runtime.Safe(WebSharper.Remoting);
 });
 Runtime.OnLoad(function()
 {
  return;
 });
}());
