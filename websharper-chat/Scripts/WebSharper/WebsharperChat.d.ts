declare module WebsharperChat {
    module Skin {
        interface Page {
            Title: string;
            Body: __ABBREV.__List.T<any>;
        }
    }
    module ChatClient {
        var Append : {
            (elem: __ABBREV.__Html.Element): void;
        };
        var ShowUserList : {
            (lst: __ABBREV.__Html.Element): void;
        };
        var AppendUser : {
            (name: string): void;
        };
        var RemoveUser : {
            (name: string): void;
        };
        var RenderError : {
            (msg: string): __ABBREV.__Html.Element;
        };
        var RenderMsg : {
            (user: string, msg: string): __ABBREV.__Html.Element;
        };
        var RenderUserlist : {
            (lst: string[]): __ABBREV.__Html.Element;
        };
        var RenderInfo : {
            (msg: string): __ABBREV.__Html.Element;
        };
        var HandleMessage : {
            (_arg1: __ABBREV.__Chat.Message): void;
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
            (logout: string): __ABBREV.__Html.Element;
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
        var RenderLoginForm : {
            <_M1, _M2>(x: __ABBREV.__Piglets.Stream<string>, y: __ABBREV.__Piglets.Stream<string>, submit: _M1): __ABBREV.__Html.Element;
        };
        var LoginForm : {
            (redirectUrl: string): __ABBREV.__Html.Element;
        };
        var LoginPiglet : {
            (): __ABBREV.__Piglets.Piglet<any, {
                (x: {
                    (x: __ABBREV.__Piglets.Stream<string>): {
                        (x: __ABBREV.__Piglets.Stream<string>): {
                            (x: __ABBREV.__Piglets.Submitter<any>): __ABBREV.__Html.Element;
                        };
                    };
                }): __ABBREV.__Html.Element;
            }>;
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
    interface ChatControl {
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
    export import __Piglets = IntelliFactory.WebSharper.Piglets;
}
