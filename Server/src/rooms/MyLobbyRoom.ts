import { Schema, type } from "@colyseus/schema";
import { Client, LobbyRoom } from "colyseus";
import {MyRoomState} from "./schema/MyRoomState";

export class MyLobbyRoom extends LobbyRoom {
    async onCreate(options:any) {
        await super.onCreate(options);
        this.autoDispose = false;
        this.setState(new MyRoomState());
    }

    onJoin(client: Client, options:any) {
        super.onJoin(client, options);
        this.state.custom = client.sessionId;
    }

    onLeave(client: Client) {
        super.onLeave(client);
    }
}