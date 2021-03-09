<template>
    <div>
        <h1 class="text-center">{{ msg }}</h1>

        <p class="text-center">
            This is a Vue App served from a local web root. Click on the button below to send a message to the backend. It will respond and send a message back to the UI.
        </p>

        <button class="primary center" v-on:click="callDotNet(`Hello .NET! 🤖`)">Call .NET</button>
    </div>
</template>

<script lang="ts">
    // Declare the necessary interface for messaging functions
    // in the PhotinoWindow application.
    declare global {
        interface External {
            sendMessage: (message: string) => void;
            receiveMessage: (delegate: (message: string) => void) => void;
        }

        interface Window { External: object; }
    }

    // Make sure that sendMessage and receiveMessage exist
    // when the frontend is started without the Photino context.
    // I.e. using Vue's `npm run serve` command and hot reload.
    if (typeof(window.external.sendMessage) !== 'function') {
        window.external.sendMessage = (message: string) => console.log("Emulating sendMessage.\nMessage sent: " + message);
    }

    if (typeof(window.external.receiveMessage) !== 'function') {
        window.external.receiveMessage = (delegate: (message: string) => void) => {
            let message = 'Simulating message from backend.';
            delegate(message);
        };

        window.external.receiveMessage((message: string) => console.log("Emulating receiveMessage.\nMessage received: " + message));
    } else {
        window.external.receiveMessage((message: string) => alert(message));
    }

    import Vue from 'vue';

    export default Vue.extend({
        name: 'HelloPhotino',
        props: {
            msg: String,
        },
        methods: {
            callDotNet: (message: string) => {
                window.external.sendMessage(message);
            },
        },
    });
</script>
