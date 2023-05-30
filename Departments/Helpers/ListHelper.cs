using Company.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using MySqlX.XDevAPI.Common;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;

namespace Company.Helpers
{
    public static class ListHelper
    {
        
        public static IHtmlContent RenderSubDepartments(IHtmlHelper html, int? departmentId, List<DepartmentNumberPoco> departments)
        {
           
            var subdepartments = departments.Where(d => d.ParentID == departmentId);
            var ul = new TagBuilder("ul");
            

            if (subdepartments.Any())
            {                
                foreach (var dep in subdepartments)
                {
                    var li = new TagBuilder("li");
                    //var a = new TagBuilder("a");
                    //var actionUrl = urlHelper.Action("DetailsDepartment", "Department");

                     
                    //a.InnerHtml.Append(dep.DepartmentName!);
                    //a.MergeAttribute("href", actionUrl);
                    //a.Attributes["class"] = "nav-link text-dark";
                    //li.InnerHtml.AppendHtml();

                    //if (IsButtonRequired(dep,departments))
                    //{
                    //    var form = CreateForm(dep);
                    //    li.InnerHtml.AppendHtml(form);
                    //}                    
                    li.InnerHtml.AppendHtml(RenderSubDepartments(html, dep.ID, departments));
                    ul.InnerHtml.AppendHtml(li);
                }  
                
            }         
            return ul;
        }

        private static bool IsButtonRequired(DepartmentNumberPoco department, List<DepartmentNumberPoco> departments)
        {
            var check = departments.FirstOrDefault(d => d.ParentID == department.ID);
            return check == null;
        }

        public static IHtmlContent CreateForm(DepartmentNumberPoco department)
        {
            var form = new TagBuilder("form");
            form.MergeAttribute("action", "/Department/Create");
            form.MergeAttribute("method", "post");

            var departmentIdTag = new TagBuilder("input");
            departmentIdTag.MergeAttribute("type", "hidden");
            departmentIdTag.MergeAttribute("name", "DepartmentID");
            departmentIdTag.MergeAttribute("value", department.ID.ToString());

            var submissionBtn = new TagBuilder("button");
            submissionBtn.MergeAttribute("type", "submit");
            submissionBtn.MergeAttribute("value", "Test");
            submissionBtn.InnerHtml.Append("Добавить");

            form.InnerHtml.AppendHtml(departmentIdTag);
            form.InnerHtml.AppendHtml(submissionBtn);

            return form;
        }
    }


    
}
