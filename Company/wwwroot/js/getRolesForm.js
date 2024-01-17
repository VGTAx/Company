document.addEventListener('DOMContentLoaded', (event) => {

   let navLink = document.querySelectorAll(".nav-link.admin");

   navLink.forEach(function (link) {
      link.addEventListener("click", (event) => {
         let link = event.target;
         let partialView = link.dataset.target;
         getManageAccountPartialView(event, partialView);
      });
   });

   function attachButtonFormHandler() {
      let form = document.querySelector('[data-form]');
      if (form) {
         form.addEventListener('submit', event => {
            event.preventDefault();

            let partialView = form.dataset.form;
            let url = "/Admin/" + partialView;            
            
            const selectedRoles = [];
            document.querySelectorAll('input[type="checkbox"][name="selectedRoles"]:checked')
               .forEach(function (checkbox) {
                  selectedRoles.push(checkbox.value);
               });

            let users = {
               Id: document.querySelector("#userId").getAttribute("value"),
            };       
            const dataToSend = {
               user: users,
               selectedRoles: selectedRoles,               
            }
            fetch(url, {
               method: 'POST',
               headers: {
                  'Content-Type': 'application/json'
               },
               body: JSON.stringify(dataToSend)
            }).then(response => {
               if (response.ok) {
                  return response.text()
                     .then(html => {
                        const tempDiv = document.querySelector('#partialContainer');
                        tempDiv.innerHTML = html;
                        attachButtonFormHandler();
                        getUser();
                     })
               }
               if (response.status == 400) {
                  return response.json()
                     .then(data => {
                        getPartialViewErrors(data, partialView, userId);
                     })
               }

            }).catch(error => {
               console.error(error);
            });
         });
      }
   }

   //возвращает PartialView на страницу в профиле
   function getManageAccountPartialView(event, partialView) {
      event.preventDefault();

      let url = "/Admin/" + partialView;
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
         if (partialView == "UserList") {
            getUser();
         }
      }).catch(error => {
         console.error(error);
         window.location.href = "/Account/Login";
      })
   }

   //Обработка ошибок Для PartialView DeletePersonData
   function getPartialViewErrors(data, partialView, id) {
      if (data) {
         for (let key in data) {
            if (data.hasOwnProperty(key)) {

               let textError = data[key].join(", ");
               let url = "/Admin/" + partialView + "/" + id;
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
                  //document.querySelector("#Span" + `${key}`).textContent = textError;
                  document.querySelector("#access").textContent = textError;
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

   function getUser() {
      let users = document.querySelectorAll(".user-info");
      users.forEach(function (btn)  {
         btn.addEventListener('click', event => {
            let userId = event.target.getAttribute('id');
            let url = `/Admin/AccessSettings?id=${userId}`;
            fetch(url, {
               method: "GET"
            }).then(response => {
               if (response.ok) {
                  return response.text()
                     .then(html => {
                        const tempDiv = document.querySelector('#partialContainer');
                        tempDiv.innerHTML = html;
                        attachButtonFormHandler();
                        getUser();
                     })
               }
            }).catch(error => {
               console.error(error);
            });
         })
      })
   }
})