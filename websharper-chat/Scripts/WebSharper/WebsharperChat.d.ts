declare module WebsharperChat {
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
    module ChatClient {
        interface Anim {
            scrollTop: number;
        }
        var RenderMsg : {
            (data: any): __ABBREV.__Html1.Element;
        };
        var RenderError : {
            (msg: string): __ABBREV.__Html1.Element;
        };
        var Append : {
            (elem: __ABBREV.__Html1.Element): void;
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
    module Chat {
        interface Message {
            Username: string;
            Msg: string;
        }
        interface User {
            Name: string;
        }
        interface WebSocketChatHandler {
        }
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
        get_Body(): __ABBREV.__Html1.IPagelet;
    }
    interface Action {
    }
    interface Website {
    }
}
declare module __ABBREV {
    
    export import __List = IntelliFactory.WebSharper.List;
    export import __Html = IntelliFactory.Html.Html;
    export import __Web = IntelliFactory.WebSharper.Web;
    export import __Html1 = IntelliFactory.WebSharper.Html;
    export import __Html5 = IntelliFactory.WebSharper.Html5;
    export import __Data = IntelliFactory.WebSharper.Formlet.Data;
}
