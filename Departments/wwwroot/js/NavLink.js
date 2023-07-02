document.addEventListener("DOMContentLoaded", (event) => {    

    let navLink = document.querySelector(".navContainerLink");    
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

                fetch(url, {
                    method: "POST",
                    body: formData
                }).then(response => response.text()
                    .then(html => {
                        const tempDiv = document.getElementById('partialContainer');
                        tempDiv.innerHTML = html;
                    }));
                
                let updatedData = document.querySelector("#Name").value;

                let dataUpdatedEvent = new CustomEvent('dataUpdated', { detail: { data: updatedData } });
                // Отправка события
                document.dispatchEvent(dataUpdatedEvent);                
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
        }).then(response => {
            if (!response.ok) {
                throw new Error("Ошибка авторизации, пройдите авторизацию");
            }
            return response.text()
        }).then(html => {
                const tempDiv = document.getElementById('partialContainer');
                tempDiv.innerHTML = html;       
                // При обновлении partialView вешаем обработчик на отправку формы
                attachButtonHandler();

        }).catch(error => {
            window.location.href = "/Account/Login";
        })
           
    });    

})


    //.catch(error => {
    //    window.location.href = '/Account/Login';
    //});      