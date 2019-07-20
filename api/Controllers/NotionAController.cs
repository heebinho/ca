using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Model;
using System.Reflection.Emit;
using System.Reflection;

namespace API.Controllers
{
    /// <summary>
    /// Projects
    /// </summary>
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class NotionAController : ControllerBase
    {
        private readonly TnRContext context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ctx"></param>
        public NotionAController(TnRContext ctx)
        {
            context = ctx;
        }

        /// <summary>
        /// Dynamic type approach
        /// </summary>
        /// <param name="username">Admin or special value:' or 1=1 or ' </param>
        /// <param name="password">123456</param>
        /// <returns>list of projects</returns>
        /// <remarks>
        /// # Dynamic type
        /// 
        /// For a source code analyzer it's a challenge to analyze dynamic modules.
        /// This approach combines the possibility to create a type at runtime and a dynamic type.
        /// 
        /// C# 4.0 introduced a new type that avoids compile time type checking. (dynamic)
        /// 
        /// The input parameter can be tainted and needs to be checked and sanitized.
        /// 
        /// 
        /// 
        /// 
        /// | Username      | Password     |
        /// | ------------- | -------------|
        /// | Admin         | 123456       |
        /// | ' or 1=1 or ' |              |
        /// 
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("{username},{password}")]
        public IActionResult GetProjects([FromRoute] string username, [FromRoute] string password)
        {
            string sql = "select * from Users where Name='" + username + "' and Password='" + password + "' ";

            //Create a dynamic type at runtime with a TaintedSQL property (string)
            TypeBuilder tb = GetTypeBuilder();
            CreateProperty(tb, "TaintedSQL", typeof(string));
            TypeInfo typeInfo = tb.CreateTypeInfo();
            var dynType = typeInfo.AsType();

            //Create an instance of the dynamic type and set the tainted sql as property value
            dynamic obj = Activator.CreateInstance(dynType);
            obj.TaintedSQL = sql;

            //retrieve the tainted value and query the database
            string taintedSql = obj.TaintedSQL;
            var query = context.Users.FromSql(taintedSql);

            User user = query.SingleOrDefault();
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(context.Projects);
        }


        private static TypeBuilder GetTypeBuilder()
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType("Safe",
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }


    }
}