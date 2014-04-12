declare module WebsharperChat {
    module Chat {
        interface WebSocketChatHandler {
        }
    }
    module Skin {
        interface Page {
            Title: string;
            Body: __ABBREV.__List.T<__ABBREV.__Html.Element<__ABBREV.__Web.Control>>;
        }
    }
    module Controls {
        interface EntryPoint {
            get_Body(): __ABBREV.__Html1.IPagelet;
        }
    }
    module Client {
        var Render : {
            (text: string, color: string): __ABBREV.__Html1.Element;
        };
        var Append : {
            (data: string): void;
        };
        var SetEventHandlers : {
            (ws: __ABBREV.__Html5.WebSocket): __ABBREV.__Html5.WebSocket;
        };
        var Connect : {
            (href: string): __ABBREV.__Html5.WebSocket;
        };
        var SendText : {
            (ws: __ABBREV.__Html5.WebSocket, textbox: __ABBREV.__Html1.Element): void;
        };
        var Main : {
            (): __ABBREV.__Html1.Element;
        };
    }
    interface Action {
    }
    interface Website {
    }
    interface ChatWebSocket {
    }
}
declare module __ABBREV {
    
    export import __List = IntelliFactory.WebSharper.List;
    export import __Html = IntelliFactory.Html.Html;
    export import __Web = IntelliFactory.WebSharper.Web;
    export import __Html1 = IntelliFactory.WebSharper.Html;
    export import __Html5 = IntelliFactory.WebSharper.Html5;
}
