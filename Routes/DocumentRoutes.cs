using System.Text.Json;
using IdVaultServer.Data;
using IdVaultServer.Models;
using idvault_server.TokenValidator;
using Microsoft.EntityFrameworkCore;

public static class DocumentRoutes
{
    public static void MapDocumentRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/documents/document_list",
            async context =>
            {
                string? user = JwtTokenValidator.ValidateTokenAndGetCurrentUser(
                    context.Request.Headers
                );
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Unauthorized", } }
                    );
                    return;
                }

                using var dbContext = context.RequestServices.GetService<ApplicationDbContext>();
                var user_data = dbContext!.Users.FirstOrDefault(u => u.Username == user);

                if (user_data == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "User not found", } }
                    );
                    return;
                }

                var userWithDocuments = dbContext!
                    .Users.Include(u => u.Documents)
                    .FirstOrDefault(u => u.Username == user);

                var documents = new List<dynamic>();

                foreach (var document in userWithDocuments!.Documents!)
                {
                    if (document.DocumentType == "DriversLicense")
                    {
                        documents.Add(
                            dbContext!.DriversLicenses.FirstOrDefault(d =>
                                d.DocumentId == document.DocumentId
                            )!
                        );
                    }
                    else if (document.DocumentType == "BirthCertificate")
                    {
                        documents.Add(
                            dbContext!.BirthCertificates.FirstOrDefault(d =>
                                d.DocumentId == document.DocumentId
                            )!
                        );
                    }
                    else if (document.DocumentType == "Passport")
                    {
                        documents.Add(
                            dbContext!.Passports.FirstOrDefault(d =>
                                d.DocumentId == document.DocumentId
                            )!
                        );
                    }
                }
                context.Response.ContentType = "text/json";
                await context.Response.WriteAsJsonAsync(documents);
                return;
            }
        );
        endpoints.MapGet(
            "/documents/details/{document_id}",
            async context =>
            {
                string? user = JwtTokenValidator.ValidateTokenAndGetCurrentUser(
                    context.Request.Headers
                );
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Unauthorized", } }
                    );
                    return;
                }

                using var dbContext = context.RequestServices.GetService<ApplicationDbContext>();
                var user_data = dbContext!.Users.FirstOrDefault(u => u.Username == user);

                if (user_data == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "User not found", } }
                    );
                    return;
                }

                var document_id = context.Request.RouteValues["document_id"] as string;
                if (document_id == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Document ID not provided", } }
                    );
                    return;
                }

                var documentSuperClassObject = dbContext!.Documents.FirstOrDefault(d =>
                    d.DocumentId == int.Parse(document_id)
                );

                if (documentSuperClassObject == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Document not found", } }
                    );
                    return;
                }

                if (documentSuperClassObject.UserId != user_data.UserId)
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Forbidden", } }
                    );
                    return;
                }

                if (documentSuperClassObject.DocumentType == "DriversLicense")
                {
                    var document = dbContext!.DriversLicenses.FirstOrDefault(d =>
                        d.DocumentId == documentSuperClassObject.DocumentId
                    );
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (documentSuperClassObject.DocumentType == "BirthCertificate")
                {
                    var document = dbContext!.BirthCertificates.FirstOrDefault(d =>
                        d.DocumentId == documentSuperClassObject.DocumentId
                    );
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (documentSuperClassObject.DocumentType == "Passport")
                {
                    var document = dbContext!.Passports.FirstOrDefault(d =>
                        d.DocumentId == documentSuperClassObject.DocumentId
                    );
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
            }
        );
        endpoints.MapPost(
            "/documents/add/{document_type}",
            async context =>
            {
                string? user = JwtTokenValidator.ValidateTokenAndGetCurrentUser(
                    context.Request.Headers
                );
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Unauthorized", } }
                    );
                    return;
                }

                using var dbContext = context.RequestServices.GetService<ApplicationDbContext>();
                var user_data = dbContext!.Users.FirstOrDefault(u => u.Username == user);

                if (user_data == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "User not found", } }
                    );
                    return;
                }

                var document_type = context.Request.RouteValues["document_type"] as string;
                if (document_type == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Document type not provided", } }
                    );
                    return;
                }

                if (document_type == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Document type not provided", } }
                    );
                    return;
                }

                if (
                    document_type != "DriversLicense"
                    && document_type != "BirthCertificate"
                    && document_type != "Passport"
                )
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Invalid document type", } }
                    );
                    return;
                }

                if (document_type == "DriversLicense")
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
                    var document = await context.Request.ReadFromJsonAsync<DriversLicense>(options);
                    document!.DocumentType = "DriversLicense";
                    document!.CreationDate = DateTime.Now.ToUniversalTime();

                    if (document.ExpirationDate.HasValue)
                    {
                        document.ExpirationDate = document.ExpirationDate.Value.ToUniversalTime();
                    }

                    if (document.IssueDate.HasValue)
                    {
                        document.IssueDate = document.IssueDate.Value.ToUniversalTime();
                    }
                    if (document.DateOfBirth.HasValue)
                    {
                        document.DateOfBirth = document.DateOfBirth.Value.ToUniversalTime();
                    }

                    document!.UserId = user_data.UserId;
                    document!.User = user_data;
                    document!.ValidationStatus = "Pending";
                    dbContext!.DriversLicenses.Add(document);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (document_type == "BirthCertificate")
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };

                    var document = await context.Request.ReadFromJsonAsync<BirthCertificate>(
                        options
                    );
                    document!.DocumentType = "BirthCertificate";
                    document!.CreationDate = DateTime.Now.ToUniversalTime();
                    if (document.ExpirationDate.HasValue)
                    {
                        document.ExpirationDate = document.ExpirationDate.Value.ToUniversalTime();
                    }

                    if (document.IssueDate.HasValue)
                    {
                        document.IssueDate = document.IssueDate.Value.ToUniversalTime();
                    }
                    if (document.DateOfBirth.HasValue)
                    {
                        document.DateOfBirth = document.DateOfBirth.Value.ToUniversalTime();
                    }

                    document!.UserId = user_data.UserId;
                    document!.User = user_data;
                    document!.ValidationStatus = "Pending";
                    dbContext!.BirthCertificates.Add(document);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (document_type == "Passport")
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };

                    var document = await context.Request.ReadFromJsonAsync<Passport>(options);
                    document!.DocumentType = "Passport";
                    document!.CreationDate = DateTime.Now.ToUniversalTime();
                    if (document.ExpirationDate.HasValue)
                    {
                        document.ExpirationDate = document.ExpirationDate.Value.ToUniversalTime();
                    }

                    if (document.IssueDate.HasValue)
                    {
                        document.IssueDate = document.IssueDate.Value.ToUniversalTime();
                    }
                    if (document.DateOfBirth.HasValue)
                    {
                        document.DateOfBirth = document.DateOfBirth.Value.ToUniversalTime();
                    }

                    document!.UserId = user_data.UserId;
                    document!.User = user_data;
                    document!.ValidationStatus = "Pending";
                    dbContext!.Passports.Add(document);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
            }
        );
        endpoints.MapDelete(
            "/documents/delete/{document_id}",
            async context =>
            {
                string? user = JwtTokenValidator.ValidateTokenAndGetCurrentUser(
                    context.Request.Headers
                );
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Unauthorized", } }
                    );
                    return;
                }

                using var dbContext = context.RequestServices.GetService<ApplicationDbContext>();
                var user_data = dbContext!.Users.FirstOrDefault(u => u.Username == user);

                if (user_data == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "User not found", } }
                    );
                    return;
                }

                var document_id = context.Request.RouteValues["document_id"] as string;
                if (document_id == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Document ID not provided", } }
                    );
                    return;
                }

                var documentSuperClassObject = dbContext!.Documents.FirstOrDefault(d =>
                    d.DocumentId == int.Parse(document_id)
                );

                if (documentSuperClassObject == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Document not found", } }
                    );
                    return;
                }

                if (documentSuperClassObject.UserId != user_data.UserId)
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Forbidden", } }
                    );
                    return;
                }

                if (documentSuperClassObject.DocumentType == "DriversLicense")
                {
                    var document = dbContext!.DriversLicenses.FirstOrDefault(d =>
                        d.DocumentId == documentSuperClassObject.DocumentId
                    );
                    dbContext!.DriversLicenses.Remove(document!);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (documentSuperClassObject.DocumentType == "BirthCertificate")
                {
                    var document = dbContext!.BirthCertificates.FirstOrDefault(d =>
                        d.DocumentId == documentSuperClassObject.DocumentId
                    );
                    dbContext!.BirthCertificates.Remove(document!);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (documentSuperClassObject.DocumentType == "Passport")
                {
                    var document = dbContext!.Passports.FirstOrDefault(d =>
                        d.DocumentId == documentSuperClassObject.DocumentId
                    );
                    dbContext!.Passports.Remove(document!);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
            }
        );
    }
}
