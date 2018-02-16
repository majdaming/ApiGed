
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Helpers
{
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId == "ApiDocumentsPost")
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "uploadedFile",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Name",
                    In = "formData",
                    Description = "Nom du document",
                    Required = true,
                    Type = "string"
                });
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "TypeId",
                    In = "formData",
                    Description = "Type de document",
                    Required = true,
                    Type = "int"
                });
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "PublishedAt",
                    In = "formData",
                    Description = "Date de publication",
                    Required = true,
                    Type = "string"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }

    }
}
