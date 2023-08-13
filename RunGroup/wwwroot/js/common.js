function openModal(modalId) {
    var modal = document.getElementById(modalId);
    modal.style.display = "block";
}

function closeModal(modalId) {
    var modal = document.getElementById(modalId);
    modal.style.display = "none";
}

function triggerClickButton(buttonId) {
    var button = document.getElementById(buttonId);
    button.click();
}

function submitProfileImage() {
    var submitImageButton = document.getElementById("submit-image-button");
    submitImageButton.click();
}