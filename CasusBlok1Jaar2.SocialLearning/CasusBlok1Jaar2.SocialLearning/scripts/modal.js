// Get the modal
var modal = document.getElementById("userModal");

var modal1 = document.getElementById("channelModal");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

var span1 = document.getElementsByClassName("close")[1];

// When the user clicks on the button, open the modal
function btnClick() {
    modal.style.display = "block";
}

function channelClick() {
    modal1.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

span1.onclick = function () {
    modal1.style.display = "none";
}


// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

window.onclick = function (event) {
    if (event.target == modal1) {
        modal1.style.display = "none";
    }
}