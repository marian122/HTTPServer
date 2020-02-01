using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Reflection;

namespace SIS.MvcFramework
{
    public partial class ViewEngine : IViewEngine
    { 

        public string GetHtml(string template, object model)
        {
            var methodCode = PrepareCsharpCode(template);

            var code = $@"using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using SIS.MvcFramework;
namespace AppViewNamespace
{{
    public class AppViewCode : IView
    {{
        public string GetHtml(object model)
        {{
            var html = new StringBuilder();

            {methodCode}            

            return html.ToString();
        }}
    }}
}}";

            IView view = GetInstanceFromCode(code, model);

            string html = view.GetHtml(model);

            return html;
        }

        private IView GetInstanceFromCode(string code, object model)
        {
            var compilation =  CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(model.GetType().Assembly.Location));

            var libraries = Assembly.Load(new AssemblyName("netstandart")).GetReferencedAssemblies();

            foreach (var library in libraries)
            {
                compilation = compilation.AddReferences(
                    MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));
        }

        private string PrepareCsharpCode(string template)
        {
            return string.Empty;
        }
    }
}
