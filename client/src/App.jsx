import { useEffect, useState } from "react";
import { MESSAGE_HUB, MESSAGE_URL } from "../constants";
import { HubConnectionBuilder } from "@microsoft/signalr";
import "../public/styles.css";

const connection = new HubConnectionBuilder().withUrl(MESSAGE_HUB).build();

function App() {
  const [textInput, setTextInput] = useState("");
  const [messageCounter, setMessageCounter] = useState(0);
  const [lastMessages, setLastMessages] = useState([]);
  const [curMessages, setCurMessages] = useState([]);
  const [isSignalConnection, setIsSignalConnection] = useState(false);

  useEffect(() => {
    if (isSignalConnection) return;
    connection
      .start()
      .then(() => {
        console.log("Connected to SignalR hub");
        setIsSignalConnection(true);
      })
      .catch((err) => console.error("Error connecting to hub:", err));
  }, []);

  useEffect(() => {
    if (!isSignalConnection) return;
    connection.on("RecieveMessage", (message) => {
      setCurMessages((prev) => [message, ...prev]);
    });
  }, [isSignalConnection]);

  const handleSendMessage = () => {
    if (!textInput) return;

    const message = { orderNumber: messageCounter, text: textInput };

    fetch(MESSAGE_URL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(message),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Ошибка HTTP: ${response.status}`);
        }
        return response.json();
      })
      .then((data) => {
        console.log("Успех:", data);
      })
      .catch((error) => {
        console.error("Ошибка:", error);
      });

    setMessageCounter((prev) => prev + 1);
    setTextInput("");
  };

  const handleGetMessage = () => {
    const url = MESSAGE_URL + `?period=00:10`;
    fetch(url, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Ошибка HTTP: ${response.status}`);
        }
        return response.json();
      })
      .then((data) => {
        setLastMessages(data);
      })
      .catch((error) => {
        console.error("Ошибка:", error);
      });
  };

  return (
    <div className="row">
      <div className="col">
        <input
          type="text"
          value={textInput}
          onChange={(e) => setTextInput(e.target.value)}
        />
        <button onClick={handleSendMessage}>Send Message</button>
      </div>
      <div className="col">
        <button onClick={handleGetMessage}>Get last 10 min's messages</button>
        <table>
          <caption>Last 10 min messages</caption>
          <thead>
            <tr>
              <th scope="col">OrderNumber</th>
              <th scope="col">SentDateTime</th>
              <th scope="col">Text</th>
            </tr>
          </thead>
          <tbody>
            {lastMessages?.map((m) => (
              <tr key={m.id}>
                <td>{m.orderNumber}</td>
                <td>{m.sentDateTime}</td>
                <td>{m.text}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <div className="col">
        <table>
          <caption>CurrentMessage</caption>
          <thead>
            <tr>
              <th scope="col">OrderNumber</th>
              <th scope="col">SentDateTime</th>
              <th scope="col">Text</th>
            </tr>
          </thead>
          <tbody>
            {curMessages?.map((m) => (
              <tr key={m.id}>
                <td>{m.orderNumber}</td>
                <td>{m.sentDateTime}</td>
                <td>{m.text}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default App;
