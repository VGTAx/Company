import { CheckFormValueChanges } from './SubmitBtnHandler.js';

document.addEventListener("DOMContentLoaded", () => {
   let navLink = document.querySelectorAll(".nav-link.account");

   navLink.forEach(function (link) {
      link.addEventListener("click", (event) => {
         let link = event.target;
         let partialView = link.href;
         GetManageAccountPanelSection(event, partialView, SetSubmitButtonFormHandler);
      });
   });      
})

//возвращает раздел панели управления аккаунта в виде PartialView 
function GetManageAccountPanelSection(event, requestUrl, sub) {
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
      // При обновлении partialView вешаем обработчик на отправку формы
      sub();
   }).catch(error => {
      console.error(error);
      window.location.href = "/Account/Login";
   })
}
//Устанавливает обработчик на кнопку отправки формы
function SetSubmitButtonFormHandler() {
   let form = document.forms[0];
   if (form) {
      CheckFormValueChanges();
      form.addEventListener('submit', function (event) {
         event.preventDefault();

         let url = form.action;
         const formData = new FormData(form);

         if (form.action.includes("Profile") || form.action.includes("ChangeEmail")) {
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
                     //После получения PartialView вешаем обработчик на кнопку отправки формы
                     SetSubmitButtonFormHandler();
                  })
            }
            if (response.status == 400) {
               return response.json()
                  .then(data => {
                     ShowModelErrors(data)
                  });
            }
         }).catch(error => {
            console.error(error);
            window.location.href = "/Account/Login";
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

      for (let key in modelErrors) {
         if (modelErrors.hasOwnProperty(key)) {
            let textError = modelErrors[key].join(", ");
            document.querySelector(`span#${key}`).textContent = textError;
         }
      }
   }
} 