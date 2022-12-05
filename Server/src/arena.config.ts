import Arena from "@colyseus/arena";
import { monitor } from "@colyseus/monitor";

/**
 * Import your Room files
 */
import {MyLobbyRoom} from "./rooms/MyLobbyRoom";
import {MyGameRoom} from "./rooms/MyGameRoom";
import {LobbyRoom, matchMaker, Room} from "colyseus";

export default Arena({
    getId: () => "Your Colyseus App",

    initializeGameServer: (gameServer) => {
        /**
         * Define your room handlers:
         */
        gameServer.define('my_lobby', MyLobbyRoom);
        gameServer.define('my_game', MyGameRoom).enableRealtimeListing();

    },

    initializeExpress: (app) => {
        /**
         * Bind your custom express routes here:
         */
        app.get("/", (req, res) => {
            res.send("It's time to kick ass and chew bubblegum!");
        });

        /**
         * Bind @colyseus/monitor
         * It is recommended to protect this route with a password.
         * Read more: https://docs.colyseus.io/tools/monitor/
         */
        app.use("/colyseus", monitor());
    },


    beforeListen: () => {
        /**
         * Before before gameServer.listen() is called.
         */
        matchMaker.create("my_lobby");

    }
});