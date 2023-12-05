$(document).ready(function () {
    // Assuming your chat history has the class "chat-history"
    var chatHistory = $(".chat-history");

    // Set the scrollTop property to the maximum value
    chatHistory.scrollTop(chatHistory.prop("scrollHeight"));
});
const currentTime = new Date();
const formattedTime = currentTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();
// handle new message notify
connection.on("NotifyNewMessage", (check) => {
    if (check.length != 0) {
        let notify = document.querySelector('.message_count')
        notify.style.display = "block";
        notify.innerHTML = check.length

        let no = document.getElementById('notify-new-message')
        let data = check.map((e) => `
       <a href="/chat/private/${e[0].user.userName}">
        <div class="media" style="border-bottom:solid 0.5px gray">
                            <img src="/images/${e[0].user.avatar}" alt="User Avatar" class="img-size-50 mr-3 img-circle">
                            <div class="media-body" style="width:250px">
                                <h3 class="dropdown-item-title" stye="foent-size:22px">
                                    ${e[0].user.userName}</h3>
                                <p class="text-sm text-in-p">${e[0].message.content}</p>
                                <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
                            </div>
                        </div>
                         </a>
                        `
        ).join();
        no.innerHTML = data
    }
})

// handle check olnine status

connection.on("CheckOnlineResult", (data) => {
    console.log(data)
    data.forEach(d => {
        var handle = document.querySelector(`[data-name="${d.name}"]`)
        if (d.isOnline) {
            handle.classList.remove('online')
            handle.classList.remove('offline')
            handle.classList.add('online')
            handle.innerHTML = "  Online"
        } else {
            handle.classList.remove('online')
            handle.classList.remove('offline')
            handle.classList.add('offline')
            handle.innerHTML = "  Offline"
        }
    })
})

// handle recieve message
connection.on("ReceiveMessage", (user, message) => {
    const li = document.createElement("li");
    let avatar = document.getElementById('rootUrl').getAttribute('data-send')

    let senid = document.getElementById('reciveId').value
    let redid = document.getElementById('send_Id').value

    $.get(`${window.location.href.split('/chat')[0]}/api/chat/up/${senid}/${redid}`);
    let text = `  <li class="clearfix">
                                            <div class="message-data">
                                                <span class="message-data-time">${formattedTime} , Today</span>

                                            </div>
                                            <img style="width:40px;height:40px; border-radius:50%" src="/images/${avatar}" alt="avatar">
                                            <div class="message my-message">${message}</div>
                                        </li>`
    li.className = "clearfix"
    li.innerHTML = text
    document.getElementById("messagesList").appendChild(li);
    var chatHistory = $(".chat-history");

    // Set the scrollTop property to the maximum value
    chatHistory.scrollTop(chatHistory.prop("scrollHeight"));
})
connection.start().catch(err => console.error(err.toString()));

// send message to group

const sendMessageGroup = async () => {
    let roomId = $('#room-data').val();
    let message = $('#room-mess').val();
    let send_Id = $('#send_Id').val();

    await connection.invoke("SendMessageToGroup", roomId, send_Id, message)
        .then(() => {
            document.getElementById("room-mess").value = "";

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
        .catch((err) => { console.log(err) });
}

function sendMessage() {
    // Assuming connection is properly initialized before this point

    const toUser = document.getElementById("userIdInput").value;
    const message = document.getElementById("messageInput").value;
    const id = document.getElementById("reciveId").value;

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
    connection.invoke("NotifyMessage", name.trim(), id.trim()).catch(err => console.error(err.toString()));
}

$(document).ready(() => {
    setTimeout(function () {
        checkMess()
    }, 2000);
})

// check status on line

//function to invok hub check

const checkOnlineStatus = () => {
    let list = []
    var data = document.querySelectorAll('.name-check-online')
    data.forEach(e => {
        list = [...list, e.innerHTML.trim()]
    });

    connection.invoke("CheckOnline", list).catch((err) => { console.log(err) });
}

setInterval(checkOnlineStatus, 10000)

// set enter sent message
let messageCheck = document.getElementById("messageInput")
let roomMess = document.getElementById("room-mess")

if (messageCheck != null) {
    messageCheck.addEventListener('keyup', (e) => {
        if (e.keyCode == 13) {
            if (messageCheck.value.trim().length != 0) {
                sendMessage()
            }
        }
    })
}

if (roomMess != null) {
    roomMess.addEventListener('keyup', (e) => {
        if (e.keyCode == 13) {
            if (roomMess.value.trim().length != 0) {
                sendMessageGroup()
            }
        }
    })
}

let joinData = $("#join-data").val();
if (joinData != null) {
    connection.invoke("JoinGroup", joinData)
        .then(() => console.log("join successed"))
        .catch(err => console.log(err))
}