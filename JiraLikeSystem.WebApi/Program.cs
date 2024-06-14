using JiraLikeSystem;

var builder = WebApplication.CreateBuilder();

//Configure services
builder = builder.ConfigureApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseHttpsRedirection();
app.UseRouting();

// //============ CORS  =============
app.UseCors("AllowAll");

//============ Authentication & Authorization  =============
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
