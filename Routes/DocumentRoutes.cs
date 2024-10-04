using System.Text.Json;
using idvault_server.TokenValidator;
using IdVaultServer.Data;
using IdVaultServer.Models;
using Microsoft.EntityFrameworkCore;

public static class DocumentRoutes
{
    private static List<dynamic> MapDocsToChildren(
        User userWithDocuments,
        ApplicationDbContext dbContext
    )
    {
        var documents = new List<dynamic>();

        foreach (var document in userWithDocuments!.Documents!) // retrieve nested document data from document object.
        // must be done for all doc types
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
                    dbContext!.Passports.FirstOrDefault(d => d.DocumentId == document.DocumentId)!
                );
            }
        }
        return documents;
    }

    public static void MapDocumentRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/documents/document_list",
            async context =>
            {
                // auth + user
                string? user = JwtTokenValidator.ValidateTokenAndGetCurrentUser(
                    context.Request.Headers
                );
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Unauthorized" } }
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
                        new { error = new { message = "User not found" } }
                    );
                    return;
                }
                // auth + user end



                var userWithDocuments = dbContext! // get all documents
                    .Users.Include(u => u.Documents)
                    .FirstOrDefault(u => u.Username == user);

                var documents = MapDocsToChildren(userWithDocuments, dbContext);
                Console.WriteLine(documents.Count);

                context.Response.ContentType = "text/json";
                await context.Response.WriteAsJsonAsync(documents);
                return;
            }
        );

        endpoints.MapPost(
            "/documents/add/{document_type}",
            async context =>
            {
                // user auth
                string? user = JwtTokenValidator.ValidateTokenAndGetCurrentUser(
                    context.Request.Headers
                );
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Unauthorized" } }
                    );
                    return;
                }

                using var dbContext = context.RequestServices.GetService<ApplicationDbContext>();
                var user_data = dbContext!.Users.FirstOrDefault(u => u.Username == user);
                var document_type = context.Request.RouteValues["document_type"] as string;
                if (user_data == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "User not found" } }
                    );
                    return;
                }

                // user auth end


                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (document_type == "DriversLicense")
                {
                    var document = await context.Request.ReadFromJsonAsync<DriversLicense>(options);
                    document!.UserId = user_data.UserId;
                    document!.User = user_data;
                    dbContext!.DriversLicenses.Add(document);
                    dbContext!.SaveChanges();

                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (document_type == "BirthCertificate")
                {
                    var document = await context.Request.ReadFromJsonAsync<BirthCertificate>(
                        options
                    );
                    document!.UserId = user_data.UserId;
                    document!.User = user_data;
                    dbContext!.BirthCertificates.Add(document);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (document_type == "Passport")
                {
                    var document = await context.Request.ReadFromJsonAsync<Passport>(options);
                    document!.UserId = user_data.UserId;
                    document!.User = user_data;
                    dbContext!.Passports.Add(document);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
            }
        );
        endpoints.MapPost(
            "/documents/share/{document_type}/{recepient_user_name}",
            async context =>
            {
                // user auth
                string? user = JwtTokenValidator.ValidateTokenAndGetCurrentUser(
                    context.Request.Headers
                );
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Unauthorized" } }
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
                        new { error = new { message = "User not found" } }
                    );
                    return;
                }

                // user auth end

                // verify recipient exists
                var recipient_user_name =
                    context.Request.RouteValues["recepient_user_name"] as string;

                if (recipient_user_name == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Recipient not found" } }
                    );
                    return;
                }

                // lookup recipient
                var recipient = dbContext.Users.First(u => u.Username == recipient_user_name);
                if (recipient == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Recipient not found" } }
                    );
                    return;
                }

                // the json sent will be the document encrypted using the recipients public key.
                // recipient can then download that.
                var document_type = context.Request.RouteValues["document_type"] as string;

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (document_type == "DriversLicense")
                {
                    var document = await context.Request.ReadFromJsonAsync<DriversLicense>(options);
                    document!.UserId = recipient.UserId;
                    document!.User = recipient;
                    document!.DocumentId = 0; // ensure the db creates a new doc id.
                    dbContext!.DriversLicenses.Add(document);
                    dbContext!.SaveChanges();

                    context.Response.ContentType = "text/json";
                    dbContext.SharedDocuments.Add(
                        new SharedDocument
                        {
                            Id = 0,
                            DocumentId = document.DocumentId,
                            SenderUserId = user_data.UserId,
                            ReceiverUserId = recipient.UserId,
                            ExpiryDate = DateTime.UtcNow,
                        }
                    );
                    dbContext!.SaveChanges();
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (document_type == "BirthCertificate")
                {
                    var document = await context.Request.ReadFromJsonAsync<BirthCertificate>(
                        options
                    );
                    document!.UserId = recipient.UserId;
                    document!.User = recipient;
                    document!.DocumentId = 0; // ensure the db creates a new doc id.

                    dbContext!.BirthCertificates.Add(document);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    dbContext.SharedDocuments.Add(
                        new SharedDocument
                        {
                            Id = 0,
                            DocumentId = document.DocumentId,
                            SenderUserId = user_data.UserId,
                            ReceiverUserId = recipient.UserId,
                            ExpiryDate = DateTime.UtcNow,
                        }
                    );
                    dbContext!.SaveChanges();
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
                else if (document_type == "Passport")
                {
                    var document = await context.Request.ReadFromJsonAsync<Passport>(options);
                    document!.UserId = recipient.UserId;
                    document!.User = recipient;
                    document!.DocumentId = 0; // ensure the db creates a new doc id.

                    dbContext!.Passports.Add(document);
                    dbContext!.SaveChanges();
                    context.Response.ContentType = "text/json";
                    dbContext.SharedDocuments.Add(
                        new SharedDocument
                        {
                            Id = 0,
                            DocumentId = document.DocumentId,
                            SenderUserId = user_data.UserId,
                            ReceiverUserId = recipient.UserId,
                            ExpiryDate = DateTime.UtcNow,
                        }
                    );
                    dbContext!.SaveChanges();
                    await context.Response.WriteAsJsonAsync(document);
                    return;
                }
            }
        );

        endpoints.MapDelete(
            "/documents/delete/{document_id}",
            async context =>
            {
                // user auth
                string? user = JwtTokenValidator.ValidateTokenAndGetCurrentUser(
                    context.Request.Headers
                );
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Unauthorized" } }
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
                        new { error = new { message = "User not found" } }
                    );
                    return;
                }
                // user auth end


                var document_id = context.Request.RouteValues["document_id"] as string;
                if (document_id == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Document ID not provided" } }
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
                        new { error = new { message = "Document not found" } }
                    );
                    return;
                }

                if (documentSuperClassObject.UserId != user_data.UserId)
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Forbidden" } }
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
                        new { error = new { message = "Unauthorized" } }
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
                        new { error = new { message = "User not found" } }
                    );
                    return;
                }

                var document_id = context.Request.RouteValues["document_id"] as string;
                if (document_id == null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Document ID not provided" } }
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
                        new { error = new { message = "Document not found" } }
                    );
                    return;
                }

                if (documentSuperClassObject.UserId != user_data.UserId)
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Forbidden" } }
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

        endpoints.MapGet(
            "/users/recipient/{recipient}/publicKey",
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
                        new { error = new { message = "Unauthorized" } }
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
                        new { error = new { message = "User not found" } }
                    );
                    return;
                }

                var recipientUserName = context.Request.RouteValues["recipient"] as string;
                var recipient = dbContext!.Users.FirstOrDefault(u =>
                    u.Username == recipientUserName
                );
                if (recipient == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Recipient not found" } }
                    );
                    return;
                }

                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/json";
                await context.Response.WriteAsJsonAsync(new { publicKey = recipient.PublicKey });
            }
        );
        endpoints.MapGet(
            "/documents/shared",
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
                        new { error = new { message = "Unauthorized" } }
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
                        new { error = new { message = "User not found" } }
                    );
                    return;
                }




                // 1. get all documents in share table that have this userid
                var sharedDocuments = dbContext!.SharedDocuments.Where(s => s.ReceiverUserId == user_data.UserId).ToList();
                // 2. for each shared document, get the document
                var documents = new List<Document>();
                foreach (var sharedDoc in sharedDocuments)
                {
                    documents.Add(dbContext!.Documents.FirstOrDefault(d => d.DocumentId == sharedDoc.DocumentId));
                }

                var documentsExpanded = new List<dynamic>();
                foreach (var docMain in documents)
                {
                    if (docMain.DocumentType == "DriversLicense")
                    {
                        documentsExpanded.Add(
                            dbContext!.DriversLicenses.FirstOrDefault(d =>
                                d.DocumentId == docMain.DocumentId
                            )!
                        );
                    }
                    else if (docMain.DocumentType == "BirthCertificate")
                    {
                        documentsExpanded.Add(
                            dbContext!.BirthCertificates.FirstOrDefault(d =>
                                d.DocumentId == docMain.DocumentId
                            )!
                        );
                    }
                    else if (docMain.DocumentType == "Passport")
                    {
                        documentsExpanded.Add(
                            dbContext!.Passports.FirstOrDefault(d => d.DocumentId == docMain.DocumentId)!
                        );
                    }

                }


                context.Response.ContentType = "text/json";
                await context.Response.WriteAsJsonAsync(documentsExpanded);
                return;
            }
        );
    }
}
