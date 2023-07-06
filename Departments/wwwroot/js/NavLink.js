document.addEventListener("DOMContentLoaded", (event) => {    

    let navLink = document.querySelector(".navContainerLink");    
      
    function attachButtonFormHandler() {
        let form = document.querySelector('[data-form]');
        attachButtonDeleteDataFormHandler();
        if (form) {            
            form.addEventListener('submit', function (event) {
                event.preventDefault();
                let target = form.dataset.form;
                let url = "/ManageAccount/" + target;                      

                const formData = new FormData(form);

                if (form.dataset.form == "Profile" || form.dataset.form == "ChangeEmail") {
                    const email = document.querySelector("#Email").value;
                    formData.append("Email", email);
                }               
                
                fetch(url, {
                    method: "POST",
                    body: formData
                }).then(response => {
                    if (response.ok) {
                        return response.text()
                    }
                    if (response.status == 400) {
                        
                    }
                   
                })
                    .then(html => {
                        const tempDiv = document.getElementById('partialContainer');
                        if (form != "DeletePersonalData") {
                            tempDiv.innerHTML = html;
                            attachButtonDeleteDataFormHandler();
                            attachButtonFormHandler();
                        }                        
                        window.location.href = "/Department/Index";
                    }).catch(error => { 
                        console.error(error);
                        /*window.location.href = "/Account/Login";*/
                    });
                if (form.dataset.form == "Profile") {
                    let updatedData = document.querySelector("#Name").value;

                    let dataUpdatedEvent = new CustomEvent('dataUpdated', { detail: { data: updatedData } });
                    // Отправка события
                    document.dispatchEvent(dataUpdatedEvent);
                }           
            });            
        }
    }    
    //обработчик для формы, которая 
    function attachButtonDeleteDataFormHandler() {
        let deletePersonalDataForm = document.querySelector("#delete-personal-data"); 
        if (deletePersonalDataForm) {
            deletePersonalDataForm.addEventListener('click', function (event) {
                event.preventDefault();
                let target = deletePersonalDataForm.dataset.form;
                let url = "/ManageAccount/" + target;   
                fetch(url, {
                    method: "GET"
                }).then(response => {
                    if (!response.ok) {
                        throw new Error("Ошибка при попытке удалить учетную запись");
                    }
                    return response.text()
                }).then(html => {
                    const tempDiv = document.getElementById('partialContainer');
                    tempDiv.innerHTML = html;
                    // При обновлении partialView вешаем обработчик на отправку формы
                    attachButtonFormHandler();
                }).catch(error => {                    
                    console.error(error);
                    window.location.href = "/Account/Login";
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
        }).then(response => {
            if (!response.ok) {
                throw new Error("Ошибка авторизации, пройдите авторизацию");
            }
            return response.text()
        }).then(html => {
                const tempDiv = document.getElementById('partialContainer');
                tempDiv.innerHTML = html;       
                // При обновлении partialView вешаем обработчик на отправку формы
                attachButtonFormHandler();
        }).catch(error => {
            attachButtonFormHandler();
            console.error(error);
            window.location.href = "/Account/Login";
        })    
        
    });  
   
})


