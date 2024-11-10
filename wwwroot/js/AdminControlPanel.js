import { CheckFormValueChanges } from './SubmitBtnHandler.js'

document.addEventListener('DOMContentLoaded', () => {
   let navLink = document.querySelectorAll(".nav-link.admin");

   navLink.forEach(function (link) {
      link.addEventListener("click", (event) => {
         let link = event.target;
         let requestUrl = link.href;
         GetControlPanelSection(event, requestUrl);
      });
   });
})

//возвращает раздел панели управления администратора в виде PartialView 
function GetControlPanelSection(event, requestUrl) {
   event.preventDefault();

   let url = requestUrl;
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
      // При обновлении partialView вешаем обработчик на кнопку Инфо для получения пользователя
      GetUserInfo();
   }).catch(error => {
      console.error(error);
      window.location.href = "/Account/Login";
   })
}
//устанавливает обработчик на кнопку отправки формы
function SetSubmitButtonFormHandler() {
   let form = document.forms[0];
   if (form) {

      CheckFormValueChanges(); 
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
                     GetUserInfo(); //вешаем обработчик на кнопку Инфо каждого пользователя
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
//Отображение ошибок формы
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
//Обработчик на кнопку Инфо каждого пользователя в списке для его получения.
function GetUserInfo() {
   let users = document.querySelectorAll(".user-info");
   users.forEach(function (btn) {
      btn.addEventListener('click', event => {
         event.preventDefault();
         let userId = event.target.getAttribute('id');
         let url = event.target.href + `/${userId}`;
         fetch(url, {
            method: "GET"
         }).then(response => {
            if (response.ok) {
               return response.text()
                  .then(html => {
                     const tempDiv = document.querySelector('#partialContainer');
                     tempDiv.innerHTML = html;
                     SetSubmitButtonFormHandler(); //после получения профиля пользователя вешаем обработчик на отправку формы для сохранения изменений
                  })
            }
         }).catch(error => {
            console.error(error);
         });
      })
   })
}