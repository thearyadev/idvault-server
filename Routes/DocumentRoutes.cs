public static class DocumentRoutes {
  public static void MapDocumentRoutes(this IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/document/document_list", async context => {});
    endpoints.MapGet("/documents/details/{document_id}", async context => {});
    endpoints.MapPost("/documents/add", async context => {});
    endpoints.MapDelete("/documents/delete/{document_id}", async context => {});

  }
}
