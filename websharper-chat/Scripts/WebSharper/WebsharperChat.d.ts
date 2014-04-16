declare module WebsharperChat {
    module Skin {
        interface Page {
            Title: string;
            Body: __ABBREV.__List.T<any>;
        }
    }
    module Controls {
        interface EntryPoint {
            get_Body(): __ABBREV.__Html.IPagelet;
        }
    }
    module ChatClient {
        interface Anim {
            scrollTop: number;
        }
        var RenderMsg : {
            (data: __ABBREV.__Chat.Message): __ABBREV.__Html.Element;
        };
        var RenderError : {
            (msg: string): __ABBREV.__Html.Element;
        };
        var Append : {
            (elem: __ABBREV.__Html.Element): void;
        };
        var SetEventHandlers : {
            (ws: __ABBREV.__Html5.WebSocket): __ABBREV.__Html5.WebSocket;
        };
        var Connect : {
            (href: string): __ABBREV.__Html5.WebSocket;
        };
        var SendText : {
            (ws: __ABBREV.__Html5.WebSocket, textbox: __ABBREV.__Html.Element): void;
        };
        var Main : {
            (): __ABBREV.__Html.Element;
        };
    }
    module Chat {
        interface Message {
        }
        interface User {
            Name: string;
        }
        interface WebSocketChatHandler {
        }
    }
    module ClAuth {
        var WarningPanel : {
            <_M1>(label: string): __ABBREV.__Data.Formlet<_M1>;
        };
        var WithLoadingPane : {
            <_M1, _M2>(a: any, f: {
                (x: _M1): __ABBREV.__Data.Formlet<_M2>;
            }): __ABBREV.__Data.Formlet<_M2>;
        };
        var LoginForm : {
            (redirectUrl: string): __ABBREV.__Data.Formlet<void>;
        };
    }
    module SQLConnection {
        module DbSchema {
            module ServiceTypes {
                module SimpleDataContextTypes {
                    interface WebSharperChat {
                    }
                }
                interface Users {
                }
                interface SimpleDataContextTypes {
                }
            }
            interface ServiceTypes {
            }
        }
        interface DbSchema {
        }
    }
    interface ChatWebSocket {
    }
    interface LoginControl {
        get_Body(): __ABBREV.__Html.IPagelet;
    }
    interface Action {
    }
    interface Website {
    }
}
declare module __ABBREV {
    
    export import __List = IntelliFactory.WebSharper.List;
    export import __Html = IntelliFactory.WebSharper.Html;
    export import __Chat = WebsharperChat.Chat;
    export import __Html5 = IntelliFactory.WebSharper.Html5;
    export import __Data = IntelliFactory.WebSharper.Formlet.Data;
}
