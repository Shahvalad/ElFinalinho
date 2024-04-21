var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

document.getElementById("sendButton").disabled = true;

var recipientUsername = null;
var recipientProfilePicture = null;
document.querySelectorAll('.friend').forEach(function (friend) {
    friend.addEventListener('click', function () {
        document.querySelectorAll('.friend').forEach(function (friend) {
            friend.classList.remove('selected');
        });

        this.classList.add('selected');

        recipientProfilePicture = this.getAttribute('data-profilepicture');

        recipientUsername = this.getAttribute('data-username');

        document.querySelector('.chat-messages-head .user-item__name').textContent = recipientUsername;
        document.querySelector('.chat-messages-head .user-item__avatar img').src = '/images/ProfilePictures/' + recipientProfilePicture;
        document.getElementById("sendButton").disabled = false;
        fetchChatHistory(currentUser, recipientUsername);
        var newMessageSpan = document.getElementById("new-message-" + this.getAttribute('data-username'));
        if (newMessageSpan) {
            newMessageSpan.innerHTML = '';
        }
    });

});

connection.on("ReceiveMessage", function (user, message, unreadMessageCount) {
    var selectedFriend = document.querySelector('.friend.selected');
    if (selectedFriend && user === selectedFriend.getAttribute('data-username')) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var messageItem = document.createElement("div");
        var avatarDiv = document.createElement("div");
        var textDiv = document.createElement("div");
        user = currentUser;
        messageItem.className = 'messages-item --your-message';

        avatarDiv.className = 'user-item__avatar';

        avatarDiv.innerHTML = '<img src="/images/ProfilePictures/' + recipientProfilePicture + '" alt="user">';

        textDiv.className = 'messages-item__text';
        textDiv.textContent = msg;

        messageItem.appendChild(avatarDiv);
        messageItem.appendChild(textDiv);

        document.getElementById("messagesList").appendChild(messageItem);
        var audio = new Audio('/Sounds/NewMessage.mp3');
        audio.play();

    } else {
        var newMessageSpan = document.getElementById("new-message-" + user);
        if (newMessageSpan) {
            newMessageSpan.innerHTML = '<i class="fas fa-exclamation-circle new-message-icon"></i>';
        }
        var audio = new Audio('/Sounds/NewMessage.mp3');
        audio.play();
    }
});

document.querySelector('.chat-messages-footer form').addEventListener("submit", function (event) {
    event.preventDefault();
    var user = currentUser;
    var message = document.getElementById("messageInput").value;

    var selectedFriend = document.querySelector('.friend.selected');
    if (selectedFriend) {
        recipientUsername = selectedFriend.getAttribute('data-username');
    }
    connection.invoke("SendMessage", user, recipientUsername, message).catch(function (err) {
        return console.error(err.toString());
    });
    var messageItem = document.createElement("div");
    var textDiv = document.createElement("div");

    messageItem.className = 'messages-item --friend-message';

    textDiv.className = 'messages-item__text';
    textDiv.textContent = message;

    messageItem.appendChild(textDiv);

    document.getElementById("messagesList").appendChild(messageItem);

    document.getElementById("messageInput").value = '';
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    var firstFriend = document.querySelector('.friend');
    if (firstFriend) {
        firstFriend.click();
    }


}).catch(function (err) {
    return console.error(err.toString());
});

function fetchChatHistory(currentUser, selectedFriend) {
    fetch('/Chats/LoadChat?recipientUsername=' + selectedFriend)
        .then(response => response.text())
        .then(data => {
            document.getElementById("messagesList").innerHTML = data;
        })
        .catch(error => console.error('Error:', error));
}

window.onload = function () {
    var firstFriend = document.querySelector('.friend');
    if (firstFriend) {
        firstFriend.click();
    }
};
