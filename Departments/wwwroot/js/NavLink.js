document.addEventListener("DOMContentLoaded", (event) => {    

    let navLink = document.querySelector(".navContainerLink");    
    //let btnSendForm = document.querySelector('[data-btn]');
    //let check = true;

    function attachButtonHandler() {
        let submitButton = document.getElementById('profile-form');
        if (submitButton) {
            submitButton.addEventListener('submit', function (event) {
                event.preventDefault();

                let url = "/ManageAccount/Profile"
                const form = document.getElementById('profile-form');
                
                const formData = new FormData(form);

                const email = document.querySelector("#Email").value;
                formData.append("Email", email);
                //formData.append("Email", formData.get("Email"));
                //formData.append("Email", formData.get("Phone"));


                fetch(url, {
                    method: "POST",
                    body: formData
                }).then(response => response.text())
                    .then(html => {
                        document.getElementById('partialContainer').innerHTML = html;
                    })
            });
        }
    }
    navLink.addEventListener("click", (event) => {
        event.preventDefault();

        let link = event.target;
        let target = link.dataset.target;
        
        let url = "/ManageAccount/" + target;
        fetch(url, {
            method: "GET"
        }).then(response => response.text())
            .then(html => {
                const tempDiv = document.getElementById('partialContainer');
                tempDiv.innerHTML = html;               

                // При обновлении partialView повесим обработчик на кнопку
                attachButtonHandler();

            });      
    });

    //if (check) {    

    //    btnSendForm.addEventListener("click", (event) => {
    //        event.preventDefault();

    //        let target = event.target.dataset.target;
    //        let url = "/ManageAccount/" + target;
    //        const formData = new FormData(document.querySelector("form"));

    //        fetch(url, {
    //            method: "POST",
    //            body: formData
    //        }).then(response => response.text())
    //            .then(html => {
    //                document.getElementById('partialContainer').innerHTML = html;

    //            })

    //    })
    //}
    
    

})


