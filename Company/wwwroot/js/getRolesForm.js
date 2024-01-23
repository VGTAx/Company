document.addEventListener('DOMContentLoaded', () => {

   let navLink = document.querySelectorAll(".nav-link.admin");

   navLink.forEach(function (link) {
      link.addEventListener("click", (event) => {
         let link = event.target;
         let url = link.dataset.target;
         GetManageAccountPartialView(event, url);
      });
   });
})

//возвращает PartialView на страницу в профиле
function GetManageAccountPartialView(event, partialView) {
   event.preventDefault();

   let url = partialView;
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
      // При обновлении partialView вешаем обработчик на отправку формы
      SetSubmitButtonFormHandler();
      GetUser();
   }).catch(error => {
      console.error(error);
      window.location.href = "/Account/Login";
   })
}

function SetSubmitButtonFormHandler() {
   let form = document.forms[0];
   if (form) {
      form.addEventListener('submit', event => {
         event.preventDefault();

         let url = form.action;
         let formData = new FormData(form);
         fetch(url, {
            method: 'POST',
            body: formData
         }).then(response => {
            if (response.ok) {
               return response.text()
                  .then(html => {
                     const tempDiv = document.querySelector('#partialContainer');
                     tempDiv.innerHTML = html;
                     GetUser();
                  })
            }
            if (response.status == 400) {
               return response.json()
                  .then(data => {
                     ShowModelErrors(data);
                  })
            }
         }).catch(error => {
            console.error(error);
         });
      });
   }
}

//Отображение ошибок модели и др.
function ShowModelErrors(modelErrors) {
   if (modelErrors) {

      let modelErrorMessages = document.querySelectorAll("span");
      modelErrorMessages.forEach(function (error) {
         error.textContent = ""
      });

      let textError = "";
      for (let key in modelErrors) {
         if (modelErrors.hasOwnProperty(key)) {
            textError += modelErrors[key].join(", ");
         }
      }
      document.querySelector(`#formMessage`).textContent = textError;
   }
}

function GetUser() {
   let users = document.querySelectorAll(".user-info");
   users.forEach(function (btn) {
      btn.addEventListener('click', event => {
         let userId = event.target.getAttribute('id');
         let url = event.target.dataset.target + `/${userId}`;
         fetch(url, {
            method: "GET"
         }).then(response => {
            if (response.ok) {
               return response.text()
                  .then(html => {
                     const tempDiv = document.querySelector('#partialContainer');
                     tempDiv.innerHTML = html;
                     SetSubmitButtonFormHandler()
                  })
            }
         }).catch(error => {
            console.error(error);
         });
      })
   })
}