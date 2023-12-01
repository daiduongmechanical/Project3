$(document).ready(function () {
    // Assuming your chat history has the class "chat-history"
    var chatHistory = $(".chat-history");

    // Set the scrollTop property to the maximum value
    chatHistory.scrollTop(chatHistory.prop("scrollHeight"));
  
});





const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

connection.on("NotifyNewMessage", (check) => {
    console.log(check)
    if (check.length != 0) {
        let notify = document.querySelector('.message_count')
        notify.style.display = "block";
        notify.innerHTML = check.length

        let no = document.getElementById('notify-new-message')
        let data = check.map((e) => `
       <a href="/chat/${e[0].user.userName}">
        <div class="media" style="border-bottom:solid 0.5px gray">
                            <img src="/images/${e[0].user.avatar}" alt="User Avatar" class="img-size-50 mr-3 img-circle">
                            <div class="media-body" style="width:250px">
                                <h3 class="dropdown-item-title" stye="foent-size:22px">
                                    ${e[0].user.userName}
                                </h3>
                                <p class="text-sm text-in-p">${e[0].message.content}</p>
                                <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
                            </div>
                        </div>
                         </a>
                        `

        ).join();

        no.innerHTML=data

    }
})
// handle recieve message
connection.on("ReceiveMessage", (user, message) =>
{
    const li = document.createElement("li");
    let avatar = document.getElementById('rootUrl').getAttribute('data-send')
    let root = document.getElementById('rootUrl').getAttribute('data-root-url')

console.log(1)


    let text =`  <li class="clearfix">
                                            <div class="message-data">
                                                <span class="message-data-time">10:12 AM, Today</span>
                                               
                                            </div>
                                            <img style="width:50px;height:50px; border-radius:50%" src="${root}images/${avatar}" alt="avatar">
                                            <div class="message my-message">${message}</div>
                                        </li>`
    li.className = "clearfix"
    li.innerHTML=text
    document.getElementById("messagesList").appendChild(li);
    var chatHistory = $(".chat-history");

    // Set the scrollTop property to the maximum value
    chatHistory.scrollTop(chatHistory.prop("scrollHeight"));
})
connection.start().catch(err => console.error(err.toString()));



function sendMessage() {
    // Assuming connection is properly initialized before this point

    const toUser = document.getElementById("userIdInput").value;
    const message = document.getElementById("messageInput").value;
    const id = document.getElementById("reciveId").value;

    // Get the current time for the timestamp
    const currentTime = new Date();
    const formattedTime = currentTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

    connection.invoke("SendMessage", toUser, message, id)
        .then(() => {
            document.getElementById("messageInput").value = "";

            // Create a new message element with the current timestamp
            let newDiv = document.createElement("li");
            newDiv.className = "clearfix";
            newDiv.innerHTML = `
                <div class="message-data text-right">
                    <span class="message-data-time">${formattedTime}</span>
                </div>
                <div class="message other-message float-right">${message}</div>
            `;

            document.getElementById("messagesList").appendChild(newDiv);
            var chatHistory = $(".chat-history");

            // Set the scrollTop property to the maximum value
            chatHistory.scrollTop(chatHistory.prop("scrollHeight"));
        })
        .catch(err => {
            Toastify({
                text: "You just sent 5 message for unfriend",
                offset: {
                    x: 50, // horizontal axis - can be a number or a string indicating unity. eg: '2em'
                    y: 100 // vertical axis - can be a number or a string indicating unity. eg: '2em'
                },
                backgroundColor: "red",
               
            }).showToast();
        });
}


//notify when have new message
const checkMess = () => {
    let name = document.getElementById('avatar-container').getAttribute('data-send');
    let id = document.getElementById('avatar-container').getAttribute('data-info');
    connection.invoke("NotifyMessage",name.trim(), id.trim()).catch(err => console.error(err.toString()));

}


$(document).ready( () => {
  setTimeout(function () {
      checkMess()
   }, 2000);

   
})


