let btn = document.getElementById("add-to-group")

if (btn != null) {
    btn.addEventListener('click', (e) => {
        let baseUrl = document.location.href.split('/chat')[0];
        let id = e.target.getAttribute('data-trans');
        let roomId = e.target.getAttribute('data-room');
        $.ajax({
            method: "get",
            url: `${baseUrl}/api/user/listfriend/${id}/${roomId}`,
        })
            .done(function (data) {
                let finalArray = []
                let form = document.getElementById('data-action');

                let x = data.map((e, i) => `
                <div class="d-flex align-items-center justify-content-around mb-2" style="border-bottom:0.5px solid gray">
                        <label for="select_${i}" class="d-flex align-items-center">
                            <img src="/images/${e.avatar}" style="width:50px;height:50px; border-radius:50%" />
                            <span class="ml-3 " style="font-size:20px">${e.name}</span>
                        </label>
                        <input id="select_${i}" style="margin-left:auto" name="users[]" value="${e.id}" class="checkbox_size mr-3" type="checkbox" />
                    </div>
                `).join(' ');

                form.innerHTML = x + `<input type = "hidden" name = "groupId"  value = "${roomId}" /> <input type="submit" class="btn btn-primary" value="Add" />`
            })
    })
}

//toast to show noftify

let successToast = document.getElementById("success-notify-toast");
let errorToast = document.getElementById("error-notify-toast");

if (successToast != null) {
    Toastify({
        text: successToast.innerHTML,
        offset: {
            x: 50,
            y: 100
        },

        className: "toast-success-notify",
        style: {
            background: "#00bc8c",
        }
    }).showToast();
}

if (errorToast != null) {
    Toastify({
        text: errorToast.innerHTML,
        offset: {
            x: 50,
            y: 100
        },
        className: "toast-error-notify",
        style: {
            background: "#e84c3d",
        },
    }).showToast();
}