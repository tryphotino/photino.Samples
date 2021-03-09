import photinoLogo from './photino-logo.svg';
import './App.css';

function App() {
    // Make sure that sendMessage and receiveMessage exist
    // when the frontend is started without the Photino context.
    // I.e. using React's `npm run start` command and hot reload.
    if (typeof(window.external.sendMessage) !== 'function') {
        window.external.sendMessage = (message) => console.log("Emulating sendMessage.\nMessage sent: " + message);
    }

    if (typeof(window.external.receiveMessage) !== 'function') {
        window.external.receiveMessage = (delegate) => {
            let message = 'Simulating message from backend.';
            delegate(message);
        };

        window.external.receiveMessage((message) => console.log("Emulating receiveMessage.\nMessage received: " + message));
    } else {
        window.external.receiveMessage((message) => alert(message));
    }

    function callDotNet() {
        window.external.sendMessage('Hi .NET! 🤖');
    }
    
    return (
        <div className="App">
            <img src={photinoLogo} alt="Photino" className="logo center" />

            <h1 className="text-center">Hello to Photino.React</h1>
        
            <p className="text-center">
                This is a React App served from a local web root. Click on the button below to send a message to the backend. It will respond and send a message back to the UI.
            </p>

            <button className="primary center" onClick={callDotNet}>Call .NET</button>
        </div>
    );
}

export default App;
