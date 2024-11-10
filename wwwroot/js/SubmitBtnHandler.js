export { CheckFormValueChanges };

//Провереят изменения в форме, если значения не изменились, то кнопка отправки становится не доступна.
function CheckFormValueChanges() {
   let form = document.forms[0];
   let submitBtn = document.querySelector("#submitBtn");
   let initialFormValues = {};

   //получаем элементы формы и их значения
   for (let element of form) {
      if (element.type !== 'submit') {
         if (element.type === 'checkbox') {
            initialFormValues[element.id] = element.checked;
         } else {
            initialFormValues[element.id] = element.value;
         }
      }
   }
   //Добавление обработчика на изменение значений в форме
   form.addEventListener('input', UpdateSubmitButton)
   //Проверяет изменение значений формы
   function IsFormChanged() {
      for (let element of form) {
         if (element.type !== 'submit') {
            if (element.type === 'checkbox' && element.checked !== initialFormValues[element.id]) {
               return true;
            } else if (element.type !== 'checkbox' && element.value !== initialFormValues[element.id]) {
               return true;
            }
         }
      }
      return false;
   }
   //Изменяет состояние кнопки отправки формы
   function UpdateSubmitButton() {
      if (IsFormChanged()) {
         submitBtn.disabled = false;
      }
      else {
         submitBtn.disabled = true;
      }
   }
}