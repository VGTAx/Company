# Company

Company is a web application that is a system for managing users and company departments. The application implements a hierarchical departmental structure. For example, “Logistics Department” with subdepartments “Warehouse” and “Delivery Department”. This hierarchical structure allows departments to be logically organized and managed, as well as linking users to the appropriate departments.<br>

The application is developed on the **ASP.NET** platform using the **C#** programming language.<br>
The application uses **MySQL** DBMS to store data, which ensures reliability and efficiency when working with large amounts of data.
To work with this database, ORM is used - **Entity Framework**. <br>
The client side of the application is developed using web development languages and technologies such as **CSS**, **HTML** and **JavaScript**. <br>

**The application provides the following functionality:**
<ol>
   <li>User registration with email confirmation.</li>
   <li>Changing user credentials.</li>
   <li>Delimitation of access to functions depending on the user role.</li>
   <li>Ability to view, add, edit and delete employee data.</li>
</ol>

**Installation Instructions:**
<ol>
   <li>Clone the repository</li>
   <li>Go to the project directory</li>
   <li>Make sure you have the .NET SDK installed. If not, install it from the official .NET site: https://dotnet.microsoft.com/download/dotnet</li>
   <li>Change the appsettings.json settings file:
     <ul>
       <li>Update the database connection string in the file with your MySQL database credentials</li>
       <li>Update your connection details to your email service's SMTP server</li>
     </ul>
   </li>
  <li>Restore the project dependencies using the command: <b><i>dotnet restore</i></b></li>
   <li>Migrate to create the database and fill it with initial data: <b><i>dotnet ef database update</i></b></li>
   <li>Run the App using the command: <b><i>dotnet run</i></b></li>
</ol>
