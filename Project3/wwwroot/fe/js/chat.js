$(document).ready(function () {
    // Assuming your chat history has the class "chat-history"
    var chatHistory = $(".chat-history");

    // Set the scrollTop property to the maximum value
    chatHistory.scrollTop(chatHistory.prop("scrollHeight"));
});

//join group when page loaded
$(document).ready(() => {
    let checkToJoin = document.getElementById("join-group-now");
    if (checkToJoin != null) {
        setTimeout(() => {
            connection.invoke("JoinGroup", checkToJoin.value)
                .then(() => console.log("join successed"))
                .catch(err => console.log(err))
        }, 1000)
    }
});
const currentTime = new Date();
const formattedTime = currentTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();
// handle new message notify
connection.on("NotifyNewMessage", (check) => {
    if (check.length != 0) {
        let notify = document.querySelector('.message_count')
        notify.style.display = "block";

        notify.innerHTML = Number(notify.innerHTML.trim()) + check.length

        let no = document.getElementById('notify-new-message')
        let messCover = document.createElement("div");
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
        messCover.innerHTML = data
        no.appendChild(messCover);
    }
})

//notify new group message
connection.on("NewGroupMess", (data, groupId) => {
    let notify = document.querySelector('.message_count')
    let current = notify.innerHTML;
    let name = groupId.split('_s')[0]

    notify.innerHTML = Number(notify.innerHTML.trim()) + 1;
    notify.style.display = "block";

    let no = document.getElementById('notify-new-message')
    let messCover = document.createElement("div");
    let newMess = ` <a href="/chat/group/${groupId}">
        <div class="media" style="border-bottom:solid 0.5px gray">
                            <img src="/images/room.png" alt="User Avatar" class="img-size-50 mr-3 img-circle">
                            <div class="media-body" style="width:250px">
                                <h3 class="dropdown-item-title" stye="foent-size:22px">
                                    Group : ${name}</h3>
                                <p class="text-sm text-in-p">${data.content}</p>
                                <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
                            </div>
                        </div>
                         </a> `
    let currentData = { group: groupId, data: data };
    messCover.innerHTML = newMess
    no.appendChild(messCover);

    // handle add data to session
    if (sessionStorage.getItem("group_notice") == null) {
        sessionStorage.setItem("group_notice", JSON.stringify([currentData]))
    } else {
        let count = 0;
        let position = 0;

        let getData = JSON.parse(sessionStorage.getItem("group_notice"));
        getData.forEach((x, i) => {
            if (x.group == groupId) {
                count++;
                position = i;
            }
        })
        if (count == 0) {
            getData = [...getData, currentData];
        } else {
            getData[position] = currentData
        }
        sessionStorage.setItem("group_notice", JSON.stringify(getData))
    }
})

//handle recieve newmess from group
connection.on("ReceiveMessageGroup", (name, avatar, message, GroupId) => {
    let li = document.createElement("li");

    let dataAddText = ` <li class="clearfix">
                                            <div class="message-data">
                                                <span class="message-data-time">${formattedTime} , Today</span>

                                            </div>
                                            <img style="width:40px;height:40px; border-radius:50%" src="/images/${avatar}" alt="avatar">
                                            <div class="message my-message d-flex flex-column align-items-lg-start pr-2" style="width:fit-content ; max-width:30vw" >
                                        <small>${name}</small>
                                        <span>${message}</span>
                                        </div>
                                        </li> `

    li.innerHTML = dataAddText

    document.getElementById("messagesList").appendChild(li);

    var chatHistory = $(".chat-history");

    // Set the scrollTop property to the maximum value
    chatHistory.scrollTop(chatHistory.prop("scrollHeight"));
})

// handle check olnine status

connection.on("CheckOnlineResult", (data) => {
    data.forEach(d => {
        var handle = document.querySelector(`[data-name="${d.name}"]`)
        if (d.isOnline) {
            handle.classList.remove('offline')
            handle.classList.add('online')
            handle.innerHTML = "  Online"
        } else {
            handle.classList.remove('online')

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

    $.get(`${window.location.href.split('/chat')[0]} /api/chat / up / ${senid} /${redid}`);
    let text = `  <li class="clearfix">
                                            <div class="message-data">
                                                <span class="message-data-time">${formattedTime} , Today</span>

                                            </div>
                                            <img style="width:40px;height:40px; border-radius:50%" src="/images/${avatar}" alt="avatar">
                                            <div class="message my-message">${message}</div>
                                        </li>`

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

//cation after 2s page load successed

$(document).ready(() => {
    setTimeout(function () {
        checkMess()
    }, 1000);
})

$(document).ready(
    () => {
        setTimeout(function () {
            if (sessionStorage.getItem("group_notice") != null) {
                let data = JSON.parse(sessionStorage.getItem("group_notice"));

                if (data.length != 0) {
                    let notify = document.querySelector('.message_count');

                    notify.innerHTML = Number(notify.innerHTML.trim()) + data.length;
                    notify.style.display = "block";

                    let no = document.getElementById('notify-new-message')
                    data.forEach(x => {
                        let messCover = document.createElement("div");
                        let newMess = ` <a href="/chat/group/${x.group}">
        <div class="media" style="border-bottom:solid 0.5px gray">
                            <img src="/images/${x.data.user.avatar}" alt="User Avatar" class="img-size-50 mr-3 img-circle">
                            <div class="media-body" style="width:250px">
                                <h3 class="dropdown-item-title" stye="foent-size:22px">
                                    Group : ${x.group.split('_s')[0]}</h3>
                                <p class="text-sm text-in-p">${x.data.content}</p>
                                <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
                            </div>
                        </div>
                         </a> `
                        messCover.innerHTML = newMess
                        no.appendChild(messCover);
                    })
                }
            }
        }, 1100);
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

//check account is onile each 5s
setInterval(checkOnlineStatus, 5000)

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

//remove room notify

let checkRooomAccess = document.getElementById('checkAccessGroup');

if (checkRooomAccess != null) {
    console.log(1)
    let getSeesionData = sessionStorage.getItem("group_notice");
    let position = 10000;
    if (getSeesionData != null) {
        let data = JSON.parse(getSeesionData);

        data.forEach((e, i) => {
            if (e.group == checkRooomAccess.value) {
                position = i;
            }
        });

        if (position != 10000) {
            data.splice(position, 1);
            sessionStorage.setItem("group_notice", JSON.stringify(data))
        }
    }
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