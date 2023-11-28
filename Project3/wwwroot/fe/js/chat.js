const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

connection.on("ReceiveMessage", (user, message) => {
    const encodedMsg = `${user}: ${message}`;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().catch(err => console.error(err.toString()));

function sendMessage() {
    const toUser = document.getElementById("userIdInput").value;
    const message = document.getElementById("messageInput").value;
    const id = document.getElementById("reciveId").value; 
    console.log(id)
    connection.invoke("SendMessage", toUser, message,id).catch(err => console.error(err.toString()));

    document.getElementById("messageInput").value = ""
    let newDiv = document.createElement("li");
    newDiv.className = "clearfix";
    newDiv.innerHTML = ` <div class="message-data text-right">
<span class="message-data-time">10:10 AM, Today</span>
 </div>
<div class="message other-message float-right">${message} 
</div>`
    document.getElementById("messagesList").appendChild(newDiv)
}