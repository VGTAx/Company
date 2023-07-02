document.addEventListener("DOMContentLoaded", (event) => {
    
    let btnSendForm = document.querySelector("[data-BtnSendForm]");

    btnSendForm.addEventListener("click", (event) => {
        event.preventDefault();

        let target = event.target.dataset.btnSendForm;
        let url = "/ManageAccount/" + target;
        const formData = new FormData(document.querySelector("form"));

        fetch(url, {
            method: "POST",
            body: formData
        }).then(response => response.text())
            .then(html => {
                document.getElementById('partial').innerHTML = html;
            })

    })

})


