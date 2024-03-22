import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)

//@ts-ignore
window.external.sendMessage(JSON.stringify({ key: "PHOTINO_TEST_CHANNEL", data: "Hello .NET 🤖" }))

//@ts-ignore
window.external.receiveMessage((data: string) => {
  var message = JSON.parse(data) as { key: string, data: string }
  if (message.key == 'PHOTINO_TEST_CHANNEL') {
    alert(message.data)
  }
})