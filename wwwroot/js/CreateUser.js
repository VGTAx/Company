import { CheckFormValueChanges } from './SubmitBtnHandler.js'

document.addEventListener('DOMContentLoaded', () => {

   let name = document.querySelector("#Name");
   let surname = document.querySelector("#Surname");
   let number = document.querySelector("#Number");
   let age = document.querySelector("#Age");   
   
   name.addEventListener("keypress", event => {
      EventHandlerCheckLetters(event)
   });
   surname.addEventListener("keypress", event => {
      EventHandlerCheckLetters(event)
   });
   age.addEventListener("keypress", (event) => {
      let codeKey = event.which || event.keyCode;
      let inputValue = String.fromCharCode(codeKey);
      if (!/^\d$/.test(inputValue) || !/^\d{0,1}$/.test(event.target.value)) {
         event.preventDefault();
      }
   });   
   number.addEventListener('focus', (event) => {
      if (event.target.value == "") {
         event.target.value = "+";
      }
   });
   number.addEventListener("keypress", (event) => {

      if (!event.target.value.startsWith("+") && event.target.value != "") {
         event.target.value = "+" + event.target.value;
      }

      let codeKey = event.which || event.keyCode;
      let inputValue = String.fromCharCode(codeKey);

      if (!/^\d$/.test(inputValue)) {
         event.preventDefault();
      }

      if (inputValue.includes("+") || !/^[\d+]{1,12}$/.test(event.target.value)) {
         event.preventDefault();
      }
   });

   SetSubmitButtonFormHandler();
})
//Обработчик вводимых значений формы
function EventHandlerCheckLetters(event) {
   let codeKey = event.which || event.keyCode;
   let inputValue = String.fromCharCode(codeKey);

   if (!/^[a-zA-Zа-яА-Я-]$/.test(inputValue)) {
      event.preventDefault();
   }
   if ((event.target.value.includes("-") && inputValue == "-")
      || !/^[a-zA-Zа-яА-Я-]{0,20}$/.test(event.target.value)) {
      event.preventDefault();
   }
}
//Обработчик отправки формы
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
               if (response.redirected) {
                  window.location.href = response.url
               }
            }
            if (response.status == 400) {
               return response.json()
                  .then(modelErrors => {
                     ShowModelErrors(modelErrors);
                  })
            }
         }).catch(error => {
            console.error(error);
         });
      });
   }
}
//Обработчик ошибок 
function ShowModelErrors(modelErrors) {
   if (modelErrors) {
      //удаление сообщений ошибок
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