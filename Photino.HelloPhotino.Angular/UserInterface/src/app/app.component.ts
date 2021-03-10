import { Component } from '@angular/core';

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

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
    
export class AppComponent {
    title = 'ui';
    
    callDotNet = () => {
        window.external.sendMessage('Hi .NET! 🤖');
    }
}
