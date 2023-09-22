document.addEventListener("DOMContentLoaded", (event) => {

   let navLink = document.querySelectorAll(".nav-link.account");

   navLink.forEach(function (link) {
      link.addEventListener("click", (event) => {
         let link = event.target;
         let partialView = link.dataset.target;
         getManageAccountPartialView(event, partialView);
      });
   });

   function attachButtonFormHandler() {
      let form = document.querySelector('[data-form]');
      attachButtonDeleteDataFormHandler();
      if (form) {
         form.addEventListener('submit', function (event) {
            event.preventDefault();
            let partialView = form.dataset.form;
            let url = "/ManageAccount/" + partialView;

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
                     .then(html => {
                        const tempDiv = document.querySelector('#partialContainer');
                        if (form.id == "DeletePersonalData") {
                           location = "/";
                        }                        
                        tempDiv.innerHTML = html;
                        attachButtonDeleteDataFormHandler();
                        attachButtonFormHandler();                        
                     })
               }
               if (response.status == 400) {
                  return response.json()
                     .then(data => {
                        getPartialViewErrors(data, partialView)
                     });
               }
            }).catch(error => {
               console.error(error);
               window.location.href = "/Account/Login";
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
   //обработчик для формы удаления аккаунта 
   function attachButtonDeleteDataFormHandler() {
      let deletePersonalDataLink = document.querySelector("#delete-personal-data");
      if (deletePersonalDataLink) {
         deletePersonalDataLink.addEventListener('click', function (event) {
            event.preventDefault();
            let partialView = deletePersonalDataLink.dataset.form;
            getManageAccountPartialView(event, partialView);
         });
      }
   }
   
   //возвращает PartialView на страницу в профиле
   function getManageAccountPartialView(event, partialView) {
      event.preventDefault();

      let url = "/ManageAccount/" + partialView;
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
         console.error(error);
         window.location.href = "/Account/Login";
      })
   }
   //Обработка ошибок Для PartialView DeletePersonData
   function getPartialViewErrors(data, partialView) {
      if (data) {
         for (let key in data) {
            if (data.hasOwnProperty(key)) {

               let textError = data[key].join(", ");
               let url = "/ManageAccount/" + partialView;
               fetch(url, {
                  method: "GET"
               }).then(response => {
                  if (!response.ok) {
                     throw new Error("Ошибка авторизации, пройдите авторизацию");
                  }
                  return response.text()
               }).then(html => {
                  const tempDiv = document.querySelector('#partialContainer');
                  tempDiv.innerHTML = html;
                  document.querySelector("#Span" + `${key}`).textContent = textError;
                  // При обновлении partialView вешаем обработчик на отправку формы
                  attachButtonFormHandler();
               }).catch(error => {
                  console.error(error);
                  window.location.href = "/Account/Login";
               })
            }
         }
      }
   }    
})
